using System.IO;

namespace LittlePdf.Core.Text
{
    public class Font
    {
        public string FileName { get; private set; }
        public double Size { get; set; }
        public bool Embedded { get; set; }

        public Font(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new System.Exception("File name not specified");
            if (!File.Exists(fileName)) throw new FileNotFoundException(fileName);

            FileName = fileName;
            Size = 12;
            Embedded = true;
        }
    }
}
