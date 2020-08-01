using System.Collections.Generic;

namespace LittlePdf.Core.Wrapping
{
    public class Row
    {
        public double Width { get; internal set; }
        public double WidthWithoutSpacing { get; internal set; }
        public double ShortestItemHeight { get; internal set; }
        public double TallestItemHeight { get; internal set; }
        public IList<ISolid> Items { get; } = new List<ISolid>();
    }
}
