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
        public PdfIndirectObjectReference FontDescriptor { get; set; }
        public PdfInteger FirstChar { get; set; }
        public PdfInteger LastChar { get; set; }
        public PdfIndirectObjectReference Widths { get; set; }

        public override async Task WriteAsync(Stream stream)
        {
            var pdfDictionary = new PdfDictionary();
            pdfDictionary.Add(PdfSpec.Names.Type, _type);
            if (Subtype != null) pdfDictionary.Add("Subtype", Subtype);
            if (BaseFont != null) pdfDictionary.Add("BaseFont", BaseFont);
            if (Encoding != null) pdfDictionary.Add("Encoding", Encoding);
            if (FontDescriptor != null) pdfDictionary.Add("FontDescriptor", FontDescriptor);
            if (FirstChar != null) pdfDictionary.Add("FirstChar", FirstChar);
            if (LastChar != null) pdfDictionary.Add("LastChar", LastChar);
            if (Widths != null) pdfDictionary.Add("Widths", Widths);

            Value = pdfDictionary;

            await base.WriteAsync(stream);
        }
    }
}
