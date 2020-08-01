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
            var pdfDocumentCatalog = new PdfDocumentCatalog();
            var pdfPageTree = new PdfPageTree();
            pdfDocumentCatalog.PagesReference = pdfPageTree.Reference;

            var pdfFonts = new List<PdfFont>();
            var fontToPdfFont = new Dictionary<Font, PdfFont>();
            foreach (var font in Fonts)
            {
                var pdfFont = new PdfFont
                {
                    Subtype = new PdfName(font.Type),
                    BaseFont = new PdfName(font.Name),
                    Encoding = new PdfName(font.Encoding)
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
                pdfPageTree.Resources.Fonts.Add($"F{font.Id}", fontToPdfFont[font].Reference);
            }

            var pdfContentStreams = new List<PdfContenStream>();
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
                    var pdfContentStream = new PdfContenStream
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
