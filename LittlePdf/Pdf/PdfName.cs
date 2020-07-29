using LittlePdf.Extensions;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public class PdfName : PdfObject
    {
        public string Value { get; }

        public PdfName(string value)
        {
            Value = value;
        }

        public static byte[] Encode(string name)
        {
            var sb = new StringBuilder();
            sb.Append("/");
            foreach (var c in name)
            {
                if (PdfSpec.IsDelimiterCharacter(c) || c == '#' || (c < '!' && c > '~'))
                {
                    sb.Append("#");
                    sb.Append(((int)c).ToString("X2"));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        public override async Task WriteAsync(Stream stream)
        {
            var value = PdfName.Encode(Value);
            await stream.WriteAsync(value);
        }
    }
}
