using LittlePdf.Utils;

namespace LittlePdf.Pdf
{
    public class PdfFlatDecodeStreamFilter : IPdfStreamFilter
    {
        public byte[] Encode(PdfStream pdfStream)
        {
            var encodedValue = Zlib.Deflate(pdfStream.OriginalValue);

            pdfStream.Items["Filter"] = new PdfName("FlateDecode");
            pdfStream.Items["Length"] = new PdfInteger(encodedValue.Length);
            return encodedValue;
        }
    }
}
