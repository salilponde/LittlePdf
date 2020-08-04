using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf.HighLevel
{
    public class PdfRectangle : PdfObject
    {
        public double X1 { get; }
        public double Y1 { get; }
        public double X2 { get; }
        public double Y2 { get; }

        public PdfRectangle(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public override async Task WriteAsync(Stream stream)
        {
            var array = new PdfArray();
            array.Add(X1);
            array.Add(Y1);
            array.Add(X2);
            array.Add(Y2);
            await array.WriteAsync(stream);
        }
    }
}
