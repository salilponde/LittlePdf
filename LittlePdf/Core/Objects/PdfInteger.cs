namespace LittlePdf.Core.Objects
{
    public class PdfInteger : PdfObject
    {
        public long Value { get; }

        public PdfInteger(long value)
        {
            Value = value;
        }
    }
}
