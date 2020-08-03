using FontReader.Extensions;
using System.IO;

namespace FontReader.Tables
{
    public class HeadTable
    {
        public ushort MajorVersion { get; private set; }
        public ushort MinorVersion { get; private set; }
        public uint FontRevision { get; private set; }          // Fixed Point Number
        public uint CheckSumAdjustment { get; private set; }
        public uint MagicNumber { get; private set; }
        public ushort Flags { get; private set; }
        public ushort UnitsPerEm { get; private set; }
        public long Created { get; private set; }
        public long Modified { get; private set; }
        public short XMin { get; private set; }
        public short YMin { get; private set; }
        public short XMax { get; private set; }
        public short YMax { get; private set; }
        public ushort MacStyle { get; private set; }
        public ushort LowestRecPPEM { get; private set; }
        public short FontDirectionHint { get; private set; }
        public short IndexToLocFormat { get; private set; }
        public short GlyphDataFormat { get; private set; }

        public static HeadTable Read(BinaryReader reader)
        {
            var instance = new HeadTable();
            instance.MajorVersion = reader.ReadUInt16BigEndian();
            instance.MajorVersion = reader.ReadUInt16BigEndian();
            instance.FontRevision = reader.ReadUInt32();
            instance.CheckSumAdjustment = reader.ReadUInt32BigEndian();
            instance.MagicNumber = reader.ReadUInt32BigEndian();
            instance.Flags = reader.ReadUInt16BigEndian();
            instance.UnitsPerEm = reader.ReadUInt16BigEndian();
            instance.Created = reader.ReadInt64BigEndian();
            instance.Modified = reader.ReadInt64BigEndian();
            instance.XMin = reader.ReadInt16BigEndian();
            instance.YMin = reader.ReadInt16BigEndian();
            instance.XMax = reader.ReadInt16BigEndian();
            instance.YMax = reader.ReadInt16BigEndian();
            instance.MacStyle = reader.ReadUInt16BigEndian();
            instance.LowestRecPPEM = reader.ReadUInt16BigEndian();
            instance.FontDirectionHint = reader.ReadInt16BigEndian();
            instance.IndexToLocFormat = reader.ReadInt16BigEndian();
            instance.GlyphDataFormat = reader.ReadInt16BigEndian();
            return instance;
        }
    }
}
