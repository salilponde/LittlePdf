using LittlePdf.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public class PdfIndirectObject : IPdfWriteable
    {
        private static int _nextObjectNumber = 1;
        public int ObjectNumber { get; private set; }
        public int GenerationNumber { get; set; }
        public PdfObject Value { get; private set; }

        public PdfIndirectObject(PdfObject value)
        {
            Initialize(_nextObjectNumber++, 0, value);
        }

        public PdfIndirectObject(int generationNumber, PdfObject value)
        {
            Initialize(_nextObjectNumber++, generationNumber, value);
        }

        private void Initialize(int objectNumber, int generationNumber, PdfObject value)
        {
            ObjectNumber = objectNumber;
            GenerationNumber = generationNumber;
            Value = value;
        }

        public async Task WriteAsync(Stream stream)
        {
            await new PdfInteger(ObjectNumber).WriteAsync(stream);
            await stream.WriteAsync(PdfSpec.Space);
            await new PdfInteger(GenerationNumber).WriteAsync(stream);
            await stream.WriteAsync(PdfSpec.Space);

            await stream.WriteAsync(PdfSpec.IndirectObjectBegin);
            await Value.WriteAsync(stream);
            await stream.WriteAsync(PdfSpec.IndirectObjectEnd);
        }
    }
}
