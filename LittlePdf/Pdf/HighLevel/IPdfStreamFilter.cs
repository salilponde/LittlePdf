namespace LittlePdf.Pdf.HighLevel
{
    public interface IPdfStreamFilter
    {
        byte[] Encode(byte[] bytes);
    }
}
