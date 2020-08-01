using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf.HighLevel
{
    public class PdfFontResources : PdfObject
    {
        private readonly PdfDictionary _dictionary = new PdfDictionary();

        public void Clear()
        {
            _dictionary.Items.Clear();
        }

        public void Add(string name, PdfIndirectObjectReference reference)
        {
            _dictionary.Add(name, reference);
        }

        public override async Task WriteAsync(Stream stream)
        {
            await _dictionary.WriteAsync(stream);
        }
    }
}
