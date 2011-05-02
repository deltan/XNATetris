using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace deltan.XNATetris.View.Renderers
{
    public class PositionScaleInfo
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }

        public PositionScaleInfo(Vector2 position, Vector2 scale)
        {
            Position = position;
            Scale = scale;
        }

        public PositionScaleInfo()
        {
        }
    }
}
