using FontReader.Extensions;
using System.IO;

namespace FontReader.Tables
{
    public class HorizontalMetricsTable
    {
        public LongHorizontalMetric[] HMetrics { get; private set; }
        public short[] LeftSideBearings { get; private set; }

        public static HorizontalMetricsTable Read(BinaryReader reader, int numberOfHMetrics, int numGlyphs)
        {
            var instance = new HorizontalMetricsTable();
            instance.HMetrics = new LongHorizontalMetric[numberOfHMetrics];
            for (int i = 0; i < numberOfHMetrics; i++)
            {
                instance.HMetrics[i] = LongHorizontalMetric.Read(reader);
            }

            var lsbCount = numGlyphs - numberOfHMetrics;
            instance.LeftSideBearings = new short[lsbCount];
            for (int i = 0; i < lsbCount; i++)
            {
                instance.LeftSideBearings[i] = reader.ReadInt16BigEndian();
            }

            return instance;
        }
    }

    public class LongHorizontalMetric
    {
        public ushort AdvanceWidth { get; private set; }
        public short Lsb { get; private set; }

        public static LongHorizontalMetric Read(BinaryReader reader)
        {
            var instance = new LongHorizontalMetric();
            instance.AdvanceWidth = reader.ReadUInt16BigEndian();
            instance.Lsb = reader.ReadInt16BigEndian();
            return instance;
        }
    }
}
