using FontReader.Extensions;
using FontReader.Tables;
using System.IO;
using System.Linq;

namespace FontReader
{
    public class FontReader
    {
        public static Font ReadFromFile(string filename)
        {
            if (!File.Exists(filename)) throw new FileNotFoundException($"File {filename} not found");

            var fileStream = File.OpenRead(filename);
            return ReadFromStream(fileStream);
        }

        public static Font ReadFromStream(Stream stream)
        {
            var font = new Font();
            var reader = new BinaryReader(stream);

            // Offset Table
            font.OffsetTable = OffsetTable.Read(reader);

            // Head
            var headTableRecord = font.OffsetTable.TableRecords.Single(x => x.TableTag == "head");
            stream.Seek(headTableRecord.Offset, SeekOrigin.Begin);
            font.HeadTable = HeadTable.Read(reader);

            // Horizontal Header
            var horizontalHeaderTableRecord = font.OffsetTable.TableRecords.Single(x => x.TableTag == "hhea");
            stream.Seek(horizontalHeaderTableRecord.Offset, SeekOrigin.Begin);
            font.HorizontalHeaderTable = HorizontalHeaderTable.Read(reader);

            // Maximum Profile
            var maximumProfileTableRecord = font.OffsetTable.TableRecords.Single(x => x.TableTag == "maxp");
            stream.Seek(maximumProfileTableRecord.Offset, SeekOrigin.Begin);
            var maximumProfileTableVersion = reader.ReadUInt32BigEndian();
            if (maximumProfileTableVersion == 0x00005000)
            {
                font.MaximumProfileTable = MaximumProfileTableV0_5.Read(reader, maximumProfileTableVersion);
            }
            else if (maximumProfileTableVersion == 0x00010000)
            {
                font.MaximumProfileTable = MaximumProfileTableV1.Read(reader, maximumProfileTableVersion);
            }

            // Horizontal Metrics
            var horizontalMetricsTableRecord = font.OffsetTable.TableRecords.Single(x => x.TableTag == "hmtx");
            stream.Seek(horizontalMetricsTableRecord.Offset, SeekOrigin.Begin);
            font.HorizontalMetricsTable = HorizontalMetricsTable.Read(reader,
                font.HorizontalHeaderTable.NumberOfHMetrics,
                font.MaximumProfileTable.NumGlyphs);

            // PostScript
            var postScriptTableRecord = font.OffsetTable.TableRecords.Single(x => x.TableTag == "post");
            stream.Seek(postScriptTableRecord.Offset, SeekOrigin.Begin);
            font.PostScriptTable = PostScriptTable.Read(reader);

            // OS/2 and Windows
            var os2TableRecord = font.OffsetTable.TableRecords.Single(x => x.TableTag == "OS/2");
            stream.Seek(os2TableRecord.Offset, SeekOrigin.Begin);
            font.Os2Table = Os2Table.Read(reader);

            // CharacterMap
            var characterMapTableRecord = font.OffsetTable.TableRecords.Single(x => x.TableTag == "cmap");
            stream.Seek(characterMapTableRecord.Offset, SeekOrigin.Begin);
            font.CharacterMapTable = CharacterMapTable.Read(reader);

            return font;
        }
    }
}