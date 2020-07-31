using System.Text;

namespace LittlePdf
{
    public abstract class PageObject
    {
        public double X { get; set; }
        public double Y { get; set; }

        public double AbsoluteX => (Parent?.X ?? 0.0) + X;
        public double AbsoluteY => (Parent?.Y ?? 0.0) + Y;

        public Container Parent { get; }

        public PageObject(Container parent)
        {
            Parent = parent;
        }

        internal abstract void Paint(Page page, StringBuilder output);
    }
}
