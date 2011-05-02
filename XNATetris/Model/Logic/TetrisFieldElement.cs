using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace deltan.XNATetris.Model.Logic
{
    public class TetrisFieldElement
    {
        private bool isBlock = false;

        private int blockID;

        public TetrisFieldElement()
        {
        }

        public bool IsBlock
        {
            get
            {
                return isBlock;
            }
        }

        public int BlockID
        {
            set
            {
                blockID = value;
                isBlock = true;
            }
            get
            {
                return blockID;
            }
        }

        public void ClearBlock()
        {
            isBlock = false;
        }

        

    }
}
