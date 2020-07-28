using LittlePdf.Core.Objects;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Core.Writers
{
    public class PdfNullWriter : PdfObjectWriter
    {
        private readonly Stream _stream;
        private readonly byte[] _null = Encoding.ASCII.GetBytes("null");

        public PdfNullWriter(Stream stream)
        {
            _stream = stream;
        }

        public async Task WriteAsync(PdfNull _)
        {
            await _stream.WriteAsync(_null, 0, _null.Length);
        }
    }
}
