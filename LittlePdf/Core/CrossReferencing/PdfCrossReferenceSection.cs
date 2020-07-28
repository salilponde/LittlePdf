using System.Collections.Generic;

namespace LittlePdf.Core.CrossReferencing
{
    public class PdfCrossReferenceSection
    {
        public List<PdfCrossReferenceSubSection> SubSections { get; }

        public PdfCrossReferenceSection(List<PdfCrossReferenceSubSection> subSections)
        {
            SubSections = subSections;
        }
    }
}
