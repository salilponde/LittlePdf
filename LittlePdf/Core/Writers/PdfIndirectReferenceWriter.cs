using LittlePdf.Core.Objects;
using LittlePdf.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Core.Writers
{
    public class PdfIndirectObjectReferenceWriter
    {
        private readonly Stream _stream;
        private readonly PdfIntegerWriter _pdfIntegerWriter;

        public PdfIndirectObjectReferenceWriter(Stream stream)
        {
            _stream = stream;
            _pdfIntegerWriter = new PdfIntegerWriter(_stream);
        }

        public async Task WriteAsync(PdfIndirectObjectReference @object)
        {
            await _pdfIntegerWriter.WriteAsync(new PdfInteger(@object.ObjectNumber));
            await _stream.WriteByteAsync((byte)' ');
            await _pdfIntegerWriter.WriteAsync(new PdfInteger(@object.GenerationNumber));
            await _stream.WriteByteAsync((byte)' ');
            await _stream.WriteByteAsync((byte)'R');
        }
    }
}
