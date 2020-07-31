using LittlePdf.Core;
using System.Text;

namespace LittlePdf
{
    public class Page
    {
        public IAxes Axes { get; }
        public double Width { get; private set; } = 595;
        public double Height { get; private set; } = 842;
        public double Unit { get; private set; } = 1.0;

        public Container Container { get; }
        internal Document Document { get; }

        public Page(Document document, IAxes axes)
        {
            Axes = axes;
            Container = new Container(null, 0, 0, Width, Height);
            Document = document;
        }

        public void SetSize(double width, double height)
        {
            Width = width;
            Height = height;
            Axes.SetPageSize(width, height);
        }

        public void Rotate()
        {
            var tempWidth = Width;
            Width = Height;
            Height = tempWidth;
            Axes.SetPageSize(Width, Height);
        }

        public string Paint()
        {
            var output = new StringBuilder();
            Container.Paint(this, output);
            return output.ToString();
        }
    }
}
