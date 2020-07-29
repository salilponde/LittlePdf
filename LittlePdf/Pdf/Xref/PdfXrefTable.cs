using LittlePdf.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf.Xref
{
    public class PdfXrefTable : IPdfWriteable
    {
        public List<PdfXrefEntry> Entries { get; }

        public PdfXrefTable(List<PdfXrefEntry> entries)
        {
            Entries = entries;
        }

        public async Task WriteAsync(Stream stream)
        {
            await stream.WriteAsync(PdfSpec.XrefBegin);
            await new PdfInteger(0).WriteAsync(stream);
            await stream.WriteAsync(PdfSpec.Space);
            await new PdfInteger(Entries.Count).WriteAsync(stream);
            await stream.WriteAsync(PdfSpec.NewLine);
            foreach (var entry in Entries)
            {
                await entry.WriteAsync(stream);
            }
        }
    }
}
