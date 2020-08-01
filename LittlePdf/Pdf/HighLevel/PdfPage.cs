using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf.HighLevel
{
    public class PdfPage : PdfIndirectObject
    {
        private readonly PdfName _type = new PdfName("Page");
        public PdfIndirectObjectReference Parent { get; set; }
        public PdfRectangle MediaBox { get; set; }
        public PdfReal UserUnit { get; set; }
        public PdfIndirectObjectReference Contents { get; set; }

        public override async Task WriteAsync(Stream stream)
        {
            var pdfDictionary = new PdfDictionary();
            pdfDictionary.Add(PdfSpec.Names.Type, _type);
            if (Parent != null) pdfDictionary.Add("Parent", Parent);
            if (MediaBox != null) pdfDictionary.Add("MediaBox", MediaBox);
            if (UserUnit != null) pdfDictionary.Add("UserUnit", UserUnit);
            if (Contents != null) pdfDictionary.Add("Contents", Contents);

            Value = pdfDictionary;

            await base.WriteAsync(stream);
        }
    }
}
