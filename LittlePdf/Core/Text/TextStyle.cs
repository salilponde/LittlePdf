namespace LittlePdf.Core.Text
{
    public enum VerticalAlignment
    {
        Top,
        Middle,
        Bottom,
        Custom
    }

    public class TextStyle
    {
        public int Font { get; set; }
        public double FontSize { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Underline { get; set; }
        public bool Strikethrough { get; set; }
        public string Color { get; set; }
        public string BackgroundColor { get; set; }
        public double LetterSpacing { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }
        public VerticalAlignment DistanceFromTop { get; set; }
    }
}
