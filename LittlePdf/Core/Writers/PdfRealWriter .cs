using LittlePdf.Core.Objects;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Core.Writers
{
    public class PdfRealWriter : PdfObjectWriter
    {
        private readonly Stream _stream;

        public PdfRealWriter(Stream stream)
        {
            _stream = stream;
        }

        public async Task WriteAsync(PdfReal @object)
        {
            var bytes = Encoding.ASCII.GetBytes(@object.Value.ToString("G29"));
            await _stream.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
