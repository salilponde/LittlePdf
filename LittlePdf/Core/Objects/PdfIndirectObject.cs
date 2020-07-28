using System;

namespace LittlePdf.Core.Objects
{
    public class PdfIndirectObject : PdfObject
    {
        public int ObjectNumber { get; }        // Starts from 1
        public int GenerationNumber { get; }    // Starts from 0
        public PdfObject Value { get; }

        public PdfIndirectObject(int objectNumber, int generationNumber, PdfObject value)
        {
            if (objectNumber < 1) throw new ArgumentException($"{nameof(objectNumber)} should be greater than or equal to 1");
            if (generationNumber < 0) throw new ArgumentException($"{nameof(generationNumber)} should be greater than or equal to 0");

            ObjectNumber = objectNumber;
            GenerationNumber = generationNumber;
            Value = value;
        }
    }
}
