namespace LittlePdf.Core.Objects
{
    public class PdfBoolean : PdfObject
    {
        public bool Value { get; }

        public PdfBoolean(bool value)
        {
            Value = value;
        }
    }
}
