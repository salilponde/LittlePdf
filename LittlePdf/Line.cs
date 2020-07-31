using System.Text;

namespace LittlePdf
{
    public class Line : PageObject
    {
        public double XEnd { get; set; }
        public double YEnd { get; set; }

        public double AbsoluteXEnd => (Parent?.AbsoluteX ?? 0.0) + XEnd;
        public double AbsoluteYEnd => (Parent?.AbsoluteY ?? 0.0) + YEnd;

        public LineStyle Style { get; set; } = new LineStyle();

        public Line(Container parent) : base(parent)
        {
        }

        internal override void Paint(Page page, StringBuilder output)
        {
            Style.Paint(output);
            var (x1, y1) = page.Axes.GetPoint(AbsoluteX, AbsoluteY);
            var (x2, y2) = page.Axes.GetPoint(AbsoluteXEnd, AbsoluteYEnd);
            output.Append($"{x1} {y1} m {x2} {y2} l S ");
        }
    }
}
