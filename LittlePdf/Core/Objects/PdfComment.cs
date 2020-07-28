namespace LittlePdf.Core.Objects
{
    public class PdfComment : PdfObject
    {
        public byte[] Value { get; }

        public PdfComment(byte[] value)
        {
            Value = value;
        }
    }
}
