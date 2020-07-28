using System;
using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Extensions
{
    public static class StreamExtensions
    {
        public static Task WriteByteAsync(this Stream stream, byte value)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            return stream.WriteAsync(new[] { value }, 0, 1);
        }
    }
}
