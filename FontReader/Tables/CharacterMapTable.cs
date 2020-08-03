using FontReader.Extensions;
using System.Collections.Generic;
using System.IO;

namespace FontReader.Tables
{
    public class CharacterMapTable
    {
        public ushort Version { get; private set; }
        public ushort NumTables { get; private set; }
        public List<EncodingRecord> EncodingRecords { get; private set; }
        public Dictionary<EncodingRecord, FormatSubTableBase> Encodings { get; private set; }

        public static CharacterMapTable Read(BinaryReader reader)
        {
            var tableOffset = reader.BaseStream.Position;

            var instance = new CharacterMapTable();
            instance.Version = reader.ReadUInt16BigEndian();
            instance.NumTables = reader.ReadUInt16BigEndian();

            instance.EncodingRecords = new List<EncodingRecord>();
            for (int i = 0; i < instance.NumTables; i++)
            {
                instance.EncodingRecords.Add(EncodingRecord.Read(reader));
            }

            instance.Encodings = new Dictionary<EncodingRecord, FormatSubTableBase>();
            for (int i = 0; i < instance.EncodingRecords.Count; i++)
            {
                var encodingRecord = instance.EncodingRecords[i];
                reader.BaseStream.Seek(tableOffset + encodingRecord.Offset, SeekOrigin.Begin);
                var format = reader.ReadUInt16BigEndian();

                if (format == 4)
                {
                    instance.Encodings[encodingRecord] = Format4SubTable.Read(format, reader);
                }
                else
                {
                    //throw new FontReaderException($"Unsupported cmap Format Table format {format}");
                }
            }

            return instance;
        }
    }

    public class EncodingRecord
    {
        public ushort PlatformID { get; private set; }
        public ushort EncodingID { get; private set; }
        public uint Offset { get; private set; }

        public static EncodingRecord Read(BinaryReader reader)
        {
            var instance = new EncodingRecord();
            instance.PlatformID = reader.ReadUInt16BigEndian();
            instance.EncodingID = reader.ReadUInt16BigEndian();
            instance.Offset = reader.ReadUInt32BigEndian();
            return instance;
        }
    }

    public abstract class FormatSubTableBase
    {
        public abstract bool TryGetGlyphId(int c, out ushort glyphId);
    }

    public class Format4SubTable : FormatSubTableBase
    {
        public ushort Format { get; private set; }
        public ushort Length { get; private set; }
        public ushort Language { get; private set; }
        public ushort SegCountX2 { get; private set; }
        public ushort SearchRange { get; private set; }
        public ushort EntrySelector { get; private set; }
        public ushort RangeShift { get; private set; }
        public ushort[] EndCode { get; private set; }
        public ushort ReservedPad { get; private set; }
        public ushort[] StartCode { get; private set; }
        public short[] IdDelta { get; private set; }
        public ushort[] IdRangeOffset { get; private set; }
        public ushort[] GlyphIdArray { get; private set; }

        public static Format4SubTable Read(ushort format, BinaryReader reader)
        {
            var instance = new Format4SubTable();
            instance.Format = format;
            instance.Length = reader.ReadUInt16BigEndian();
            instance.Language = reader.ReadUInt16BigEndian();
            instance.SegCountX2 = reader.ReadUInt16BigEndian();
            int segCount = instance.SegCountX2 / 2;
            instance.SearchRange = reader.ReadUInt16BigEndian();
            instance.EntrySelector = reader.ReadUInt16BigEndian();
            instance.RangeShift = reader.ReadUInt16BigEndian();
            instance.EndCode = reader.ReadUInt16BigEndianArray(segCount);
            instance.ReservedPad = reader.ReadUInt16BigEndian();
            instance.StartCode = reader.ReadUInt16BigEndianArray(segCount);
            instance.IdDelta = reader.ReadInt16BigEndianArray(segCount);
            instance.IdRangeOffset = reader.ReadUInt16BigEndianArray(segCount);
            instance.GlyphIdArray = reader.ReadUInt16BigEndianArray(segCount);

            return instance;
        }

        public override bool TryGetGlyphId(int c, out ushort glyphId)
        {
            var segmentCount = SegCountX2 / 2;
            var charCode = (uint)c;

            for (int i = 0; i < segmentCount; i++)
            {
                if (EndCode[i] >= charCode && StartCode[i] <= charCode)
                {
                    if (IdRangeOffset[i] == 0)
                    {
                        glyphId = (ushort)((IdDelta[i] + charCode) & 0xFFFF);
                        return true;
                    }
                    else
                    {
                        long offset = IdRangeOffset[i] / 2 + (charCode - StartCode[i]);
                        glyphId = GlyphIdArray[offset - segmentCount + i];
                        return true;
                    }
                }
            }

            glyphId = 0;
            return false;
        }
    }
}
