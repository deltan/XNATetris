using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace deltan.XNATetris.Model.Logic
{
    sealed class Mino6 : Mino
    {
        public override int ID
        {
            get { return 6; }
        }
        protected override IList<IList<MinoBlock>> CreateMinoBlocks()
        {
            IList<IList<MinoBlock>> minoBlocks = new List<IList<MinoBlock>>
            { 
                new List<MinoBlock>
                { 
                    new MinoBlock(new Point(0, 0), 6),
                    new MinoBlock(new Point(-1, 0), 6),
                    new MinoBlock(new Point(0, -1), 6),
                    new MinoBlock(new Point(1, 0), 6)
                },
                new List<MinoBlock>
                { 
                    new MinoBlock(new Point(0, 0), 6),
                    new MinoBlock(new Point(0, -1), 6),
                    new MinoBlock(new Point(1, 0), 6),
                    new MinoBlock(new Point(0, 1), 6)
                },
                new List<MinoBlock>
                { 
                    new MinoBlock(new Point(0, 0), 6),
                    new MinoBlock(new Point(-1, 0), 6),
                    new MinoBlock(new Point(0, 1), 6),
                    new MinoBlock(new Point(1, 0), 6)
                },
                new List<MinoBlock>
                {
                    new MinoBlock(new Point(0, 0), 6),
                    new MinoBlock(new Point(0, -1), 6),
                    new MinoBlock(new Point(-1, 0), 6),
                    new MinoBlock(new Point(0, 1), 6)
                }
            };
            return minoBlocks;
        }
    }
}
