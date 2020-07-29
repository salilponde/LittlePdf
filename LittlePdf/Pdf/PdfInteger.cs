using LittlePdf.Extensions;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public class PdfInteger : PdfObject
    {
        public long Value { get; }

        public PdfInteger(long value)
        {
            Value = value;
        }

        public PdfInteger(int value)
        {
            Value = value;
        }

        public static byte[] Encode(long value)
        {
            return Encoding.ASCII.GetBytes(value.ToString());
        }

        public static byte[] Encode(int value)
        {
            return Encoding.ASCII.GetBytes(value.ToString());
        }

        public override async Task WriteAsync(Stream stream)
        {
            var value = PdfInteger.Encode(Value);
            await stream.WriteAsync(value);
        }
    }
}
