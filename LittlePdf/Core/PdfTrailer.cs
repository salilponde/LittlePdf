using LittlePdf.Core.Objects;

namespace LittlePdf.Core
{
    public class PdfTrailer
    {
        public int Size { get; }
        public PdfIndirectObjectReference Root { get; set; }
        public PdfIndirectObjectReference Info { get; set; }
        public long StartXrefOffset { get; }

        public PdfTrailer(int size, PdfIndirectObjectReference root, PdfIndirectObjectReference info, long startXrefOffset)
        {
            Size = size;
            Root = root;
            Info = info;
            StartXrefOffset = startXrefOffset;
        }
    }
}
