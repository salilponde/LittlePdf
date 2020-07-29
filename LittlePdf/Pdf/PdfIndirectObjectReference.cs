using LittlePdf.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public class PdfIndirectObjectReference : PdfObject
    {
        public PdfIndirectObject Value { get; }

        public PdfIndirectObjectReference(PdfIndirectObject value)
        {
            Value = value;
        }

        public override async Task WriteAsync(Stream stream)
        {
            await new PdfInteger(Value.ObjectNumber).WriteAsync(stream);
            await stream.WriteAsync(PdfSpec.Space);
            await new PdfInteger(Value.GenerationNumber).WriteAsync(stream);
            await stream.WriteAsync(PdfSpec.Space);
            await stream.WriteAsync(new byte[] { (byte)'R' });
        }
    }
}
