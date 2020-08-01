using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf.HighLevel
{
    public class PdfRectangle : PdfObject
    {
        public double X { get; }
        public double Y { get; }
        public double Width { get; }
        public double Height { get; }

        public PdfRectangle(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override async Task WriteAsync(Stream stream)
        {
            var array = new PdfArray();
            array.Add(X);
            array.Add(Y);
            array.Add(Width);
            array.Add(Height);
            await array.WriteAsync(stream);
        }
    }
}
