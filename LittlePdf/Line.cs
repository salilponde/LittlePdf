using System.Text;

namespace LittlePdf
{
    public class Line : PageObject
    {
        public double XEnd { get; set; }
        public double YEnd { get; set; }
        public LineStyle Style { get; set; } = new LineStyle();

        internal override void Paint(StringBuilder output, double x, double y, double height)
        {
            Style.Paint(output);
            output.Append($"{X + x} {height - (Y + y)} m {XEnd + x} {height - (YEnd + y)} l S ");
        }
    }
}
