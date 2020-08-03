using FontReader.Extensions;
using System.IO;

namespace FontReader.Tables
{
    public class PostScriptTable    // post
    {
        public uint Version { get; private set; }          // Fixed Point Number
        public uint ItalicAngle { get; private set; }          // Fixed Point Number
        public short UnderlinePosition { get; private set; }
        public short UnderlineThickness { get; private set; }
        public uint IsFixedPitch { get; private set; }
        public uint MinMemType42 { get; private set; }
        public uint MaxMemType42 { get; private set; }
        public uint MinMemType1 { get; private set; }
        public uint MaxMemType1 { get; private set; }

        public static PostScriptTable Read(BinaryReader reader)
        {
            var instance = new PostScriptTable();
            instance.Version = reader.ReadUInt32BigEndian();
            instance.ItalicAngle = reader.ReadUInt32BigEndian();
            instance.UnderlinePosition = reader.ReadInt16BigEndian();
            instance.UnderlineThickness = reader.ReadInt16BigEndian();
            instance.IsFixedPitch = reader.ReadUInt32BigEndian();
            instance.MinMemType42 = reader.ReadUInt32BigEndian();
            instance.MaxMemType42 = reader.ReadUInt32BigEndian();
            instance.MinMemType1 = reader.ReadUInt32BigEndian();
            instance.MaxMemType1 = reader.ReadUInt32BigEndian();
            return instance;
        }
    }
}
