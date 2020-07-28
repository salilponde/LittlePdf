using LittlePdf.Core.Objects;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Core.Writers
{
    public class PdfBooleanWriter : PdfObjectWriter
    {
        private readonly Stream _stream;
        private readonly byte[] _true = Encoding.ASCII.GetBytes("true");
        private readonly byte[] _false = Encoding.ASCII.GetBytes("false");

        public PdfBooleanWriter(Stream stream)
        {
            _stream = stream;
        }

        public async Task WriteAsync(PdfBoolean @object)
        {
            var bytes = @object.Value ? _true : _false;
            await _stream.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
