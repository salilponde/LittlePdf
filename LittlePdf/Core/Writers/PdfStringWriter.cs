using LittlePdf.Core.Objects;
using LittlePdf.Extensions;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Core.Writers
{
    public class PdfStringWriter : PdfObjectWriter
    {
        private readonly Stream _stream;

        public PdfStringWriter(Stream stream)
        {
            _stream = stream;
        }

        public async Task WriteAsync(PdfString @object)
        {
            var bytes = Encoding.ASCII.GetBytes(@object.Value);
            await _stream.WriteByteAsync((byte)'(');
            foreach (var @byte in bytes)
            {
                switch (@byte)
                {
                    case (byte)'\n':
                        await _stream.WriteByteAsync((byte)'\\');
                        await _stream.WriteByteAsync((byte)'n');
                        break;
                    case (byte)'\r':
                        await _stream.WriteByteAsync((byte)'\\');
                        await _stream.WriteByteAsync((byte)'r');
                        break;
                    case (byte)'\t':
                        await _stream.WriteByteAsync((byte)'\\');
                        await _stream.WriteByteAsync((byte)'t');
                        break;
                    case (byte)'\b':
                        await _stream.WriteByteAsync((byte)'\\');
                        await _stream.WriteByteAsync((byte)'b');
                        break;
                    case (byte)'\f':
                        await _stream.WriteByteAsync((byte)'\\');
                        await _stream.WriteByteAsync((byte)'f');
                        break;
                    case (byte)'(':
                        await _stream.WriteByteAsync((byte)'\\');
                        await _stream.WriteByteAsync((byte)'(');
                        break;
                    case (byte)')':
                        await _stream.WriteByteAsync((byte)'\\');
                        await _stream.WriteByteAsync((byte)')');
                        break;
                    case (byte)'\\':
                        await _stream.WriteByteAsync((byte)'\\');
                        await _stream.WriteByteAsync((byte)'\\');
                        break;
                    default:
                        await _stream.WriteByteAsync(@byte);
                        break;
                }
            }
            await _stream.WriteByteAsync((byte)')');
        }
    }
}
