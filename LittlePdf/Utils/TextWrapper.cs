using LittlePdf.Core;
using System;
using System.Collections.Generic;

namespace LittlePdf.Utils
{
    public class Line
    {
        private readonly double _totalWidth;
        private readonly double _spaceWidth;
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
        private readonly double _spaceWidth;
        private readonly ITextTokenizer _tokenizer;

        public TextWrapper(ITextTokenizer textTokenizer = null)
        {
            _tokenizer = textTokenizer ?? new WhitespaceTextTokenizer();
            _spaceWidth = GetTextWidth(" ");
        }

        public List<Line> Wrap(string text, double width)
        {
            var lines = new List<Line>();
            if (text == null || string.IsNullOrEmpty(text)) return lines;

            var line = new Line(width, _spaceWidth);

            _tokenizer.SetText(text);
            string token;
            while ((token = _tokenizer.GetNextToken()) != null)
            {
                if (token == "\n")
                {
                    lines.Add(line);
                    line = new Line(width, _spaceWidth);
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
                        line = new Line(width, _spaceWidth);

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
                                line = new Line(width, _spaceWidth);

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
