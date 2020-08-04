using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf.HighLevel
{
    public class PdfCharWidths : PdfIndirectObject
    {
        public PdfArray Widths { get; set; }

        public override async Task WriteAsync(Stream stream)
        {
            Value = Widths;
            await base.WriteAsync(stream);
        }
    }
}
