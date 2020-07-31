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
            output.Append($"{AbsoluteX} {page.Height - AbsoluteY} m {AbsoluteXEnd} {page.Height - AbsoluteYEnd} l S ");
        }
    }
}
