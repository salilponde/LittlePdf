using System.Collections.Generic;

namespace LittlePdf.Core.Wrapping
{
    public interface IWrapper
    {
        ISolidSplitter Splitter { get; set; }
        double ItemSpacing { get; set; }
        IEnumerable<Row> Wrap(IEnumerable<ISolid> items, double width);
    }
}
