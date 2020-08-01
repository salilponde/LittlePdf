namespace LittlePdf.Pdf.HighLevel
{
    public class PdfNoStreamFilter : IPdfStreamFilter
    {
        public byte[] Encode(byte[] bytes)
        {
            return bytes;
        }
    }
}
