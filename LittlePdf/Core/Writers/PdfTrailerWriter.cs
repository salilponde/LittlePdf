using LittlePdf.Core.Objects;
using LittlePdf.Extensions;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Core.Writers
{
    public class PdfTrailerWriter
    {
        private readonly Stream _stream;
        private readonly PdfIntegerWriter _pdfIntegerWriter;
        private readonly PdfNameWriter _pdfNameWriter;
        private readonly PdfIndirectObjectReferenceWriter _pdfIndirectObjectReferenceWriter;

        public PdfTrailerWriter(Stream stream)
        {
            _stream = stream;
            _pdfIntegerWriter = new PdfIntegerWriter(_stream);
            _pdfNameWriter = new PdfNameWriter(_stream);
            _pdfIndirectObjectReferenceWriter = new PdfIndirectObjectReferenceWriter(_stream);
        }

        public async Task WriteAsync(PdfTrailer trailer)
        {
            await _stream.WriteAsync(Encoding.UTF8.GetBytes("trailer\r\n"), 0, 9);
            await _stream.WriteAsync(Encoding.UTF8.GetBytes("<<"), 0, 2);

            await _pdfNameWriter.WriteAsync(new PdfName("Size"));
            await _stream.WriteByteAsync((byte)' ');
            await _pdfIntegerWriter.WriteAsync(new PdfInteger(trailer.Size));

            await _pdfNameWriter.WriteAsync(new PdfName("Root"));
            await _stream.WriteByteAsync((byte)' ');
            await _pdfIndirectObjectReferenceWriter.WriteAsync(trailer.Root);

            await _pdfNameWriter.WriteAsync(new PdfName("Info"));
            await _stream.WriteByteAsync((byte)' ');
            await _pdfIndirectObjectReferenceWriter.WriteAsync(trailer.Info);

            await _stream.WriteAsync(Encoding.UTF8.GetBytes(">>\r\n"), 0, 4);

            await _stream.WriteAsync(Encoding.UTF8.GetBytes("startxref\r\n"), 0, 11);
            await _pdfIntegerWriter.WriteAsync(new PdfInteger(trailer.StartXrefOffset));
            await _stream.WriteAsync(Encoding.UTF8.GetBytes("\r\n"), 0, 2);

            await _stream.WriteAsync(Encoding.UTF8.GetBytes("%%EOF\r\n"), 0, 7);
        }
    }
}
