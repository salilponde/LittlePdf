using LittlePdf.Core;
using LittlePdf.Core.CrossReferencing;
using LittlePdf.Core.Objects;
using LittlePdf.Core.Writers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf
{
    public class Document
    {
        public int PageWidth { get; set; } = 595;
        public int PageHeight { get; set; } = 842;
        public List<Page> Pages { get; } = new List<Page>();

        public Page AddPage()
        {
            var page = new Page();
            page.Width = PageWidth;
            page.Height = PageHeight;
            Pages.Add(page);
            return page;
        }

        public void AddFont()
        {
        }

        public async Task SaveAsync(string filename)
        {
            var fileStream = File.Create(filename);
            await SaveAsync(fileStream);
            fileStream.Close();
        }

        private Stream _stream;
        private List<PdfCrossReferenceEntry> _pdfCrossReferenceEntries;
        private int _objectNumber;
        private long _crossReferenceTableOffset;

        public async Task SaveAsync(Stream stream)
        {
            _stream = stream;
            _pdfCrossReferenceEntries = new List<PdfCrossReferenceEntry>()
            {
                new PdfCrossReferenceEntry(0, 65535, CrossReferenceEntryType.Free),
            };

            var pdfWriter = new PdfWriter(stream);
            await WriteHeader(pdfWriter);

            var pageIndirectObjects = new List<PdfIndirectObject>();
            _objectNumber = 4;
            foreach (var page in Pages)
            {
                pageIndirectObjects.Add(new PdfIndirectObject(_objectNumber++, 0, new PdfDictionary(new Dictionary<PdfName, PdfObject>
                {
                    { new PdfName("Type"), new PdfName("Page") },
                    { new PdfName("Parent"), new PdfIndirectObjectReference(2, 0) },
                    { new PdfName("MediaBox"), new PdfArray(new List<PdfObject>
                    {
                        new PdfInteger(0), new PdfInteger(0), new PdfInteger(page.Width), new PdfInteger(page.Height)
                    })},
                     { new PdfName("Resources"), new PdfDictionary(new Dictionary<PdfName, PdfObject>()) },
                })));
            }

            await WriteDocumentCatalog(pdfWriter);  // Object Number 1
            await WritePageTree(pdfWriter);         // Object Number 2
            await WriteInfo(pdfWriter);         // Object Number 3
            // Pages
            foreach (var pageIndirectObject in pageIndirectObjects)
            {
                var pageOffset = stream.Position;
                await pdfWriter.WriteAsync(pageIndirectObject);
                _pdfCrossReferenceEntries.Add(new PdfCrossReferenceEntry(pageOffset, 0, CrossReferenceEntryType.InUse));
            }
            await WriteCrossReferenceTable(pdfWriter);
            await WriteTrailer(pdfWriter);
        }

        private async Task WriteHeader(PdfWriter writer)
        {
            await writer.WriteAsync(new PdfComment(Encoding.ASCII.GetBytes("PDF-1.7")));
            await writer.WriteAsync(new PdfComment(new byte[] { 128, 128, 128, 128 }));     // For FTP programs to identify file as binary
        }

        private async Task WriteDocumentCatalog(PdfWriter writer)
        {
            var documentCatalogDictionary = new PdfDictionary(new Dictionary<PdfName, PdfObject>
            {
                { new PdfName("Type"), new PdfName("Catalog") },
                { new PdfName("Pages"), new PdfIndirectObjectReference(2, 0) }
            });
            var documentCatalog = new PdfIndirectObject(1, 0, documentCatalogDictionary);

            var offset = _stream.Position;
            await writer.WriteAsync(documentCatalog);
            _pdfCrossReferenceEntries.Add(new PdfCrossReferenceEntry(offset, 0, CrossReferenceEntryType.InUse));
        }

        private async Task WritePageTree(PdfWriter writer)
        {
            var pageIndirectReferences = new List<PdfObject>();
            for (int i = 4; i < _objectNumber; i++)
            {
                pageIndirectReferences.Add(new PdfIndirectObjectReference(i, 0));
            }

            var pageTreeDictionary = new PdfDictionary(new Dictionary<PdfName, PdfObject>
            {
                { new PdfName("Type"), new PdfName("Pages") },
                { new PdfName("Kids"), new PdfArray(pageIndirectReferences) },
                { new PdfName("Count"), new PdfInteger(Pages.Count) },
            });

            var offset = _stream.Position;
            var pageTree = new PdfIndirectObject(2, 0, pageTreeDictionary);
            await writer.WriteAsync(pageTree);
            _pdfCrossReferenceEntries.Add(new PdfCrossReferenceEntry(offset, 0, CrossReferenceEntryType.InUse));
        }

        private async Task WriteInfo(PdfWriter writer)
        {
            var dictionary = new PdfDictionary(new Dictionary<PdfName, PdfObject>
            {
                { new PdfName("Producer"), new PdfString("LittlePdf") }
            });
            var info = new PdfIndirectObject(3, 0, dictionary);

            var offset = _stream.Position;
            await writer.WriteAsync(info);
            _pdfCrossReferenceEntries.Add(new PdfCrossReferenceEntry(offset, 0, CrossReferenceEntryType.InUse));
        }

        private async Task WriteCrossReferenceTable(PdfWriter writer)
        {
            var pdfCrossReferenceSubSection = new PdfCrossReferenceSubSection(0, _pdfCrossReferenceEntries.Count, _pdfCrossReferenceEntries);
            var pdfCrossReferenceSection = new PdfCrossReferenceSection(new List<PdfCrossReferenceSubSection> { pdfCrossReferenceSubSection });
            var pdfCrossReferenceSections = new List<PdfCrossReferenceSection> { pdfCrossReferenceSection };
            var pdfCrossReferenceTable = new PdfCrossReferenceTable(pdfCrossReferenceSections);

            _crossReferenceTableOffset = _stream.Position;
            await writer.WriteAsync(pdfCrossReferenceTable);
        }

        private async Task WriteTrailer(PdfWriter writer)
        {
            var size = _pdfCrossReferenceEntries.Count;
            var pdfTrailer = new PdfTrailer(size, new PdfIndirectObjectReference(1, 0), new PdfIndirectObjectReference(3, 0), _crossReferenceTableOffset);
            await writer.WriteAsync(pdfTrailer);
        }
    }
}
