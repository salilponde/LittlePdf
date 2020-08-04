using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf.HighLevel
{
    public class PdfFontStream : PdfIndirectObject
    {
        public PdfStream Stream { get; set; }
        public PdfStreamFilter Filter { get; set; }

        public override async Task WriteAsync(Stream stream)
        {
            IPdfStreamFilter streamFilter;

            switch (Filter)
            {
                case PdfStreamFilter.FlateDecode:
                    streamFilter = new PdfFlateDecodeStreamFilter();
                    break;
                default:
                    streamFilter = new PdfNoStreamFilter();
                    break;
            }

            var encodedStream = streamFilter.Encode(Stream.Value);
            var pdfDictionary = new PdfDictionary();
            pdfDictionary.Add("Length", encodedStream.Length);
            pdfDictionary.Add("Length1", Stream.Value.Length);
            if (Filter != PdfStreamFilter.NoFilter) pdfDictionary.Add("Filter", new PdfName(Filter.ToString()));
            Value = new PdfStream(encodedStream);

            await WriteHeadAsync(stream);
            await pdfDictionary.WriteAsync(stream);
            await Value.WriteAsync(stream);
            await WriteTailAsync(stream);
        }
    }
}
