using LittlePdf.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace LittlePdf.Test.Functional
{
    public class StreamTests
    {
        [Fact]
        public void ExtractStream()
        {
            var fileName = @"C:\Users\sal\Desktop\Hey.pdf";
            var offset = 540;
            var compressedSize = 1096;

            var file = File.OpenRead(fileName);
            file.Seek(offset, SeekOrigin.Begin);

            int b;
            while ((b = file.ReadByte()) != 'm');
            while ((b = file.ReadByte()) != '\n');

            var streamOffset = file.Position;
            var compressedBytes = new byte[compressedSize];
            file.Read(compressedBytes);
            var bytes = Zlib.Inflate(compressedBytes);

            var s = Encoding.ASCII.GetString(bytes);
        }
    }
}
