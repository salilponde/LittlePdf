using FontReader.Extensions;
using System.IO;

namespace FontReader.Tables
{
    public class Os2Table   // OS/2
    {
        public ushort Version { get; private set; }
        public short XAvgCharWidth { get; private set; }
        public ushort UsWeightClass { get; private set; }
        public ushort UsWidthClass { get; private set; }
        public ushort FsType { get; private set; }
        public short YSubscriptXSize { get; private set; }
        public short YSubscriptYSize { get; private set; }
        public short YSubscriptXOffset { get; private set; }
        public short YSubscriptYOffset { get; private set; }
        public short YSuperscriptXSize { get; private set; }
        public short YSuperscriptYSize { get; private set; }
        public short YSuperscriptXOffset { get; private set; }
        public short YSuperscriptYOffset { get; private set; }
        public short YStrikeoutSize { get; private set; }
        public short YStrikeoutPosition { get; private set; }
        public short SFamilyClass { get; private set; }
        public byte[] Panose { get; private set; }          // Size 10
        public uint UlUnicodeRange1 { get; private set; }
        public uint UlUnicodeRange2 { get; private set; }
        public uint UlUnicodeRange3 { get; private set; }
        public uint UlUnicodeRange4 { get; private set; }
        public byte[] AchVendID { get; private set; }       // Size 4
        public ushort FsSelection { get; private set; }
        public ushort UsFirstCharIndex { get; private set; }
        public ushort UsLastCharIndex { get; private set; }
        public short STypoAscender { get; private set; }
        public short STypoDescender { get; private set; }
        public short STypoLineGap { get; private set; }
        public ushort UsWinAscent { get; private set; }
        public ushort UsWinDescent { get; private set; }
        public uint UlCodePageRange1 { get; private set; }
        public uint UlCodePageRange2 { get; private set; }
        public short SxHeight { get; private set; }
        public short SCapHeight { get; private set; }
        public ushort UsDefaultChar { get; private set; }
        public ushort UsBreakChar { get; private set; }
        public ushort UsMaxContext { get; private set; }
        public ushort UsLowerOpticalPointSize { get; private set; }
        public ushort UsUpperOpticalPointSize { get; private set; }

        public static Os2Table Read(BinaryReader reader)
        {
            var instance = new Os2Table();
            instance.Version = reader.ReadUInt16BigEndian();
            instance.XAvgCharWidth = reader.ReadInt16BigEndian();
            instance.UsWeightClass = reader.ReadUInt16BigEndian();
            instance.UsWidthClass = reader.ReadUInt16BigEndian();
            instance.FsType = reader.ReadUInt16BigEndian();
            instance.YSubscriptXSize = reader.ReadInt16BigEndian();
            instance.YSubscriptYSize = reader.ReadInt16BigEndian();
            instance.YSubscriptXOffset = reader.ReadInt16BigEndian();
            instance.YSubscriptYOffset = reader.ReadInt16BigEndian();
            instance.YSuperscriptXSize = reader.ReadInt16BigEndian();
            instance.YSuperscriptYSize = reader.ReadInt16BigEndian();
            instance.YSuperscriptXOffset = reader.ReadInt16BigEndian();
            instance.YSuperscriptYOffset = reader.ReadInt16BigEndian();
            instance.YStrikeoutSize = reader.ReadInt16BigEndian();
            instance.YStrikeoutPosition = reader.ReadInt16BigEndian();
            instance.SFamilyClass = reader.ReadInt16BigEndian();
            instance.Panose = reader.ReadBytes(10);
            instance.UlUnicodeRange1 = reader.ReadUInt32BigEndian();
            instance.UlUnicodeRange2 = reader.ReadUInt32BigEndian();
            instance.UlUnicodeRange3 = reader.ReadUInt32BigEndian();
            instance.UlUnicodeRange4 = reader.ReadUInt32BigEndian();
            instance.AchVendID = reader.ReadBytes(4);
            instance.FsSelection = reader.ReadUInt16BigEndian();
            instance.UsFirstCharIndex = reader.ReadUInt16BigEndian();
            instance.UsLastCharIndex = reader.ReadUInt16BigEndian();
            instance.STypoAscender = reader.ReadInt16BigEndian();
            instance.STypoDescender = reader.ReadInt16BigEndian();
            instance.STypoLineGap = reader.ReadInt16BigEndian();
            instance.UsWinAscent = reader.ReadUInt16BigEndian();
            instance.UsWinDescent = reader.ReadUInt16BigEndian();
            instance.UlCodePageRange1 = reader.ReadUInt32BigEndian();
            instance.UlCodePageRange2 = reader.ReadUInt32BigEndian();
            instance.SxHeight = reader.ReadInt16BigEndian();
            instance.SCapHeight = reader.ReadInt16BigEndian();
            instance.UsDefaultChar = reader.ReadUInt16BigEndian();
            instance.UsBreakChar = reader.ReadUInt16BigEndian();
            instance.UsMaxContext = reader.ReadUInt16BigEndian();
            instance.UsLowerOpticalPointSize = reader.ReadUInt16BigEndian();
            instance.UsUpperOpticalPointSize = reader.ReadUInt16BigEndian();

            return instance;
        }
    }
}
