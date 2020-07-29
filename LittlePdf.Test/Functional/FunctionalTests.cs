using LittlePdf.Pdf;
using LittlePdf.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LittlePdf.Test.Functional
{
    public class FunctionalTests
    {
        [Fact]
        public void TokenizerTest()
        {
            var tokenizer = new Tokenizer("Eris is the      second-largest  known\tdwarf-a-complicate-degenerating-progressing-peculiar-rhombosis-cultivating planet\r\nin the Solar System, slightly smaller by volume than the dwarf planet Pluto, although it is 27 percent more massive. Discovered in January 2005 by a team based at Palomar Observatory, it was named after Eris, the Greek goddess of strife and discord. The ninth-most-massive object directly orbiting the Sun, Eris is the largest object in the Solar System that has not been visited by a spacecraft.\r\n\nIt is a member of a high-eccentricity population known as the scattered disk and has one known moon, Dysnomia. It is about 96 astronomical units (14.4 billion kilometres; 8.9 billion miles) from the Sun, roughly three times as far away as Pluto. Except for some long-period comets, Eris and Dysnomia were the most distant known natural objects in the Solar System until 2018 VG18 was discovered in 2018.");

            string token = null;
            while ((token = tokenizer.Next()) != null)
            {
            }
        }

        [Fact]
        public void TextWrapperTest()
        {
            var tw = new TextWrapper(40);
            var lines = tw.Wrap("Eris is the second-largest known dwarf-a-complicate-degenerating-progressing-peculiar-rhombosis-cultivating planet in the Solar System, slightly smaller by volume than the dwarf planet Pluto, although it is 27 percent more massive. Discovered in January 2005 by a team based at Palomar Observatory, it was named after Eris, the Greek goddess of strife and discord. The ninth-most-massive object directly orbiting the Sun, Eris is the largest object in the Solar System that has not been visited by a spacecraft.\r\n\nIt is a member of a high-eccentricity population known as the scattered disk and has one known moon, Dysnomia. It is about 96 astronomical units (14.4 billion kilometres; 8.9 billion miles) from the Sun, roughly three times as far away as Pluto. Except for some long-period comets, Eris and Dysnomia were the most distant known natural objects in the Solar System until 2018 VG18 was discovered in 2018.");

            var sb = new StringBuilder();
            foreach (var line in lines)
            {
                // Right align
                for (int i = 0; i < line.RemainingWidth; i++) { sb.Append(" "); }

                foreach (var word in line.Words)
                {
                    sb.Append(word);
                    if (word != line.Words.Last())
                    {
                        sb.Append(" ");
                    }
                }
                sb.Append("    ");
                sb.Append("\n");
            }
            var wrappedText = sb.ToString();
        }

        [Fact]
        public async Task DocumentTest()
        {
            //var p = new PrivateFontCollection();
            //var fontFiles = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "Calibri*");
            //foreach (var fontFile in fontFiles)
            //{
            //    p.AddFontFile(fontFile);
            //}

            //var installedFonts = new InstalledFontCollection();
            //var families = installedFonts.Families;

            var fileName = @"c:\temp\d.pdf";
            File.Delete(fileName);

            var document = new Document();
            var fHelvetica = document.CreateFont("Helvetica");
            var fHelveticaBold = document.CreateFont("Helvetica-Bold");
            var fHelveticaItalic = document.CreateFont("Helvetica-Oblique");

            var page1 = document.AddPage();
            var page2 = document.AddPage();
            page2.Rotate();

            await document.SaveAsync(fileName);
        }

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
