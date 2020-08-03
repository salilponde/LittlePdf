using FontReader.Extensions;
using System.IO;

namespace FontReader.Tables
{
    public class HorizontalHeaderTable  // hhea
    {
        public ushort MajorVersion { get; private set; }
        public ushort MinorVersion { get; private set; }
        public short Ascender { get; private set; }
        public short Descender { get; private set; }
        public short LineGap { get; private set; }
        public ushort AdvanceWidthMax { get; private set; }
        public short MinLeftSideBearing { get; private set; }
        public short MinRightSideBearing { get; private set; }
        public short XMaxExtent { get; private set; }
        public short CaretSlopeRise { get; private set; }
        public short CaretSlopeRun { get; private set; }
        public short CaretOffset { get; private set; }
        public byte[] Reserved { get; private set; }
        public short MetricDataFormat { get; private set; }
        public ushort NumberOfHMetrics { get; private set; }

        public static HorizontalHeaderTable Read(BinaryReader reader)
        {
            var instance = new HorizontalHeaderTable();
            instance.MajorVersion = reader.ReadUInt16BigEndian();
            instance.MinorVersion = reader.ReadUInt16BigEndian();
            instance.Ascender = reader.ReadInt16BigEndian();
            instance.Descender = reader.ReadInt16BigEndian();
            instance.LineGap = reader.ReadInt16BigEndian();
            instance.AdvanceWidthMax = reader.ReadUInt16BigEndian();
            instance.MinLeftSideBearing = reader.ReadInt16BigEndian();
            instance.MinRightSideBearing = reader.ReadInt16BigEndian();
            instance.XMaxExtent = reader.ReadInt16BigEndian();
            instance.CaretSlopeRise = reader.ReadInt16BigEndian();
            instance.CaretSlopeRun = reader.ReadInt16BigEndian();
            instance.CaretOffset = reader.ReadInt16BigEndian();
            instance.Reserved = reader.ReadBytes(8);
            instance.MetricDataFormat = reader.ReadInt16BigEndian();
            instance.NumberOfHMetrics = reader.ReadUInt16BigEndian();
            return instance;
        }
    }
}
