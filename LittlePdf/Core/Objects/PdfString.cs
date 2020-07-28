namespace LittlePdf.Core.Objects
{
    public class PdfString : PdfObject
    {
        public string Value { get; }

        public PdfString(string value)
        {
            Value = value;
        }
    }
}
