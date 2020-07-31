namespace LittlePdf.Core
{
    public class XRightYDownAxes : IAxes
    {
        private double _pageHeight;

        public XRightYDownAxes(double pageWidth, double pageHeight)
        {
            SetPageSize(pageWidth, pageHeight);
        }

        public void SetPageSize(double width, double height)
        {
            _pageHeight = height;
        }

        public (double, double) GetPoint(double x, double y)
        {
            return (x, _pageHeight - y);
        }

        public (double, double) GetRectangleCorner(double x, double y, double width, double height)
        {
            return (x, _pageHeight - (y + height));
        }
    }
}
