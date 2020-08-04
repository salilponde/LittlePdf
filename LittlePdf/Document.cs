using LittlePdf.Core;
using LittlePdf.Pdf;
using LittlePdf.Pdf.HighLevel;
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

        private IAxes GetDefaultAxes()
        {
            return new XRightYDownAxes(DefaultPageWidth, DefaultPageHeight);
        }

        public Page AddPage()
        {
            var page = new Page(this, GetDefaultAxes());
            Pages.Add(page);
            return page;
        }

        public Page AddPage(int width, int height, IAxes axes = null)
        {
            if (axes == null) axes = GetDefaultAxes();

            var page = new Page(this, axes);
            page.SetSize(width, height);
            Pages.Add(page);
            return page;
        }

        public void AddFont(Font font)
        {
            Fonts.Add(font);
        }

        public async Task SaveAsync(string fileName)
        {
            var stream = File.Create(fileName);
            await SaveAsync(stream);
        }

        private int Normalize(int value, double factor)
        {
            return (int)(value * factor);
        }

        private double Normalize(double value, double factor)
        {
            return value * factor;
        }

        public async Task SaveAsync(Stream stream)
        {
            var pdfDocumentCatalog = new PdfDocumentCatalog();
            var pdfPageTree = new PdfPageTree();
            pdfDocumentCatalog.PagesReference = pdfPageTree.Reference;

            var pdfFonts = new List<PdfFont>();
            var pdfFontDescriptors = new List<PdfFontDescriptor>();
            var pdfCharWidthsObjects = new List<PdfCharWidths>();
            var pdfFontStreams = new List<PdfFontStream>();
            var fontToPdfFont = new Dictionary<Font, PdfFont>();
            foreach (var font in Fonts)
            {
                var fontStream = new PdfFontStream();
                fontStream.Filter = PdfStreamFilter.FlateDecode;
                fontStream.Stream = new PdfStream(File.ReadAllBytes(font.FileName));
                pdfFontStreams.Add(fontStream);

                var fontInfo = font.FontInfo;
                var factor = 1000.0 / fontInfo.HeadTable.UnitsPerEm;

                var charWidths = new int[129];
                for (int i = 0; i < 129; i++)
                {
                    fontInfo.CharacterMapTable.Encodings.Last().Value.TryGetGlyphId(i, out var glyphIndex);
                    charWidths[i] = fontInfo.HorizontalMetricsTable.HMetrics[glyphIndex].AdvanceWidth;  // Don't normalize

                    //fontInfo.CharacterMapTable.Encodings.Last().Value.TryGetGlyphId(i, out var glyphIndex);
                    //var blockLocation = fontInfo.IndexLocationTable.GetLocation(glyphIndex);
                    //var glyphHeader = fontInfo.GlyphTable.ReadHeader(blockLocation);
                    //charWidths[i] = glyphHeader.XMax - glyphHeader.XMin;
                }
                var pdfCharWidths = new PdfCharWidths { Widths = new PdfArray(charWidths.Select(x => (PdfObject)new PdfReal(Normalize(x, factor))).ToList()) };
                pdfCharWidthsObjects.Add(pdfCharWidths);

                var pdfFontDescriptor = new PdfFontDescriptor
                {
                    Ascent = new PdfInteger(Normalize(fontInfo.HorizontalHeaderTable.Ascender, factor)),
                    Descent = new PdfInteger(Normalize(fontInfo.HorizontalHeaderTable.Descender, factor)),
                    FontName = new PdfName(font.Name),
                    FontBBox = new PdfRectangle(Normalize(fontInfo.HeadTable.XMin, factor),
                                                Normalize(fontInfo.HeadTable.YMin, factor),
                                                Normalize(fontInfo.HeadTable.XMax, factor),
                                                Normalize(fontInfo.HeadTable.YMax, factor)),
                    FontWeight = new PdfInteger(fontInfo.Os2Table.UsWeightClass),
                    AvgWidth = new PdfInteger(Normalize(fontInfo.Os2Table.XAvgCharWidth, factor)),
                    CapHeight = new PdfInteger(Normalize(fontInfo.Os2Table.SCapHeight, factor)),
                    ItalicAngle = new PdfReal(Normalize((double)fontInfo.PostScriptTable.ItalicAngle, factor)),
                    // MaxWidth = new PdfInteger(0),        // TODO
                    // XHeight = new PdfInteger(0),         // TODO
                    StemV = new PdfInteger(50),             // TODO
                    Flags = new PdfInteger(32),              // TODO
                    FontFile2 = new PdfIndirectObjectReference(fontStream)

                };
                pdfFontDescriptors.Add(pdfFontDescriptor);

                var pdfFont = new PdfFont
                {
                    Subtype = new PdfName(font.Type),
                    BaseFont = new PdfName(font.Name),
                    Encoding = new PdfName(font.Encoding.ToString()),
                    FontDescriptor = pdfFontDescriptor.Reference,
                    Widths = pdfCharWidths.Reference,
                    FirstChar = new PdfInteger(0),
                    LastChar = new PdfInteger(127)
                };
                pdfFonts.Add(pdfFont);
                fontToPdfFont[font] = pdfFont;
            }

            pdfPageTree.MediaBox = new PdfRectangle(0, 0, DefaultPageWidth, DefaultPageHeight);
            pdfPageTree.Resources = new PdfResources
            {
                Fonts = new PdfFontResources()
            };
            foreach (var font in Fonts)
            {
                pdfPageTree.Resources.Fonts.Add($"F{_nextFontId++}", fontToPdfFont[font].Reference);
            }

            var pdfContentStreams = new List<PdfContentStream>();
            var pdfPages = new List<PdfPage>();
            foreach (var page in Pages)
            {
                var pdfPage = new PdfPage();
                pdfPages.Add(pdfPage);

                pdfPage.Parent = pdfPageTree.Reference;
                if (page.Unit != 1.0) pdfPage.UserUnit = new PdfReal(page.Unit);
                if (page.Width != DefaultPageWidth || page.Height != DefaultPageHeight)
                {
                    pdfPage.MediaBox = new PdfRectangle(0, 0, page.Width, page.Height);
                }

                var instructions = page.Paint();
                if (!string.IsNullOrWhiteSpace(instructions))
                {
                    var pdfContentStream = new PdfContentStream
                    {
                        Stream = new PdfStream(Encoding.ASCII.GetBytes(instructions)),
                        // Filter = PdfStreamFilter.FlateDecode // Temporarily no using. Uncomment after development complete
                    };
                    pdfContentStreams.Add(pdfContentStream);
                    pdfPage.Contents = pdfContentStream.Reference;
                }
            }
            pdfPageTree.Kids = pdfPages.Select(x => x.Reference).ToList();

            var writer = new PdfWriter(stream);
            await writer.WriteAsync(pdfDocumentCatalog);
            await writer.WriteAsync(pdfPageTree);
            foreach (var pdfFont in pdfFonts)
            {
                await writer.WriteAsync(pdfFont);
            }
            foreach (var pdfFontDescriptor in pdfFontDescriptors)
            {
                await writer.WriteAsync(pdfFontDescriptor);
            }
            foreach (var pdfCharWidths in pdfCharWidthsObjects)
            {
                await writer.WriteAsync(pdfCharWidths);
            }
            foreach (var pdfFontStream in pdfFontStreams)
            {
                await writer.WriteAsync(pdfFontStream);
            }
            foreach (var pdfPage in pdfPages)
            {
                await writer.WriteAsync(pdfPage);
            }
            foreach (var pdfContentStream in pdfContentStreams)
            {
                await writer.WriteAsync(pdfContentStream);
            }
            await writer.CloseAsync(pdfDocumentCatalog, null);
        }
    }
}
