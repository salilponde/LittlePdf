namespace LittlePdf.Core.Text
{
    public class TextChunk : ISolid
    {
        public TextStyle Style { get; set; }

        public double Width => throw new System.NotImplementedException();

        public double Height => throw new System.NotImplementedException();

        public TextChunk(string text)
        {
        }
    }
}
