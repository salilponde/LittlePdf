using LittlePdf.Utils;

namespace LittlePdf.Pdf.HighLevel
{
    public class PdfFlateDecodeStreamFilter : IPdfStreamFilter
    {
        public byte[] Encode(byte[] bytes)
        {
            return Zlib.Deflate(bytes);
        }
    }
}
