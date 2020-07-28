using LittlePdf.Core.Objects;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Core.Writers
{
    public class PdfIntegerWriter : PdfObjectWriter
    {
        private readonly Stream _stream;

        public PdfIntegerWriter(Stream stream)
        {
            _stream = stream;
        }

        public async Task WriteAsync(PdfInteger @object)
        {
            var bytes = Encoding.ASCII.GetBytes(@object.Value.ToString());
            await _stream.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
