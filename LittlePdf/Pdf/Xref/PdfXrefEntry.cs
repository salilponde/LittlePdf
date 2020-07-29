using LittlePdf.Extensions;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LittlePdf.Pdf.Xref
{
    public enum XrefEntryType
    {
        InUse,  // n
        Free    // f
    }

    public class PdfXrefEntry : IPdfWriteable
    {
        public long ByteOffset { get; }
        public int GenerationNumber { get; }
        public XrefEntryType Type { get; }

        public PdfXrefEntry(long byteOffset, int generationNumber, XrefEntryType type)
        {
            ByteOffset = byteOffset;
            GenerationNumber = generationNumber;
            Type = type;
        }

        public async Task WriteAsync(Stream stream)
        {
            await stream.WriteAsync(Encoding.ASCII.GetBytes(ByteOffset.ToString("D10")));
            await stream.WriteAsync(PdfSpec.Space);
            await stream.WriteAsync(Encoding.ASCII.GetBytes(GenerationNumber.ToString("D5")));
            await stream.WriteAsync(PdfSpec.Space);
            await stream.WriteAsync(Type == XrefEntryType.InUse ? PdfSpec.XrefInUse : PdfSpec.XrefFree);
            await stream.WriteAsync(PdfSpec.Space);
            await stream.WriteAsync(PdfSpec.NewLine);
        }
    }
}
