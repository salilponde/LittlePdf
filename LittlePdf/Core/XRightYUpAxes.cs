namespace LittlePdf.Core
{
    public class XRightYUpAxes : IAxes
    {
        public XRightYUpAxes(double pageWidth, double pageHeight)
        {
        }

        public void SetPageSize(double width, double height)
        {
        }

        public (double, double) GetPoint(double x, double y)
        {
            return (x, y);
        }

        public (double, double) GetRectangleCorner(double x, double y, double width, double height)
        {
            return (x, y);
        }
    }
}
