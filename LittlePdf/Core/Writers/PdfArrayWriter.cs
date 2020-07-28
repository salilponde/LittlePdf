using LittlePdf.Core.Objects;
using LittlePdf.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Core.Writers
{
    public class PdfArrayWriter : PdfObjectWriter
    {
        private readonly Stream _stream;
        private readonly PdfWriter _pdfWriter;

        public PdfArrayWriter(Stream stream, PdfWriter pdfWriter)
        {
            _stream = stream;
            _pdfWriter = pdfWriter;
        }

        public async Task WriteAsync(PdfArray array)
        {
            await _stream.WriteByteAsync((byte)'[');
            foreach (var item in array.Items)
            {
                await _pdfWriter.WriteAsync(item);
                await _stream.WriteByteAsync((byte)' ');
            }
            await _stream.WriteByteAsync((byte)']');
        }
    }
}
