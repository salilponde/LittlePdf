using LittlePdf.Pdf;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf
{
    public class Document
    {
        public double DefaultPageWidth { get; } = 595;
        public double DefaultPageHeight { get; } = 842;
        public List<Page> Pages { get; } = new List<Page>();
        public List<Font> Fonts { get; } = new List<Font>();
        private int _nextFontId = 1;

        public Page AddPage()
        {
            var page = new Page(this);
            Pages.Add(page);
            return page;
        }

        public Font CreateFont(string name)
        {
            var font = new Font(_nextFontId++, name);
            Fonts.Add(font);
            return font;
        }

        public async Task SaveAsync(string fileName)
        {
            var stream = File.Create(fileName);
            await SaveAsync(stream);
        }

        public async Task SaveAsync(Stream stream)
        {
            var documentCatalogProps = new PdfDictionary();
            var documentCatalog = new PdfIndirectObject(documentCatalogProps);
            var pageTreeProps = new PdfDictionary();
            var pageTree = new PdfIndirectObject(pageTreeProps);

            documentCatalogProps.Add("Type", new PdfName("Catalog"));
            documentCatalogProps.Add("Pages", pageTree.Reference);

            var fontToIndirectObjects = new Dictionary<Font, PdfIndirectObject>();
            foreach (var font in Fonts)
            {
                var fontProps = new PdfDictionary();
                var fontIndirectObject = new PdfIndirectObject(fontProps);
                fontToIndirectObjects.Add(font, fontIndirectObject);
                fontProps.Add("Type", new PdfName("Font"));
                fontProps.Add("Subtype", new PdfName(font.Type));
                fontProps.Add("BaseFont", new PdfName(font.Name));
                fontProps.Add("Encoding", new PdfName(font.Encoding));
            }

            pageTreeProps.Add("Type", new PdfName("Pages"));
            pageTreeProps.Add("MediaBox", new PdfArray(new List<PdfObject> { new PdfReal(0), new PdfReal(0), new PdfReal(DefaultPageWidth), new PdfReal(DefaultPageHeight) }));
            pageTreeProps.Add("Count", new PdfInteger(Pages.Count));
            var resources = new PdfDictionary();
            var fontResources = new PdfDictionary();
            resources.Add("Font", fontResources);
            foreach (var font in Fonts)
            {
                fontResources.Add($"F{font.Id}", fontToIndirectObjects[font].Reference);
            }
            pageTreeProps.Add("Resouces", fontResources);

            var pages = new List<PdfIndirectObject>();
            var pageRefs = new List<PdfIndirectObjectReference>();
            var contentObjects = new List<PdfIndirectObject>();
            foreach (var page in Pages)
            {
                var pageProps = new PdfDictionary();
                var pageIndirectObject = new PdfIndirectObject(pageProps);
                pages.Add(pageIndirectObject);
                pageRefs.Add(pageIndirectObject.Reference);

                pageProps.Add("Type", new PdfName("Page"));
                pageProps.Add("Parent", pageTree.Reference);
                if (page.Unit != 1.0)
                {
                    pageProps.Add("UserUnit", new PdfReal(page.Unit));
                }
                if (page.Width != DefaultPageWidth || page.Height != DefaultPageHeight)
                {
                    pageProps.Add("MediaBox", new PdfArray(new List<PdfObject> { new PdfReal(0), new PdfReal(0), new PdfReal(page.Width), new PdfReal(page.Height) }));
                }

                var instructions = page.Paint();
                if (!string.IsNullOrWhiteSpace(instructions))
                {
                    var contentStream = new PdfStream(Encoding.ASCII.GetBytes(instructions));
                    var contentIndirectObject = new PdfIndirectObject(contentStream);
                    contentObjects.Add(contentIndirectObject);
                    pageProps.Add("Contents", new PdfIndirectObjectReference(contentIndirectObject));
                }
            }
            pageTreeProps.Add("Kids", new PdfArray(pageRefs.Select(x => (PdfObject)x).ToList()));

            var writer = new PdfWriter(stream);
            await writer.WriteAsync(documentCatalog);
            await writer.WriteAsync(pageTree);
            foreach (var pair in fontToIndirectObjects)
            {
                await writer.WriteAsync(pair.Value);
            }
            foreach (var page in pages)
            {
                await writer.WriteAsync(page);
            }
            foreach (var contentObject in contentObjects)
            {
                await writer.WriteAsync(contentObject);
            }
            await writer.CloseAsync(documentCatalog, null);
        }
    }
}
