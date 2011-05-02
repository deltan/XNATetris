using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace deltan.XNATetris.Model.Logic
{
    class TetrominoGeneratorRandom : ITetrominoGenerator
    {
        private static Random randomForSeed = new Random();

        private Mino[] MinoInstances = 
        {
            new Mino0(),
            new Mino1(),
            new Mino2(),
            new Mino3(),
            new Mino4(),
            new Mino5(),
            new Mino6(),
        };

        private int seed;
        public int Seed
        {
            get
            {
                return seed;
            }
            set
            {
                seed = value;
                randomForTetromino = new Random(seed);
            }
        }

        private Random randomForTetromino;

        public TetrominoGeneratorRandom()
        {
            Seed = randomForSeed.Next();
        }

        public TetrominoGeneratorRandom(int initSeed)
        {
            Seed = initSeed;
        }

        public Mino GetNewMino()
        {
            int minoNumber = randomForTetromino.Next(MinoInstances.Length);

            Mino newMino = MinoInstances[minoNumber].ShallowCopy();

            return newMino;
        }

        public TetrominoGeneratorRandom Clone()
        {
            TetrominoGeneratorRandom newGen = new TetrominoGeneratorRandom(Seed);

            return newGen;
        }
    }
}
