using LittlePdf.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public class PdfArray : PdfObject
    {
        public List<PdfObject> Items { get; } = new List<PdfObject>();

        public PdfArray()
        {
        }

        public PdfArray(List<PdfObject> items)
        {
            Items = items;
        }

        public void Add(bool value)
        {
            Items.Add(new PdfBoolean(value));
        }

        public void Add(int value)
        {
            Items.Add(new PdfInteger(value));
        }

        public void Add(long value)
        {
            Items.Add(new PdfInteger(value));
        }

        public void Add(double value)
        {
            Items.Add(new PdfReal(value));
        }

        public void Add(string value)
        {
            Items.Add(new PdfString(value));
        }

        public void Add(PdfObject value)
        {
            Items.Add(value);
        }

        public override async Task WriteAsync(Stream stream)
        {
            await stream.WriteAsync(PdfSpec.ArrayBegin);

            for (var i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                await item.WriteAsync(stream);
                if (i != Items.Count - 1) await stream.WriteAsync(PdfSpec.Space);
            }

            await stream.WriteAsync(PdfSpec.ArrayEnd);
        }
    }
}
