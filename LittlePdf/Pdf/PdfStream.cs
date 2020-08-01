using LittlePdf.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public class PdfStream : PdfObject
    {
        public byte[] Value { get; }

        public PdfStream(byte[] value)
        {
            Value = value;
        }

        public override async Task WriteAsync(Stream stream)
        {
            await stream.WriteAsync(PdfSpec.StreamBegin);
            await stream.WriteAsync(Value);
            await stream.WriteAsync(PdfSpec.NewLine);
            await stream.WriteAsync(PdfSpec.StreamEnd);
        }
    }
}
