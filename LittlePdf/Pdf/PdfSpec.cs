using System.Collections.Generic;
using System.Text;

namespace LittlePdf.Pdf
{
    public static class PdfSpec
    {
        public static byte[] True = Encoding.ASCII.GetBytes("true");
        public static byte[] False = Encoding.ASCII.GetBytes("false");
        public static byte[] Null = Encoding.ASCII.GetBytes("null");
        public static byte[] Space = Encoding.ASCII.GetBytes(" ");
        public static byte[] NewLine = Encoding.ASCII.GetBytes("\n");

        public static byte[] CommentBegin = Encoding.ASCII.GetBytes("%");

        public static byte[] ArrayBegin = Encoding.ASCII.GetBytes("[");
        public static byte[] ArrayEnd = Encoding.ASCII.GetBytes("]");

        public static byte[] DictionaryBegin = Encoding.ASCII.GetBytes("<<");
        public static byte[] DictionaryEnd = Encoding.ASCII.GetBytes(">>");

        public static byte[] StreamBegin = Encoding.ASCII.GetBytes("stream\n");
        public static byte[] StreamEnd = Encoding.ASCII.GetBytes("endstream\n");

        public static byte[] IndirectObjectBegin = Encoding.ASCII.GetBytes("obj\n");
        public static byte[] IndirectObjectEnd = Encoding.ASCII.GetBytes("endobj\n");

        public static byte[] XrefBegin = Encoding.ASCII.GetBytes("xref\n");

        public static byte[] XrefInUse = Encoding.ASCII.GetBytes("n");
        public static byte[] XrefFree = Encoding.ASCII.GetBytes("f");

        public static bool IsDelimiterCharacter(char c)
        {
            switch (c)
            {
                case '(':
                case ')':
                case '<':
                case '>':
                case '[':
                case ']':
                case '{':
                case '}':
                case '/':
                case '%':
                    return true;
            }

            return false;
        }

        public static bool IsWhiteSpaceCharacter(char c)
        {
            switch ((int)c)
            {
                case 0:     // Null
                case 9:     // Horizontal Tab
                case 10:    // Line Feed
                case 12:    // Form Feed
                case 13:    // Carriage Return
                case 32:    // Space
                    return true;
            }

            return false;
        }

        public static Dictionary<char, string> EscapeSequences = new Dictionary<char, string>
        {
            { '\n', "\\n" },
            { '\r', "\\r" },
            { '\t', "\\t" },
            { '\b', "\\b" },
            { '\f', "\\f" },
            { '(', "\\(" },
            { ')', "\\)" },
            { '\\', "\\\\" }
        };
    }
}
