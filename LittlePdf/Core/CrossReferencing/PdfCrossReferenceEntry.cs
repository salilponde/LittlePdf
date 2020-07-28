namespace LittlePdf.Core.CrossReferencing
{
    public enum CrossReferenceEntryType
    {
        InUse,  // n
        Free    // f
    }

    public class PdfCrossReferenceEntry
    {
        public long ByteOffset { get; }
        public int GenerationNumber { get; }
        public CrossReferenceEntryType Type { get; }

        public PdfCrossReferenceEntry(long byteOffset, int generationNumber, CrossReferenceEntryType type)
        {
            ByteOffset = byteOffset;
            GenerationNumber = generationNumber;
            Type = type;
        }
    }
}
