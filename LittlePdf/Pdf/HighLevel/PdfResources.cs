using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf.HighLevel
{
    public class PdfResources : PdfObject
    {
        public PdfFontResources Fonts { get; set; }

        public override async Task WriteAsync(Stream stream)
        {
            var pdfDictionary = new PdfDictionary();
            if (Fonts != null)
            {
                pdfDictionary.Add(PdfSpec.Names.Font, Fonts);
            }
            await pdfDictionary.WriteAsync(stream);
        }
    }
}
