namespace LittlePdf
{
    public class Font
    {
        public int Id { get; }
        public string Type { get; }
        public string Name { get; }
        public string Encoding { get; }

        internal Font(int id, string name)
        {
            Id = id;
            Type = "Type1";
            Name = name;
            Encoding = "WinAnsiEncoding";
        }
    }
}
