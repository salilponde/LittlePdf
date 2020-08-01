using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf.HighLevel
{
    public class PdfDocumentCatalog : PdfIndirectObject
    {
        private readonly PdfName _type = new PdfName("Catalog");
        public PdfIndirectObjectReference PagesReference { get; set; }

        public override async Task WriteAsync(Stream stream)
        {
            var pdfDictionary = new PdfDictionary();
            pdfDictionary.Items.Clear();
            pdfDictionary.Add(PdfSpec.Names.Type, _type);
            pdfDictionary.Add(PdfSpec.Names.Pages, PagesReference);

            Value = pdfDictionary;

            await base.WriteAsync(stream);
        }
    }
}
