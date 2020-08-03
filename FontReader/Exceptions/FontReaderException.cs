using System;

namespace FontReader.Exceptions
{
    public class FontReaderException : Exception
    {
        public FontReaderException(string message) : base(message)
        {
        }
    }
}
