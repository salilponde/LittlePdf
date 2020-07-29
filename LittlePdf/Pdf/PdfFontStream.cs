using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public class PdfFontStream : PdfStream
    {
        public string FontFileName { get; }

        public PdfFontStream(
            Dictionary<string, PdfObject> items,
            string fontFileName,
            IPdfStreamFilter filter = null) : base(items, File.ReadAllBytes(fontFileName), filter)
        {
            FontFileName = fontFileName;
        }

        public override async Task WriteAsync(Stream stream)
        {
            await base.WriteAsync(stream);
        }

        protected override void AfterEncode(byte[] encodedBytes)
        {
            Add("Length1", encodedBytes.Length);
        }
    }
}
