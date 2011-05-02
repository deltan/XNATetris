using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace deltan.XNATetris.Model.Logic
{
    public class TetrisField
    {
        private IList<IList<TetrisFieldElement>> elements;

        public TetrisFieldElement this[int height, int width]
        {
            get
            {
                return elements[height][width];
            }
        }

        private int _height;
        public int Height
        {
            get
            {
                return _height;
            }
        }

        private int _width;
        public int Width
        {
            get
            {
                return _width;
            }
        }

        public bool IsInRange(int x, int y)
        {
            return (x < Width) &&
                   (x >= 0) &&
                   (y < Height) &&
                   (y >= 0);
        }

        public TetrisField(int width, int height)
        {
            Create(width, height);
        }

        public void Create(int width, int height)
        {
            _width = width;
            _height = height;

            elements = new List<IList<TetrisFieldElement>>(Height);

            FillNewLines(elementLine => elements.Add(elementLine));
        }

        public int ClearLines()
        {
            int removedLine = 0;

            for (int h = Height - 1; h >= 0; h--)
            {
                if (IsFilledLine(h))
                {
                    elements.RemoveAt(h);
                    removedLine++;
                }
            }

            FillNewLines(elementLine => elements.Insert(0, elementLine));

            return removedLine;
        }

        private bool IsFilledLine(int height)
        {
            bool isFilledLine = true;

            for (int w = 0; w < Width; w++)
            {
                if (!elements[height][w].IsBlock)
                {
                    isFilledLine = false;
                    break;
                }
            }

            return isFilledLine;
        }

        private void FillNewLines(Action<IList<TetrisFieldElement>> Pred)
        {
            while (elements.Count < Height)
            {
                IList<TetrisFieldElement> elementLine = CreateLine();

                Pred(elementLine);
            }
        }

        private IList<TetrisFieldElement> CreateLine()
        {
            IList<TetrisFieldElement> elementLine = new List<TetrisFieldElement>(Width);

            for (int w = 0; w < Width; w++)
            {
                elementLine.Add(new TetrisFieldElement());
            }

            return elementLine;
        }
    }
}
