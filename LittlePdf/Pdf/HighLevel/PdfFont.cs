using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf.HighLevel
{
    public class PdfFont : PdfIndirectObject
    {
        private readonly PdfName _type = new PdfName("Font");
        public PdfName Subtype { get; set; }
        public PdfName BaseFont { get; set; }
        public PdfName Encoding { get; set; }

        public override async Task WriteAsync(Stream stream)
        {
            var pdfDictionary = new PdfDictionary();
            pdfDictionary.Add(PdfSpec.Names.Type, _type);
            if (Subtype != null) pdfDictionary.Add("Subtype", Subtype);
            if (BaseFont != null) pdfDictionary.Add("BaseFont", BaseFont);
            if (Encoding != null) pdfDictionary.Add("Encoding", Encoding);

            Value = pdfDictionary;

            await base.WriteAsync(stream);
        }
    }
}
