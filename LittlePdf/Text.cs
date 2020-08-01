using LittlePdf.Pdf;
using SixLabors.Fonts;
using System.Text;

namespace LittlePdf
{
    public class Text : PageObject
    {
        public string Value { get; set; }

        public Text(Container parent) : base(parent)
        {
        }

        internal override void Paint(Page page, StringBuilder output)
        {
            var (x, y) = page.Axes.GetPoint(AbsoluteX, AbsoluteY);
            var valueBytes = PdfString.Encode(Value);
            var value = Encoding.ASCII.GetString(valueBytes);

            FontCollection fonts = new FontCollection();
            FontFamily font1Family = fonts.Install(@"C:\Users\sal\AppData\Local\Microsoft\Windows\Fonts\Helvetica.ttf");
            var font1 = font1Family.CreateFont(24);

            var m = TextMeasurer.Measure(Value, new RendererOptions(font1));
            var textHeight = m.Height;
            var textWidth = m.Width;

            //var h = new System.Drawing.Font("Helvetica", 24);
            //var s = System.Drawing.Graphics.FromHwnd(IntPtr.Zero).MeasureString(Value, h);
            //var textHeight = s.Height * 0.75;

            //output.Append($"BT /F1 24 Tf {x} {y - textHeight} Td {value} Tj ET ");
            output.Append($"BT /F1 24 Tf 1 0 0 1 {x} {y - textHeight} Tm {value} Tj ET ");
        }
    }
}
