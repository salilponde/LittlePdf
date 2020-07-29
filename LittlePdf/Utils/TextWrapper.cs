using System;
using System.Collections.Generic;
using System.Text;

namespace LittlePdf.Utils
{
    public class Line
    {
        public List<string> Words { get; internal set; } = new List<string>();
        public double RemainingSpace { get; internal set; }
    }

    public class Tokenizer
    {
        private string _text;
        private int _length;
        private int _p = 0;
        private bool _lastWasWord = false;

        public Tokenizer(string text)
        {
            _text = text;
            _length = text.Length;
        }

        public string Next()
        {
            var sb = new StringBuilder();

            if (_lastWasWord)
            {
                while (_p < _length)
                {
                    var c = _text[_p];

                    if (c == ' ' || c == '\t')
                    {
                        sb.Append(c);
                    }
                    else
                    {
                        _lastWasWord = false;
                        break;
                    }
                }
            }
            while (_p < _length)
            {
                var c = _text[_p];

                if (c == ' ' || c == '\t' || c == '\r' || c == '\n')
                {
                    break;
                }

                sb.Append(c);
            }

            return sb.ToString();
        }
    }

    public class TextWrapper
    {
        private double _boxWidth;
        private double _boxHeight;

        public TextWrapper(double width, double height = -1)
        {
            _boxWidth = width;
            _boxHeight = height;
        }

        public List<Line> Wrap(string text)
        {
            var ret = new List<Line>();
            if (text == null || string.IsNullOrEmpty(text)) return ret;

            var pos = 0;
            var length = text.Length;
            string word = null;

            var line = new Line { RemainingSpace = _boxWidth };
            var tokenBuilder = new StringBuilder();
            while (pos < length)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    var wordWidth = GetTextWidth(word);
                    if (wordWidth <= line.RemainingSpace)
                    {
                        line.Words.Add(word);
                        var whiteSpace = 1;
                        if ((wordWidth + whiteSpace) > line.RemainingSpace) whiteSpace = 0;
                        line.RemainingSpace -= (wordWidth + whiteSpace);
                        word = null;
                        tokenBuilder.Clear();
                    }
                    else if (wordWidth <= _boxWidth)
                    {
                        ret.Add(line);
                        if (line.RemainingSpace > 0) line.RemainingSpace++;
                        line = new Line { RemainingSpace = _boxWidth };
                        line.Words.Add(word);
                        var whiteSpace = 1;
                        if ((wordWidth + whiteSpace) >= line.RemainingSpace) whiteSpace = 0;
                        line.RemainingSpace -= (wordWidth + whiteSpace);
                        word = null;
                        tokenBuilder.Clear();
                    }
                    else
                    {
                        var (part1, part2) = SplitLongWord(word, line.RemainingSpace);
                        line.Words.Add(part1);
                        ret.Add(line);
                        line.RemainingSpace = 0;
                        line = new Line { RemainingSpace = _boxWidth };
                        word = part2;
                        continue;
                    }
                }

                var c = text[pos];
                if (c == '\r')
                {
                    if (text[pos + 1] == '\n') pos += 2;    // Eat both \r and \n
                    ret.Add(line);
                    if (line.RemainingSpace > 0) line.RemainingSpace++;
                    line = new Line { RemainingSpace = _boxWidth };
                }
                else if (c == '\n')
                {
                    pos++;
                    ret.Add(line);
                    if (line.RemainingSpace > 0) line.RemainingSpace++;
                    line = new Line { RemainingSpace = _boxWidth };
                }
                else if (c != ' ' && c != '\t')
                {
                    while (c != ' ' && c != '\t' && c != '\r' && c != '\n')
                    {
                        tokenBuilder.Append(c);
                        if (++pos >= length) break;
                        c = text[pos];
                    }
                    if (c == ' ' || c == '\t') ++pos;  // Eat single space or tab
                    word = tokenBuilder.ToString();
                }
                else
                {
                    // If we are here it means that there were multiple space or tabs between words.
                    // In this case we should treat these as a word.
                    while (c == ' ' || c == '\t')
                    {
                        tokenBuilder.Append(c);
                        if (++pos >= length) break;
                        c = text[pos];
                    }
                    word = tokenBuilder.ToString();
                }
            }

            return ret;
        }

        private double GetTextWidth(string text)
        {
            return text.Length;
        }

        private (string, string) SplitLongWord(string text, double remainingSpace)
        {
            var cut = 10;

            do
            {
                var part1 = text.Substring(0, text.Length - cut);
                var part1Width = GetTextWidth(part1);
                if (part1Width < remainingSpace)
                {
                    var add = 1;
                    while (true)
                    {
                        part1 = text.Substring(0, text.Length - cut + add);
                        part1Width = GetTextWidth(part1);
                        if (part1Width > remainingSpace)
                        {
                            var part1Length = text.Length - cut + add - 1;
                            part1 = text.Substring(0, part1Length);
                            var part2 = text.Substring(part1Length);
                            return (part1, part2);
                        }
                        else
                        {
                            add++;
                        }
                    }
                }
                else
                {
                    cut += 10;
                }
            } while (cut < text.Length);

            // Should not reach here
            throw new Exception("Something went wrong when wrapping a word longer than box width");
        }
    }
}
