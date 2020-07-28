namespace LittlePdf.Core.Objects
{
    public class PdfIndirectObjectReference : PdfObject
    {
        public int ObjectNumber { get; }
        public int GenerationNumber { get; }

        public PdfIndirectObjectReference(int objectNumber, int generationNumber)
        {
            ObjectNumber = objectNumber;
            GenerationNumber = generationNumber;
        }
    }
}
