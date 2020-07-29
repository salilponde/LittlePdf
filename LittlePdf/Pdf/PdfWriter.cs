using LittlePdf.Extensions;
using LittlePdf.Pdf.Xref;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public class PdfWriter
    {
        private readonly Stream _stream;
        private readonly List<PdfXrefEntry> _xrefEntries = new List<PdfXrefEntry>();

        public PdfWriter(Stream stream)
        {
            _stream = stream;
            _xrefEntries.Add(new PdfXrefEntry(0, 65535, XrefEntryType.Free));
            Task.Run(() => WriteHeaderAsync()).Wait();
        }

        public async Task WriteAsync(IPdfWriteable writeableObject)
        {
            if (writeableObject is PdfIndirectObject)
            {
                var offset = _stream.Position;
                _xrefEntries.Add(new PdfXrefEntry(offset, 0, XrefEntryType.InUse));
            }

            await writeableObject.WriteAsync(_stream);
        }

        public async Task CloseAsync(PdfIndirectObject root, PdfIndirectObject info)
        {
            var xrefTableOffset = _stream.Position;
            await WriteXrefTableAsync();
            await WriteTailerAsync(xrefTableOffset, root, info);
            _stream.Close();
        }

        private async Task WriteHeaderAsync()
        {
            await _stream.WriteAsync(Encoding.ASCII.GetBytes("%PDF-1.7\n"));
            await _stream.WriteAsync(new byte[] { (byte)'%', 128, 129, 130, 131, (byte)'\n' });
        }

        private async Task WriteXrefTableAsync()
        {
            await new PdfXrefTable(_xrefEntries).WriteAsync(_stream);
        }

        private async Task WriteTailerAsync(long xrefTableOffset, PdfIndirectObject root, PdfIndirectObject info)
        {
            var dictionary = new PdfDictionary(new Dictionary<string, PdfObject>());
            dictionary.Add("Size", _xrefEntries.Count);
            dictionary.Add("Root", new PdfIndirectObjectReference(root));
            if (info != null) dictionary.Add("Info", new PdfIndirectObjectReference(info));

            await _stream.WriteAsync(Encoding.ASCII.GetBytes("trailer\n"));
            await dictionary.WriteAsync(_stream);
            await _stream.WriteAsync(PdfSpec.NewLine);
            await _stream.WriteAsync(Encoding.ASCII.GetBytes("startxref\n"));
            await new PdfInteger(xrefTableOffset).WriteAsync(_stream);
            await _stream.WriteAsync(PdfSpec.NewLine);
            await _stream.WriteAsync(Encoding.ASCII.GetBytes("%%EOF\n"));
        }
    }
}
