using LittlePdf.Core.Objects;
using LittlePdf.Extensions;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Core.Writers
{
    public class PdfNameWriter : PdfObjectWriter
    {
        private readonly Stream _stream;

        public PdfNameWriter(Stream stream)
        {
            _stream = stream;
        }

        public async Task WriteAsync(PdfName @object)
        {
            var bytes = Encoding.ASCII.GetBytes(@object.Value);
            await _stream.WriteByteAsync((byte)'/');
            foreach (var @byte in bytes)
            {
                if (@byte == '#')
                {
                    await _stream.WriteByteAsync((byte)'#');
                    await _stream.WriteByteAsync((byte)'2');
                    await _stream.WriteByteAsync((byte)'3');
                    break;
                }
                else if (@byte >= '!' && @byte <= '~')
                {
                    await _stream.WriteByteAsync(@byte);
                }
                else
                {
                    var hexCodeBytes = Encoding.ASCII.GetBytes(((int)@byte).ToString("X2"));
                    await _stream.WriteByteAsync(hexCodeBytes[0]);
                    await _stream.WriteByteAsync(hexCodeBytes[1]);
                }
            }
        }
    }
}
