using ICSharpCode.SharpZipLib.Zip.Compression;
using System.IO;

namespace LittlePdf.Utils
{
    public static class Zlib
    {
        public static byte[] Deflate(byte[] inputData)
        {
            var deflater = new Deflater(Deflater.DEFAULT_COMPRESSION, false);
            deflater.SetInput(inputData);
            deflater.Finish();

            using (var ms = new MemoryStream())
            {
                var outputBuffer = new byte[65536 * 4];
                while (deflater.IsNeedingInput == false)
                {
                    var read = deflater.Deflate(outputBuffer);
                    ms.Write(outputBuffer, 0, read);

                    if (deflater.IsFinished == true)
                        break;
                }
                deflater.Reset();

                return ms.ToArray();
            }
        }

        public static byte[] Inflate(byte[] inputData)
        {
            var inflater = new Inflater();
            inflater.SetInput(inputData);

            using (var ms = new MemoryStream())
            {
                var outputBuffer = new byte[65536 * 4];
                while (inflater.IsNeedingInput == false)
                {
                    var read = inflater.Inflate(outputBuffer);
                    ms.Write(outputBuffer, 0, read);

                    if (inflater.IsFinished == true)
                        break;
                }
                inflater.Reset();

                return ms.ToArray();
            }
        }
    }
}
