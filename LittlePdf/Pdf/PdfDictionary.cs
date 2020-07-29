using LittlePdf.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public class PdfDictionary : PdfObject
    {
        public Dictionary<string, PdfObject> Items { get; } = new Dictionary<string, PdfObject>();

        public PdfDictionary()
        {
        }

        public PdfDictionary(Dictionary<string, PdfObject> items)
        {
            Items = items;
        }

        public void Add(string key, bool value)
        {
            Items.Add(key, new PdfBoolean(value));
        }

        public void Add(string key, int value)
        {
            Items.Add(key, new PdfInteger(value));
        }

        public void Add(string key, long value)
        {
            Items.Add(key, new PdfInteger(value));
        }

        public void Add(string key, double value)
        {
            Items.Add(key, new PdfReal(value));
        }

        public void Add(string key, string value)
        {
            Items.Add(key, new PdfString(value));
        }

        public void Add(string key, PdfObject value)
        {
            Items.Add(key, value);
        }

        public override async Task WriteAsync(Stream stream)
        {
            await stream.WriteAsync(PdfSpec.DictionaryBegin);

            foreach (var pair in Items)
            {
                var name = new PdfName(pair.Key);
                await name.WriteAsync(stream);
                await stream.WriteAsync(PdfSpec.Space);

                await pair.Value.WriteAsync(stream);
                await stream.WriteAsync(PdfSpec.Space);
            }

            await stream.WriteAsync(PdfSpec.DictionaryEnd);
        }
    }
}
