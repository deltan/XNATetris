using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace deltan.XNATetris.Model.Logic
{
    public interface ITetrominoGenerator
    {
        Mino GetNewMino();
    }
}
