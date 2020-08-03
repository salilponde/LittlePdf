using System.Linq;
using Xunit;

namespace LittlePdf.Test.Functional
{
    public class FontTests
    {
        [Fact]
        public void TrueType()
        {
            var font = FontReader.FontReader.ReadFromFile(@"C:\temp\fonts\calibri.ttf");
            var t = font.CharacterMapTable.Encodings.Last().Value;
            t.TryGetGlyphId('a', out var glyphId);
            var metrics = font.HorizontalMetricsTable.HMetrics[glyphId];
        }
    }
}
