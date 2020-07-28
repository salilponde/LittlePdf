using LittlePdf.Core.CrossReferencing;
using LittlePdf.Core.Objects;
using LittlePdf.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LittlePdf.Core.Writers
{
    public class PdfWriter
    {
        private readonly Stream _stream;
        private readonly PdfBooleanWriter _booleanWriter;
        private readonly PdfIntegerWriter _integerWriter;
        private readonly PdfRealWriter _realWriter;
        private readonly PdfStringWriter _stringWriter;
        private readonly PdfNameWriter _nameWriter;
        private readonly PdfArrayWriter _arrayWriter;
        private readonly PdfDictionaryWriter _dictionaryWriter;
        private readonly PdfIndirectObjectWriter _indirectObjectWriter;
        private readonly PdfCommentWriter _commentWriter;
        private readonly PdfIndirectObjectReferenceWriter _indirectObjectReferenceWriter;
        private readonly PdfCrossReferenceTableWriter _crossReferenceTableWriter;
        private readonly PdfTrailerWriter _pdfTrailerWriter;

        public PdfWriter(Stream stream)
        {
            _stream = stream;
            _booleanWriter = new PdfBooleanWriter(_stream);
            _integerWriter = new PdfIntegerWriter(_stream);
            _realWriter = new PdfRealWriter(_stream);
            _stringWriter = new PdfStringWriter(_stream);
            _nameWriter = new PdfNameWriter(_stream);
            _arrayWriter = new PdfArrayWriter(_stream, this);
            _dictionaryWriter = new PdfDictionaryWriter(_stream, this);
            _indirectObjectWriter = new PdfIndirectObjectWriter(_stream, this);
            _commentWriter = new PdfCommentWriter(_stream);
            _crossReferenceTableWriter = new PdfCrossReferenceTableWriter(_stream);
            _indirectObjectReferenceWriter = new PdfIndirectObjectReferenceWriter(_stream);
            _pdfTrailerWriter = new PdfTrailerWriter(_stream);
        }

        public async Task AddNewLine()
        {
            await _stream.WriteByteAsync((byte)'\r');
            await _stream.WriteByteAsync((byte)'\n');
        }

        public async Task WriteAsync(PdfObject pdfObject)
        {
            if (pdfObject is PdfBoolean pdfBoolean)
            {
                await _booleanWriter.WriteAsync(pdfBoolean);
            }
            else if (pdfObject is PdfInteger pdfInteger)
            {
                await _integerWriter.WriteAsync(pdfInteger);
            }
            else if (pdfObject is PdfReal pdfReal)
            {
                await _realWriter.WriteAsync(pdfReal);
            }
            else if (pdfObject is PdfString pdfString)
            {
                await _stringWriter.WriteAsync(pdfString);
            }
            else if (pdfObject is PdfName pdfName)
            {
                await _nameWriter.WriteAsync(pdfName);
            }
            else if (pdfObject is PdfArray pdfArray)
            {
                await _arrayWriter.WriteAsync(pdfArray);
            }
            else if (pdfObject is PdfDictionary pdfDictionary)
            {
                await _dictionaryWriter.WriteAsync(pdfDictionary);
            }
            else if (pdfObject is PdfIndirectObject indirectObject)
            {
                await _indirectObjectWriter.WriteAsync(indirectObject);
            }
            else if (pdfObject is PdfComment pdfComment)
            {
                await _commentWriter.WriteAsync(pdfComment);
            }
            else if (pdfObject is PdfIndirectObjectReference pdfIndirectObjectReference)
            {
                await _indirectObjectReferenceWriter.WriteAsync(pdfIndirectObjectReference);
            }
            else
            {
                throw new Exception("Unknown PdfObject");
            }
        }

        public async Task WriteAsync(PdfCrossReferenceTable pdfCrossReferenceTable)
        {
            await _crossReferenceTableWriter.WriteAsync(pdfCrossReferenceTable);
        }

        public async Task WriteAsync(PdfTrailer pdfTrailer)
        {
            await _pdfTrailerWriter.WriteAsync(pdfTrailer);
        }
    }
}
