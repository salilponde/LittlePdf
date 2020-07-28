namespace LittlePdf.Core.Objects
{
    public class PdfReal : PdfObject
    {
        public double Value { get; }

        public PdfReal(double value)
        {
            Value = value;
        }
    }
}
