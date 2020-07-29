using LittlePdf.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LittlePdf.Test.Functional
{
    public class FunctionalTests
    {
        [Fact]
        public async Task WriterTest()
        {
            var documentCatalogDict = new PdfDictionary();
            var documentCatalog = new PdfIndirectObject(documentCatalogDict);

            var infoDict = new PdfDictionary();
            var info = new PdfIndirectObject(infoDict);

            var pageTreeDict = new PdfDictionary();
            var pageTree = new PdfIndirectObject(pageTreeDict);
            var pageReferences = new PdfArray();

            documentCatalogDict.Add("Type", new PdfName("Catalog"));
            documentCatalogDict.Add("Pages", new PdfIndirectObjectReference(pageTree));

            infoDict.Add("Producer", "LittlePdf");
            infoDict.Add("CreationDate", new PdfDate(DateTime.UtcNow));
            infoDict.Add("ModDate", new PdfDate(DateTime.UtcNow));
            infoDict.Add("Title", new PdfString("Test Pdf"));
            infoDict.Add("Author", new PdfString("Salil Ponde"));

            pageTreeDict.Add("Type", new PdfName("Pages"));
            pageTreeDict.Add("Kids", pageReferences);
            pageTreeDict.Add("Count", new PdfInteger(0));
            pageTreeDict.Add("MediaBox", new PdfArray(new List<PdfObject> { new PdfReal(0), new PdfReal(0), new PdfReal(595), new PdfReal(842) }));

            var font1Dict = new PdfDictionary();
            var font1 = new PdfIndirectObject(font1Dict);
            font1Dict.Add("Type", new PdfName("Font"));
            font1Dict.Add("Subtype", new PdfName("Type1"));
            font1Dict.Add("BaseFont", new PdfName("Helvetica-Oblique"));
            font1Dict.Add("Encoding", new PdfName("WinAnsiEncoding"));

            var fontsDict = new PdfDictionary();
            var resourcesDict = new PdfDictionary();
            resourcesDict.Add("Font", fontsDict);
            fontsDict.Add("F1", new PdfIndirectObjectReference(font1));
            pageTreeDict.Add("Resources", resourcesDict);

            var pageDict = new PdfDictionary();
            var page = new PdfIndirectObject(pageDict);
            pageReferences.Add(new PdfIndirectObjectReference(page));
            pageDict.Add("Type", new PdfName("Page"));
            pageDict.Add("Parent", new PdfIndirectObjectReference(pageTree));
            var c = Encoding.ASCII.GetBytes("BT /F1 24 Tf 175 720 Td (Hello World!)Tj ET");
            var pageContentStream = new PdfStream(c, new PdfFlatDecodeStreamFilter());
            var pageContent = new PdfIndirectObject(pageContentStream);
            pageDict.Add("Contents", new PdfIndirectObjectReference(pageContent));

            var page2Dict = new PdfDictionary();
            var page2 = new PdfIndirectObject(page2Dict);
            pageReferences.Add(new PdfIndirectObjectReference(page2));
            page2Dict.Add("Type", new PdfName("Page"));
            page2Dict.Add("Parent", new PdfIndirectObjectReference(pageTree));
            page2Dict.Add("MediaBox", new PdfArray(new List<PdfObject> { new PdfReal(0), new PdfReal(0), new PdfReal(842), new PdfReal(595) }));

            // Write
            var fileName = @"c:\temp\d.pdf";
            File.Delete(fileName);
            var stream = File.Create(fileName);

            var writer = new PdfWriter(stream);
            await writer.WriteAsync(documentCatalog);
            await writer.WriteAsync(info);
            await writer.WriteAsync(pageTree);
            await writer.WriteAsync(font1);
            await writer.WriteAsync(page);
            await writer.WriteAsync(pageContent);
            await writer.WriteAsync(page2);
            await writer.CloseAsync(documentCatalog, info);
        }
    }
}
