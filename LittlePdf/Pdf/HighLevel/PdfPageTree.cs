using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LittlePdf.Pdf.HighLevel
{
    public class PdfPageTree : PdfIndirectObject
    {
        private readonly PdfName _type = new PdfName("Pages");
        public PdfRectangle MediaBox { get; set; }
        public List<PdfIndirectObjectReference> Kids { get; set; }
        public PdfResources Resources { get; set; }

        public override async Task WriteAsync(Stream stream)
        {
            var pdfDictionary = new PdfDictionary();
            pdfDictionary.Items.Clear();
            pdfDictionary.Add(PdfSpec.Names.Type, _type);
            if (MediaBox != null) pdfDictionary.Add(PdfSpec.Names.MediaBox, MediaBox);
            if (Kids != null)
            {
                pdfDictionary.Add(PdfSpec.Names.Kids, new PdfArray(Kids.Select(x => (PdfObject)x).ToList()));
                pdfDictionary.Add(PdfSpec.Names.Count, new PdfInteger(Kids.Count));
            }
            if (Resources != null)
            {
                pdfDictionary.Add(PdfSpec.Names.Resources, Resources);
            }

            Value = pdfDictionary;

            await base.WriteAsync(stream);
        }
    }
}
