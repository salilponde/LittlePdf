using System;
using System.Text;

namespace LittlePdf.Core
{
    public class WhitespaceTextTokenizer : ITextTokenizer
    {
        private string _text;
        private int _length;
        private int _pos = 0;

        public void SetText(string text)
        {
            _text = text ?? throw new ArgumentNullException(nameof(text));
            _length = text.Length;
            Reset();
        }

        public void Reset()
        {
            _pos = 0;
        }

        public string GetNextToken()
        {
            while (_pos < _length)
            {
                var c = _text[_pos];

                if (c == '\r')
                {
                    _pos++;
                    if (_text[_pos] == '\n') _pos++;
                    return "\n";
                }
                else if (c == '\n')
                {
                    _pos++;
                    return "\n";
                }
                else if (c == ' ' || c == '\t')
                {
                    _pos++;

                    var sb = new StringBuilder();
                    while (true)
                    {
                        c = _text[_pos];
                        if (c == ' ' || c == '\t')
                        {
                            sb.Append(' ');
                            _pos++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    var spaces = sb.ToString();
                    if (!string.IsNullOrEmpty(spaces)) return spaces;
                }
                else
                {
                    var sb = new StringBuilder();
                    while (_pos < _length)
                    {
                        c = _text[_pos];
                        if (c != ' ' && c != '\t' && c != '\r' && c != '\n')
                        {
                            sb.Append(c);
                            _pos++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    return sb.ToString();
                }
            }

            return null;
        }
    }
}
