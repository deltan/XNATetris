using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace deltan.XNATetris.Model.Logic
{
    sealed class Mino3 : Mino
    {
        public override int ID
        {
            get { return 3; }
        }
        protected override IList<IList<MinoBlock>> CreateMinoBlocks()
        {
            IList<IList<MinoBlock>> minoBlocks = new List<IList<MinoBlock>> 
            { 
                new List<MinoBlock>
                { 
                    new MinoBlock(new Point(0, 0), 3),
                    new MinoBlock(new Point(1, 0), 3),
                    new MinoBlock(new Point(0, 1), 3),
                    new MinoBlock(new Point(-1, 1), 3)
                },
                new List<MinoBlock> 
                { 
                    new MinoBlock(new Point(0, 0), 3),
                    new MinoBlock(new Point(0, 1), 3),
                    new MinoBlock(new Point(-1, 0), 3),
                    new MinoBlock(new Point(-1, -1), 3)
                },
                new List<MinoBlock> 
                {
                    new MinoBlock(new Point(0, 0), 3),
                    new MinoBlock(new Point(-1, 0), 3),
                    new MinoBlock(new Point(0, -1), 3),
                    new MinoBlock(new Point(1, -1), 3)
                },
                new List<MinoBlock>
                { 
                    new MinoBlock(new Point(0, 0), 3),
                    new MinoBlock(new Point(0, -1), 3),
                    new MinoBlock(new Point(1, 0), 3),
                    new MinoBlock(new Point(1, 1), 3)
                },
            };

            return minoBlocks;
        }
    }
}
