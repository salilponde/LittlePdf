using LittlePdf.Core.CrossReferencing;
using LittlePdf.Core.Objects;
using LittlePdf.Extensions;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Core.Writers
{
    public class PdfCrossReferenceTableWriter
    {
        private readonly Stream _stream;
        private readonly PdfIntegerWriter _pdfIntegerWriter;

        public PdfCrossReferenceTableWriter(Stream stream)
        {
            _stream = stream;
            _pdfIntegerWriter = new PdfIntegerWriter(_stream);
        }

        public async Task WriteAsync(PdfCrossReferenceTable crossReferencetable)
        {
            foreach (var section in crossReferencetable.Sections)
            {
                await _stream.WriteAsync(Encoding.ASCII.GetBytes("xref\n"), 0, 5);
                foreach (var subSection in section.SubSections)
                {
                    await _pdfIntegerWriter.WriteAsync(new PdfInteger(subSection.StartObjectNumber));
                    await _stream.WriteByteAsync((byte)' ');
                    await _pdfIntegerWriter.WriteAsync(new PdfInteger(subSection.TotalEntries));
                    await _stream.WriteByteAsync((byte)'\n');

                    foreach (var entry in subSection.Entries)
                    {
                        await _stream.WriteAsync(Encoding.ASCII.GetBytes(entry.ByteOffset.ToString("D10")), 0, 10);
                        await _stream.WriteByteAsync((byte)' ');
                        await _stream.WriteAsync(Encoding.ASCII.GetBytes(entry.GenerationNumber.ToString("D5")), 0, 5);
                        await _stream.WriteByteAsync((byte)' ');
                        var type = entry.Type == CrossReferenceEntryType.InUse ? 'n' : 'f';
                        await _stream.WriteByteAsync((byte)type);
                        await _stream.WriteByteAsync((byte)'\r');
                        await _stream.WriteByteAsync((byte)'\n');
                    }
                }
            }
        }
    }
}
