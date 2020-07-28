using LittlePdf.Core.Objects;
using LittlePdf.Extensions;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Core.Writers
{
    public class PdfIndirectObjectWriter : PdfObjectWriter
    {
        private readonly Stream _stream;
        private readonly PdfIntegerWriter _pdfIntegerWriter;
        private readonly PdfWriter _pdfWriter;

        public PdfIndirectObjectWriter(Stream stream, PdfWriter pdfWriter)
        {
            _stream = stream;
            _pdfIntegerWriter = new PdfIntegerWriter(_stream);
            _pdfWriter = pdfWriter;
        }

        public async Task WriteAsync(PdfIndirectObject @object)
        {
            await _pdfIntegerWriter.WriteAsync(new PdfInteger(@object.ObjectNumber));
            await _stream.WriteByteAsync((byte)' ');
            await _pdfIntegerWriter.WriteAsync(new PdfInteger(@object.GenerationNumber));
            await _stream.WriteAsync(Encoding.ASCII.GetBytes(" obj\r\n"), 0, 6);
            await _pdfWriter.WriteAsync(@object.Value);
            await _stream.WriteAsync(Encoding.ASCII.GetBytes(" endobj\r\n"), 0, 9);
        }
    }
}
