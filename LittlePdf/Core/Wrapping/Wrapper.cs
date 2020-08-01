using System;
using System.Collections.Generic;
using System.Linq;

namespace LittlePdf.Core.Wrapping
{
    public class Wrapper : IWrapper
    {
        public ISolidSplitter Splitter { get; set; }
        public double ItemSpacing { get; set; }

        private List<Row> _rows;
        private Row _currentRow;
        private double _width;
        private double _remainingSpace;
        private double _remainingSpaceAfterAddingItem;
        private LinkedList<ISolid> _remainingItems;

        public IEnumerable<Row> Wrap(IEnumerable<ISolid> items, double width)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (width <= 0) throw new ArgumentException(nameof(width), "width should be a positive number");

            _width = width;
            _rows = new List<Row>();

            if (items.Count() == 0) return _rows;

            NewRow();
            _remainingSpace = width;
            _remainingItems = new LinkedList<ISolid>(items);
            while (_remainingItems.Count > 0)
            {
                var item = _remainingItems.First();

                _remainingSpaceAfterAddingItem = _remainingSpace - item.Width;
                if (_remainingSpaceAfterAddingItem >= 0)
                {
                    AddItemToRow(item);

                    var canAccomodateSpace = _remainingSpaceAfterAddingItem >= ItemSpacing;
                    if (!canAccomodateSpace)
                    {
                        NewRow();
                    }
                }
                else if (item.Width > _width)
                {
                    EndRow();
                    NewRow();
                    var newItems = Splitter.Split(item, _width);
                    _remainingItems.RemoveFirst();
                    for (int i = newItems.Count - 1; i >= 0; i--)
                    {
                        _remainingItems.AddFirst(newItems[i]);
                    }
                }
                else
                {
                    EndRow();
                    NewRow();
                    AddItemToRow(item);
                }
            }

            return _rows;
        }

        private void NewRow()
        {
            _currentRow = new Row();
            _rows.Add(_currentRow);
            _remainingSpace = _width;
        }

        private void EndRow()
        {
            _rows.Add(_currentRow);
            _currentRow = null;
        }

        private void AddItemToRow(ISolid item)
        {
            _currentRow.Items.Add(item);
            _remainingItems.RemoveFirst();

            if (item.Height < _currentRow.ShortestItemHeight) _currentRow.ShortestItemHeight = item.Height;
            if (item.Height > _currentRow.TallestItemHeight) _currentRow.TallestItemHeight = item.Height;

            _currentRow.WidthWithoutSpacing += item.Width;
            _currentRow.Width += item.Width;

            var canAccomodateSpace = _remainingSpaceAfterAddingItem >= ItemSpacing;
            if (canAccomodateSpace)
            {
                _currentRow.Width += ItemSpacing;
                _remainingSpace = _width - _currentRow.Width;
            }
            else
            {
                NewRow();
            }
        }
    }
}
