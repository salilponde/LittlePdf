using LittlePdf.Core.Objects;
using LittlePdf.Core.Writers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LittlePdf.Test.Core.Writers
{
    public class PdfBooleanWriterTests
    {
        private readonly Stream _stream;
        private readonly PdfBooleanWriter _writer;

        public PdfBooleanWriterTests()
        {
            _stream = new MemoryStream();
            _writer = new PdfBooleanWriter(_stream);
        }

        private List<byte> ReadBytes()
        {
            var v = new List<byte>();
            int b;
            while ((b = _stream.ReadByte()) != -1)
            {
                v.Add((byte)b);
            }
            return v;
        }

        [Fact]
        public async Task True()
        {
            await _writer.WriteAsync(new PdfBoolean(true));

            _stream.Seek(0, SeekOrigin.Begin);
            var v = ReadBytes();
            Assert.True(Enumerable.SequenceEqual(v, Encoding.ASCII.GetBytes("true")));
        }


        [Fact]
        public async Task False()
        {
            await _writer.WriteAsync(new PdfBoolean(false));

            _stream.Seek(0, SeekOrigin.Begin);
            var v = ReadBytes();
            Assert.True(Enumerable.SequenceEqual(v, Encoding.ASCII.GetBytes("false")));
        }
    }
}
