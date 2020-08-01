using System.Collections.Generic;

namespace LittlePdf.Core.Wrapping
{
    public interface ISolidSplitter
    {
        List<ISolid> Split(ISolid item, double maxWidth);
    }
}
