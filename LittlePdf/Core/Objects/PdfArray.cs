using System.Collections.Generic;

namespace LittlePdf.Core.Objects
{
    public class PdfArray : PdfObject
    {
        public List<PdfObject> Items { get; }

        public PdfArray(List<PdfObject> items)
        {
            Items = items;
        }
    }
}
