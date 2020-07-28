namespace LittlePdf.Core.Objects
{
    public class PdfName : PdfObject
    {
        public string Value { get; }

        public PdfName(string value)
        {
            Value = value;
        }
    }
}
