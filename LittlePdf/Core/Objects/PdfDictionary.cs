using System.Collections.Generic;

namespace LittlePdf.Core.Objects
{
    public class PdfDictionary : PdfObject
    {
        public Dictionary<PdfName, PdfObject> Pairs { get; }

        public PdfDictionary(Dictionary<PdfName, PdfObject> pairs)
        {
            Pairs = pairs;
        }
    }
}
