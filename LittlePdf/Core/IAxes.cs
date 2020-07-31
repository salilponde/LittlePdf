namespace LittlePdf.Core
{
    public interface IAxes
    {
        void SetPageSize(double width, double height);
        (double, double) GetPoint(double x, double y);
        (double, double) GetRectangleCorner(double x, double y, double width, double height);
    }
}
