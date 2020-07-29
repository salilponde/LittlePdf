using LittlePdf.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public class PdfBoolean : PdfObject
    {
        public bool Value { get; }

        public PdfBoolean(bool value)
        {
            Value = value;
        }

        public static byte[] Encode(bool value)
        {
            if (value) return PdfSpec.True;

            return PdfSpec.False;
        }

        public override async Task WriteAsync(Stream stream)
        {
            var value = PdfBoolean.Encode(Value);
            await stream.WriteAsync(value);
        }
    }
}
