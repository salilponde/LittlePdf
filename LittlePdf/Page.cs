namespace LittlePdf
{
    public class Page
    {
        public double Width { get; private set; } = 595;
        public double Height { get; private set; } = 842;

        public void Rotate()
        {
            var tempWidth = Width;
            Width = Height;
            Height = tempWidth;
        }
    }
}
