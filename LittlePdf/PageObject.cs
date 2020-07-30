using System.Text;

namespace LittlePdf
{
    public abstract class PageObject
    {
        public double X { get; set; }
        public double Y { get; set; }

        internal abstract void Paint(StringBuilder output, double x, double y, double height);
    }
}
