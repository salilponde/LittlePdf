using LittlePdf.Extensions;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public class PdfString : PdfObject
    {
        public string Value { get; }

        public PdfString(string value)
        {
            Value = value;
        }

        public static byte[] Encode(string name)
        {
            var sb = new StringBuilder();
            sb.Append('(');
            foreach (var c in name)
            {
                if (PdfSpec.EscapeSequences.ContainsKey(c))
                {
                    sb.Append(PdfSpec.EscapeSequences[c]);
                }
                else
                {
                    sb.Append(c);
                }
            }
            sb.Append(')');
            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        public static byte[] EncodeHex(string name)
        {
            var sb = new StringBuilder();
            sb.Append('<');
            foreach (var c in name)
            {
                sb.Append(((int)c).ToString("X2"));
            }
            sb.Append('>');
            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        public override async Task WriteAsync(Stream stream)
        {
            var value = PdfString.Encode(Value);
            await stream.WriteAsync(value);
        }

        public async Task WriteHexAsync(Stream stream)
        {
            var value = PdfString.EncodeHex(Value);
            await stream.WriteAsync(value);
        }
    }
}
