using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Pdf
{
    public interface IPdfWriteable
    {
        Task WriteAsync(Stream stream);
    }
}
