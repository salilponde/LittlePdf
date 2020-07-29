using LittlePdf.Extensions;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public class PdfDate : PdfObject
    {
        public DateTime Value { get; }

        public PdfDate(DateTime value)
        {
            Value = value;
        }

        public static byte[] Encode(DateTime value)
        {
            var sb = new StringBuilder();
            sb.Append("(D:");
            sb.Append(value.ToString("yyyyMMddHHmmss"));
            sb.Append("+00'00)");
            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        public override async Task WriteAsync(Stream stream)
        {
            var value = PdfDate.Encode(Value);
            await stream.WriteAsync(value);
        }
    }
}
