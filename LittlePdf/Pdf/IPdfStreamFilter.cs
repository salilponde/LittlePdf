namespace LittlePdf.Pdf
{
    public interface IPdfStreamFilter
    {
        byte[] Encode(PdfStream pdfStream);
    }
}
