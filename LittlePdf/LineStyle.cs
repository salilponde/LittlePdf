using System.Text;

namespace LittlePdf
{
    public class LineStyle : StyleBase
    {
        public CapStyle CapStyle { get; set; } = CapStyle.Butt;
        public string DashPattern { get; set; } = LittlePdf.DashPattern.Solid;
        public JoinStyle JoinStyle { get; set; } = JoinStyle.Milter;
        public double MilterJoinLimit { get; set; } = 2.0;  // 2.0 will convert join less 60 degrees to bevel
        public double Width { get; set; } = 1.0;

        internal override void Paint(StringBuilder output)
        {
            output.Append($"{Width} w {DashPattern} d {(int) CapStyle} J {(int) JoinStyle} j ");
            if (JoinStyle == JoinStyle.Milter)
            {
                output.Append($"{MilterJoinLimit} M ");
            }
        }
    }

    public static class DashPattern
    {
        public static string Dashed { get; set; } = "[3] 0";
        public static string Solid { get; set; } = "[] 0";
    }

    public enum CapStyle
    {
        Butt = 0,
        Round = 1,
        ProjectingSquares = 2
    }

    public enum JoinStyle
    {
        Milter = 0,
        Round = 1,
        Bevel = 2
    }
}
