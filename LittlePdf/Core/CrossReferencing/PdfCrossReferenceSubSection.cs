using System.Collections.Generic;

namespace LittlePdf.Core.CrossReferencing
{
    public class PdfCrossReferenceSubSection
    {
        public int StartObjectNumber { get; }
        public int TotalEntries { get; }
        public List<PdfCrossReferenceEntry> Entries { get; }

        public PdfCrossReferenceSubSection(int startObjectNumber, int totalEntries, List<PdfCrossReferenceEntry> entries)
        {
            StartObjectNumber = startObjectNumber;
            TotalEntries = totalEntries;
            Entries = entries;
        }
    }
}
