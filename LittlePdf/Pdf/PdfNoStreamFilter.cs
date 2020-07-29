namespace LittlePdf.Pdf
{
    public class PdfNoStreamFilter : IPdfStreamFilter
    {
        public byte[] Encode(PdfStream pdfStream)
        {
            pdfStream.Items["Length"] = new PdfInteger(pdfStream.OriginalValue.Length);
            return pdfStream.OriginalValue;
        }
    }
}
