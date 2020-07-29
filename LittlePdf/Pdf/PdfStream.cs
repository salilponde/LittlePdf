using LittlePdf.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public class PdfStream : PdfDictionary
    {
        public byte[] OriginalValue { get; }
        private IPdfStreamFilter Filter { get; }

        public PdfStream(Dictionary<string, PdfObject> items, byte[] value, IPdfStreamFilter filter = null) : base(items)
        {
            OriginalValue = value;
            Filter = filter ?? new PdfNoStreamFilter();
        }

        public PdfStream(byte[] value, IPdfStreamFilter filter = null) : base()
        {
            OriginalValue = value;
            Filter = filter ?? new PdfNoStreamFilter();
        }

        public override async Task WriteAsync(Stream stream)
        {
            var encodedBytes = Filter.Encode(this);
            AfterEncode(encodedBytes);
            await base.WriteAsync(stream);
            await WriteStreamBlockAsync(stream, encodedBytes);
        }

        protected virtual void AfterEncode(byte[] encodedBytes)
        {
        }

        internal static async Task WriteStreamBlockAsync(Stream stream, byte[] bytes)
        {
            await stream.WriteAsync(PdfSpec.StreamBegin);
            await stream.WriteAsync(bytes);
            await stream.WriteAsync(PdfSpec.NewLine);
            await stream.WriteAsync(PdfSpec.StreamEnd);
        }
    }
}
