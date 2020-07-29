using LittlePdf.Extensions;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public class PdfComment : PdfObject
    {
        public byte[] Value { get; }

        public PdfComment(string value)
        {
            Value = Encoding.ASCII.GetBytes(value);
        }

        public PdfComment(byte[] value)
        {
            Value = value;
        }

        public override async Task WriteAsync(Stream stream)
        {
            await stream.WriteAsync(PdfSpec.CommentBegin);
            await stream.WriteAsync(Value);
        }
    }
}
