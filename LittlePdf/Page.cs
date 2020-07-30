using System.Collections.Generic;
using System.Text;

namespace LittlePdf
{
    public class Page
    {
        public double Width { get; private set; } = 595;
        public double Height { get; private set; } = 842;
        public double Unit { get; private set; } = 1.0;

        private List<PageObject> _objects { get; } = new List<PageObject>();
        internal Document Document { get; }

        public Page(Document document)
        {
            Document = document;
        }

        public void Rotate()
        {
            var tempWidth = Width;
            Width = Height;
            Height = tempWidth;
        }

        public Line AddLine(double x1, double y1, double x2, double y2)
        {
            var line = new Line { X = x1, Y = y1, XEnd = x2, YEnd = y2 };
            _objects.Add(line);
            return line;
        }

        public string Paint()
        {
            var output = new StringBuilder();

            output.Append("30 742 150 95 re W n "); // Clipping rectangle. Use this for container with overflow hidden.

            foreach (var @object in _objects)
            {
                @object.Paint(output, 0, 0, Height);
            }

            return output.ToString();
        }
    }
}
