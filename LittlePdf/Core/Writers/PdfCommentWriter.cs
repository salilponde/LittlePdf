using LittlePdf.Core.Objects;
using LittlePdf.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Core.Writers
{
    public class PdfCommentWriter : PdfObjectWriter
    {
        private readonly Stream _stream;

        public PdfCommentWriter(Stream stream)
        {
            _stream = stream;
        }

        public async Task WriteAsync(PdfComment @object)
        {
            await _stream.WriteByteAsync((byte)'%');
            await _stream.WriteAsync(@object.Value, 0, @object.Value.Length);
            await _stream.WriteByteAsync((byte)'\n');
        }
    }
}
