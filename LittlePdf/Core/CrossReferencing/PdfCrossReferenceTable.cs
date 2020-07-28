using System.Collections.Generic;

namespace LittlePdf.Core.CrossReferencing
{
    public class PdfCrossReferenceTable
    {
        public List<PdfCrossReferenceSection> Sections { get; }

        public PdfCrossReferenceTable(List<PdfCrossReferenceSection> sections)
        {
            Sections = sections;
        }
    }
}
