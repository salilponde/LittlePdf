using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf.HighLevel
{
    public class PdfFontDescriptor : PdfIndirectObject
    {
        private readonly PdfName _type = new PdfName("FontDescriptor");
        public PdfName FontName { get; set; }
        public PdfInteger Flags { get; set; }
        public PdfReal ItalicAngle { get; set; }
        public PdfInteger Ascent { get; set; }
        public PdfInteger Descent { get; set; }
        public PdfInteger CapHeight { get; set; }
        public PdfInteger AvgWidth { get; set; }
        public PdfInteger MaxWidth { get; set; }
        public PdfInteger FontWeight { get; set; }
        public PdfInteger XHeight { get; set; }
        public PdfInteger StemV { get; set; }
        public PdfRectangle FontBBox { get; set; }
        public PdfIndirectObjectReference FontFile2 { get; set; }

        public override async Task WriteAsync(Stream stream)
        {
            var pdfDictionary = new PdfDictionary();
            pdfDictionary.Add(PdfSpec.Names.Type, _type);
            if (FontName != null) pdfDictionary.Add("FontName", FontName);
            if (Flags != null) pdfDictionary.Add("Flags", Flags);
            if (ItalicAngle != null) pdfDictionary.Add("Encoding", ItalicAngle);
            if (Ascent != null) pdfDictionary.Add("Ascent", Ascent);
            if (Descent != null) pdfDictionary.Add("Descent", Descent);
            if (CapHeight != null) pdfDictionary.Add("CapHeight", CapHeight);
            if (AvgWidth != null) pdfDictionary.Add("AvgWidth", AvgWidth);
            if (MaxWidth != null) pdfDictionary.Add("MaxWidth", MaxWidth);
            if (FontWeight != null) pdfDictionary.Add("FontWeight", FontWeight);
            if (XHeight != null) pdfDictionary.Add("XHeight", XHeight);
            if (StemV != null) pdfDictionary.Add("StemV", StemV);
            if (FontBBox != null) pdfDictionary.Add("FontBBox", FontBBox);
            if (FontFile2 != null) pdfDictionary.Add("FontFile2", FontFile2);

            Value = pdfDictionary;

            await base.WriteAsync(stream);
        }
    }
}
