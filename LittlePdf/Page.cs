namespace LittlePdf
{
    public class Page
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public void Rotate()
        {
            var tempWidth = Width;
            Width = Height;
            Height = tempWidth;
        }
    }
}
