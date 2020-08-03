using System;
using System.Buffers.Binary;
using System.IO;

namespace FontReader.Extensions
{
    public static class BinaryReaderExtensions
    {
        public static ushort ReadUInt16BigEndian(this BinaryReader reader)
        {
            var value = reader.ReadUInt16();
            if (BitConverter.IsLittleEndian) return BinaryPrimitives.ReverseEndianness(value);
            return value;
        }

        public static ushort[] ReadUInt16BigEndianArray(this BinaryReader reader, int length)
        {
            var items = new ushort[length];

            for (int i = 0; i < length; i++)
            {
                items[i] = ReadUInt16BigEndian(reader);
            }

            return items;
        }

        public static uint ReadUInt32BigEndian(this BinaryReader reader)
        {
            var value = reader.ReadUInt32();
            if (BitConverter.IsLittleEndian) return BinaryPrimitives.ReverseEndianness(value);
            return value;
        }

        public static ulong ReadUInt64BigEndian(this BinaryReader reader)
        {
            var value = reader.ReadUInt64();
            if (BitConverter.IsLittleEndian) return BinaryPrimitives.ReverseEndianness(value);
            return value;
        }

        public static short ReadInt16BigEndian(this BinaryReader reader)
        {
            var value = reader.ReadInt16();
            if (BitConverter.IsLittleEndian) return BinaryPrimitives.ReverseEndianness(value);
            return value;
        }

        public static short[] ReadInt16BigEndianArray(this BinaryReader reader, int length)
        {
            var items = new short[length];

            for (int i = 0; i < length; i++)
            {
                items[i] = ReadInt16BigEndian(reader);
            }

            return items;
        }

        public static int ReadInt32BigEndian(this BinaryReader reader)
        {
            var value = reader.ReadInt32();
            if (BitConverter.IsLittleEndian) return BinaryPrimitives.ReverseEndianness(value);
            return value;
        }

        public static long ReadInt64BigEndian(this BinaryReader reader)
        {
            var value = reader.ReadInt64();
            if (BitConverter.IsLittleEndian) return BinaryPrimitives.ReverseEndianness(value);
            return value;
        }
    }
}
