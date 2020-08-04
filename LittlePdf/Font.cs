using System.IO;

namespace LittlePdf
{
    public enum FontEncoding
    {
        WinAnsiEncoding
    }

    public class FontStyle
    {
        public string Color { get; set; }
        public bool Underline { get; set; }
        public bool StrikeThrough { get; set; }

        // Don't add Bold, Italic here as it is part of the font file
    }

    public class Font
    {
        public string FileName { get; private set; }
        public string Type { get; private set; }
        public string Name { get; private set; }
        public FontEncoding Encoding { get; private set; }
        public FontStyle Style { get; private set; }
        public double Size { get; private set; }

        internal DumpFont.Font FontInfo { get; private set; }

        public static Font ReadFromFile(string fileName, double size, FontStyle style, FontEncoding encoding = FontEncoding.WinAnsiEncoding)
        {
            var instance = new Font
            {
                FileName = fileName,
                Type = "TrueType",
                Name = new FileInfo(fileName).Name,    // TODO: Read from 'name' table
                Encoding = encoding,
                Style = style,
                Size = size,
                FontInfo = DumpFont.Font.ReadFromFile(fileName)
            };
            return instance;
        }
    }
}
