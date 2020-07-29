using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public abstract class PdfObject : IPdfWriteable
    {
        public abstract Task WriteAsync(Stream stream);
    }
}
