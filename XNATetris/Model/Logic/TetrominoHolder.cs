using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace deltan.XNATetris.Model.Logic
{
    public class TetrominoHolder
    {
        private const int HOLD_MINO_MAX = 4;

        private Queue<Mino> holdTetrominos = new Queue<Mino>(HOLD_MINO_MAX);
        public IList<Mino> HoldTetrominoArray
        {
            get
            {
                return holdTetrominos.ToArray();
            }
        }

        public ITetrominoGenerator TetrominoGenerator { get; set; }

        public TetrominoHolder(ITetrominoGenerator initTetrominoGenerator)
        {
            TetrominoGenerator = initTetrominoGenerator;
        }

        public Mino GetNextMino()
        {
            Mino nextMino = null;

            if (holdTetrominos.Count == 0)
            {
                FillHoldTetriminos();
            }

            if (holdTetrominos.Count >= 1)
            {
                nextMino = holdTetrominos.Dequeue();
                FillHoldTetriminos();
            }

            return nextMino;
        }

        private void FillHoldTetriminos()
        {
            while (holdTetrominos.Count < HOLD_MINO_MAX)
            {
                holdTetrominos.Enqueue(TetrominoGenerator.GetNewMino());
            }
        }
    }
}
