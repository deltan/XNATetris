using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace deltan.XNATetris.Model.Logic
{
    public class MinoBlock
    {
        private Point _location;
        public Point Location
        {
            get
            {
                return _location;
            }
        }

        private int _blockID;
        public int BlockID
        {
            get
            {
                return _blockID;
            }
        }

        public MinoBlock(Point location, int blockID)
        {
            _location = location;
            _blockID = blockID;
        }
    }
}
