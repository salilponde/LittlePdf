using LittlePdf.Core.Objects;
using LittlePdf.Extensions;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Core.Writers
{
    public class PdfDictionaryWriter : PdfObjectWriter
    {
        private readonly Stream _stream;
        private readonly PdfNameWriter _pdfNameWriter;
        private readonly PdfWriter _pdfWriter;

        public PdfDictionaryWriter(Stream stream, PdfWriter pdfWriter)
        {
            _stream = stream;
            _pdfNameWriter = new PdfNameWriter(_stream);
            _pdfWriter = pdfWriter;
        }

        public async Task WriteAsync(PdfDictionary dictionary)
        {
            await _stream.WriteAsync(Encoding.ASCII.GetBytes("<<"), 0, 2);
            foreach (var pair in dictionary.Pairs)
            {
                await _pdfNameWriter.WriteAsync(pair.Key);
                await _stream.WriteByteAsync((byte)' ');
                await _pdfWriter.WriteAsync(pair.Value);
            }
            await _stream.WriteAsync(Encoding.ASCII.GetBytes(">>"), 0, 2);
        }
    }
}
