﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace deltan.XNATetris.Model.Logic
{
    sealed class Mino2 : Mino
    {
        public override int ID
        {
            get { return 2; }
        }
        protected override IList<IList<MinoBlock>> CreateMinoBlocks()
        {
            IList<IList<MinoBlock>> minoBlocks = new List<IList<MinoBlock>>
            {
                new List<MinoBlock> 
                {
                    new MinoBlock(new Point(0, 0), 2),
                    new MinoBlock(new Point(-1, 0), 2), 
                    new MinoBlock(new Point(0, 1), 2),
                    new MinoBlock(new Point(1, 1), 2)
                },
                new List<MinoBlock> 
                {
                    new MinoBlock(new Point(0, 0), 2),
                    new MinoBlock(new Point(0, -1), 2),
                    new MinoBlock(new Point(-1, 0), 2),
                    new MinoBlock(new Point(-1, 1), 2)
                },
                new List<MinoBlock> 
                {
                    new MinoBlock(new Point(0, 0), 2),
                    new MinoBlock(new Point(1, 0), 2),
                    new MinoBlock(new Point(0, -1), 2),
                    new MinoBlock(new Point(-1, -1), 2)
                },
                new List<MinoBlock> 
                { 
                    new MinoBlock(new Point(0, 0), 2),
                    new MinoBlock(new Point(0, 1), 2),
                    new MinoBlock(new Point(1, 0), 2),
                    new MinoBlock(new Point(1, -1), 2)
                },
            };

            return minoBlocks;
        }
    }
}
