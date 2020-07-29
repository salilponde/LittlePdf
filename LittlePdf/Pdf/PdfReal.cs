using LittlePdf.Extensions;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public class PdfReal : PdfObject
    {
        public double Value { get; }

        public PdfReal(double value)
        {
            Value = value;
        }

        public static byte[] Encode(double value)
        {
            return Encoding.ASCII.GetBytes(value.ToString("G29"));
        }

        public override async Task WriteAsync(Stream stream)
        {
            var value = PdfReal.Encode(Value);
            await stream.WriteAsync(value);
        }
    }
}
