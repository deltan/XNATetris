using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace deltan.XNATetris.Model.Logic
{
    public abstract class Mino
    {
        public abstract int ID { get; }
        public IList<IList<MinoBlock>> MinoBlocks { get; private set; }

        private int _minoAngleNumber = 0;
        public int MinoAngleNumber
        {
            get
            {
                return _minoAngleNumber;
            }
            private set
            {
                _minoAngleNumber = value;

                if (_minoAngleNumber < 0)
                {
                    _minoAngleNumber = MinoBlocks.Count - 1;
                }
                else
                {
                    _minoAngleNumber %= MinoBlocks.Count;
                }
            }
        }

        private Point _location;
        public Point Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }
        
        public TetrisField TetrisField { get; set; }

        private float fallSpeedSum = 0;
        public float FallSpeed { get; set; }

        public bool Finished { get; private set; }

        public int DisableFrames { get; set; }

        public Mino()
        {
            MinoBlocks = CreateMinoBlocks();
        }

        protected abstract IList<IList<MinoBlock>> CreateMinoBlocks();

        public IList<MinoBlock> CurrentMinoBlock
        {
            get
            {
                if (MinoBlocks.Count >= 1)
                {
                    return MinoBlocks[_minoAngleNumber];
                }
                else
                {
                    return null;
                }
            }
        }

        public Mino ShallowCopy()
        {
            return (Mino)MemberwiseClone();
        }

        public int Width
        {
            get
            {
                int max = 0;
                int min = 0;
                foreach (MinoBlock minoBlock in CurrentMinoBlock)
                {
                    if (max < minoBlock.Location.X)
                    {
                        max = minoBlock.Location.X;
                    }
                    if (min > minoBlock.Location.X)
                    {
                        min = minoBlock.Location.X;
                    }
                }
                return max - min + 1;
            }
        }

        public int Height
        {
            get
            {
                int max = 0;
                int min = 0;
                foreach (MinoBlock minoBlock in CurrentMinoBlock)
                {
                    if (max < minoBlock.Location.Y)
                    {
                        max = minoBlock.Location.Y;
                    }
                    if (min > minoBlock.Location.Y)
                    {
                        min = minoBlock.Location.Y;
                    }
                }
                return max - min + 1;
            }
        }

        public void FitTop()
        {
            if (TetrisField == null)
            {
                return;
            }

            int minY = 0;
            foreach (MinoBlock block in CurrentMinoBlock)
            {
                if (minY > block.Location.Y)
                {
                    minY = block.Location.Y;
                }
            }

            _location.Y = -minY;
        }

        public void AutoFall()
        {
            if (Finished)
            {
                return;
            }

            if (DisableFrames <= 0)
            {
                fallSpeedSum += FallSpeed;

                int fallBlocks = (int)(fallSpeedSum / 1.0f);

                if (fallBlocks >= 1)
                {
                    fallSpeedSum = 0;

                    bool isFallen = Fall(fallBlocks);

                    if (!isFallen)
                    {
                        Finish();
                    }
                }
            }
            else
            {
                DisableFrames--;
            }
        }

        private bool Fall(int fallBlocks)
        {
            bool isFallen = false;

            if (fallBlocks >= 1)
            {
                isFallen = true;

                for (int i = 0; i < fallBlocks; i++)
                {
                    _location.Y++;

                    if (IsExtraBottom() || IsDuplicative())
                    {
                        _location.Y--;

                        if (i == 0)
                        {
                            isFallen = false;
                        }

                        break;
                    }
                }
            }

            return isFallen;
        }

        private void Finish()
        {
            foreach (MinoBlock block in CurrentMinoBlock)
            {
                if (TetrisField.IsInRange(Location.X + block.Location.X, Location.Y + block.Location.Y))
                {
                    TetrisField[Location.Y + block.Location.Y, Location.X + block.Location.X].BlockID = block.BlockID;
                }
            }

            Finished = true;
        }

        public void RotateRight()
        {
            if (Finished || DisableFrames > 0)
            {
                return;
            }

            MinoAngleNumber++;

            if (IsExtraRight() ||
                IsExtraLeft() ||
                IsExtraBottom() ||
                IsExtraTop() ||
                IsDuplicative())
            {
                MinoAngleNumber--;
            }
        }

        public void RotateLeft()
        {
            if (Finished || DisableFrames > 0)
            {
                return;
            }

            MinoAngleNumber--;

            if (IsExtraRight() ||
                IsExtraLeft() ||
                IsExtraBottom() ||
                IsExtraTop() ||
                IsDuplicative())
            {
                MinoAngleNumber++;    
            }
        }


        public void FallBottom()
        {
            if (Finished || DisableFrames > 0)
            {
                return;
            }

            Fall(TetrisField.Height - Location.Y);

            fallSpeedSum = 0;

            Finish();
        }

        public void MoveBottom()
        {
            if (Finished || DisableFrames > 0)
            {
                return;
            }

            _location.Y++;

            if (IsExtraBottom() || IsDuplicative())
            {
                _location.Y--;

                Finish();
            }

            fallSpeedSum = 0;
            DisableFrames = 1;
        }

        public void MoveRight()
        {
            if (Finished || DisableFrames > 0)
            {
                return;
            }

            _location.X++;

            if (IsExtraRight() || IsDuplicative())
            {
                _location.X--;
            }
        }

        public void MoveLeft()
        {
            if (Finished || DisableFrames > 0)
            {
                return;
            }

            _location.X--;

            if (IsExtraLeft() || IsDuplicative())
            {
                _location.X++;
            }
        }

        private bool IsExtraRight()
        {
            return IsBadLocation(block => Location.X + block.Location.X >= TetrisField.Width);
        }

        private bool IsExtraLeft()
        {
            return IsBadLocation(block => Location.X + block.Location.X < 0);
        }

        private bool IsExtraBottom()
        {
            return IsBadLocation(block => Location.Y + block.Location.Y >= TetrisField.Height);
        }

        private bool IsExtraTop()
        {
            return IsBadLocation(block => Location.Y + block.Location.Y < 0);
        }

        public bool IsDuplicative()
        {            
            return IsBadLocation(block =>
                    TetrisField.IsInRange(Location.X + block.Location.X, Location.Y + block.Location.Y) ?
                    TetrisField[Location.Y + block.Location.Y, Location.X + block.Location.X].IsBlock : false
                );
        }

        public bool IsDuplicativeNext()
        {
            return IsBadLocation(block =>
                    TetrisField.IsInRange(Location.X + block.Location.X, Location.Y + block.Location.Y + 1) ?
                    TetrisField[Location.Y + block.Location.Y + 1, Location.X + block.Location.X].IsBlock : false
                );
        }

        private bool IsBadLocation(Func<MinoBlock, bool> Pred)
        {
            bool isBad = false;

            foreach (MinoBlock block in CurrentMinoBlock)
            {
                if (Pred(block))
                {
                    isBad = true;
                    break;
                }
            }

            return isBad;
        }
    }
}
