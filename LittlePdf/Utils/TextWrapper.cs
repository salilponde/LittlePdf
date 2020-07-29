using System;
using System.Collections.Generic;
using System.Text;

namespace LittlePdf.Utils
{
    public class Tokenizer
    {
        private string _text;
        private int _length;
        private int _pos = 0;

        public Tokenizer(string text)
        {
            _text = text;
            _length = text.Length;
        }

        public string Next()
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

    public class Line
    {
        private double _totalWidth;
        private double _spaceWidth;
        private double _currentWordsWidth;
        private double _currentWordsAndSpacesWidth;

        public List<string> Words { get; internal set; } = new List<string>();
        public double WordsLength { get; internal set; }
        public double RemainingWidthForWords
        {
            get
            {
                return (_totalWidth - _currentWordsAndSpacesWidth);
            }
        }
        public double RemainingWidth
        {
            get
            {
                if (Words.Count == 0) return 0;

                return _totalWidth - (_currentWordsWidth + _spaceWidth * (Words.Count - 1));
            }
        }

        public Line(double width, double spaceWidth)
        {
            _totalWidth = width;
            _spaceWidth = spaceWidth;
        }

        public bool CanHold(double width)
        {
            return width <= (_totalWidth - _currentWordsAndSpacesWidth);
        }

        public void AddWord(string word, double width)
        {
            Words.Add(word);
            _currentWordsWidth += width;
            _currentWordsAndSpacesWidth += width + _spaceWidth;
        }
    }

    public class TextWrapper
    {
        private double _boxWidth;
        private double _boxHeight;
        private double _spaceWidth;

        public TextWrapper(double width, double height = -1)
        {
            _boxWidth = width;
            _boxHeight = height;
            _spaceWidth = GetTextWidth(" ");
        }

        public List<Line> Wrap(string text)
        {
            var lines = new List<Line>();
            if (text == null || string.IsNullOrEmpty(text)) return lines;

            var line = new Line(_boxWidth, _spaceWidth);

            var tokenizer = new Tokenizer(text);
            string token;
            while ((token = tokenizer.Next()) != null)
            {
                if (token == "\n")
                {
                    lines.Add(line);
                    line = new Line(_boxWidth, _spaceWidth);
                }
                else
                {
                    var wordWidth = GetTextWidth(token);
                    if (line.CanHold(wordWidth))
                    {
                        line.AddWord(token, wordWidth);
                    }
                    else 
                    {
                        lines.Add(line);
                        line = new Line(_boxWidth, _spaceWidth);

                        if (line.CanHold(wordWidth))
                        {
                            line.AddWord(token, wordWidth);
                        }
                        else
                        {
                            while (true)
                            {
                                var (part1, part2) = SplitLongWord(token, line.RemainingWidthForWords);
                                var part1Width = GetTextWidth(part1);
                                line.AddWord(part1, part1Width);
                                lines.Add(line);
                                line = new Line(_boxWidth, _spaceWidth);

                                if (!string.IsNullOrEmpty(part2))
                                {
                                    var part2Width = GetTextWidth(part2);
                                    if (line.CanHold(part2Width))
                                    {
                                        line.AddWord(part2, part2Width);
                                        break;
                                    }
                                    else
                                    {
                                        token = part2;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return lines;
        }

        private double GetTextWidth(string text)
        {
            return text.Length;
        }

        private (string, string) SplitLongWord(string text, double remainingSpace)
        {
            var cut = 1;

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
                    cut++;
                }
            } while (cut < text.Length);

            // Should not reach here
            throw new Exception("Something went wrong when wrapping a word longer than box width");
        }
    }
}
