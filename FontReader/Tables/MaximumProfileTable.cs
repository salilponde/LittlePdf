using FontReader.Extensions;
using System.IO;

namespace FontReader.Tables
{
    public abstract class MaximumProfileTableBase
    {
        public uint Version { get; protected set; }
        public ushort NumGlyphs { get; protected set; }
    }

    public class MaximumProfileTableV0_5 : MaximumProfileTableBase
    {
        public static MaximumProfileTableV0_5 Read(BinaryReader reader, uint version)
        {
            var instance = new MaximumProfileTableV0_5();
            instance.Version = version;
            instance.NumGlyphs = reader.ReadUInt16BigEndian();
            return instance;
        }
    }
    public class MaximumProfileTableV1 : MaximumProfileTableBase
    {
        public ushort MaxPoints { get; private set; }
        public ushort MaxContours { get; private set; }
        public ushort MaxCompositePoints { get; private set; }
        public ushort MaxCompositeContours { get; private set; }
        public ushort MaxZones { get; private set; }
        public ushort MaxTwilightPoints { get; private set; }
        public ushort MaxStorage { get; private set; }
        public ushort MaxFunctionDefs { get; private set; }
        public ushort MaxInstructionDefs { get; private set; }
        public ushort MaxStackElements { get; private set; }
        public ushort MaxSizeOfInstructions { get; private set; }
        public ushort MaxComponentElements { get; private set; }
        public ushort MaxComponentDepth { get; private set; }

        public static MaximumProfileTableV1 Read(BinaryReader reader, uint version)
        {
            var instance = new MaximumProfileTableV1();
            instance.Version = version;
            instance.NumGlyphs = reader.ReadUInt16BigEndian();
            instance.MaxPoints = reader.ReadUInt16BigEndian();
            instance.MaxContours = reader.ReadUInt16BigEndian();
            instance.MaxCompositePoints = reader.ReadUInt16BigEndian();
            instance.MaxCompositeContours = reader.ReadUInt16BigEndian();
            instance.MaxZones = reader.ReadUInt16BigEndian();
            instance.MaxTwilightPoints = reader.ReadUInt16BigEndian();
            instance.MaxStorage = reader.ReadUInt16BigEndian();
            instance.MaxFunctionDefs = reader.ReadUInt16BigEndian();
            instance.MaxInstructionDefs = reader.ReadUInt16BigEndian();
            instance.MaxStackElements = reader.ReadUInt16BigEndian();
            instance.MaxSizeOfInstructions = reader.ReadUInt16BigEndian();
            instance.MaxComponentElements = reader.ReadUInt16BigEndian();
            instance.MaxComponentDepth = reader.ReadUInt16BigEndian();
            return instance;
        }
    }
}
