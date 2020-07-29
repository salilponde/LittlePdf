using LittlePdf.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public class PdfNull : PdfObject
    {
        public static byte[] Encode()
        {
            return PdfSpec.Null;
        }

        public override async Task WriteAsync(Stream stream)
        {
            var value = PdfSpec.Null;
            await stream.WriteAsync(value);
        }
    }
}
