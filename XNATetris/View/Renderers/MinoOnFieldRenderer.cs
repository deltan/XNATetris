using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using deltan.XNATetris.Model.Logic;

namespace deltan.XNATetris.View.Renderers
{
    /// <summary>
    /// テトリスフィールド上のミノを描画するクラス
    /// </summary>
    public class MinoOnFieldRenderer
    {
        public ContentManager ContentManager { get; set; }
        private SpriteBatch SpriteBatch { get; set; }

        public Point BaseLocation;
        public Mino MinoOnField { get; set; }

        public string[] Contents { get; set; }
        private Texture2D[] _blockTex;
        public float BlockWidth { get; set; }
        public float BlockHeight { get; set; }

        public MinoOnFieldRenderer(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            // TODO: Construct any child components here
            ContentManager = contentManager;
            SpriteBatch = spriteBatch;
        }


        public void LoadContent()
        {
            _blockTex = new Texture2D[Contents.Length];
            for (int i = 0; i < Contents.Length; i++)
            {
                _blockTex[i] = ContentManager.Load<Texture2D>(Contents[i]);
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (!MinoOnField.Finished)
            {
                foreach (MinoBlock block in MinoOnField.CurrentMinoBlock)
                {
                    int blockID = MinoOnField.ID;

                    Rectangle dest = new Rectangle();
                    dest.X = BaseLocation.X + GetDrawX(MinoOnField.Location.X + block.Location.X);
                    dest.Y = BaseLocation.Y + GetDrawY(MinoOnField.Location.Y + block.Location.Y);
                    dest.Width =
                        GetDrawX(MinoOnField.Location.X + block.Location.X + 1) -
                        GetDrawX(MinoOnField.Location.X + block.Location.X);
                    dest.Height =
                        GetDrawY(MinoOnField.Location.Y + block.Location.Y + 1) -
                        GetDrawY(MinoOnField.Location.Y + block.Location.Y);

                    SpriteBatch.Draw(_blockTex[MinoOnField.ID], dest, null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), SpriteEffects.None, 0);
                }
            }
        }

        private int GetDrawX(int x)
        {
            return (int)(x * BlockWidth);
        }

        private int GetDrawY(int y)
        {
            return (int)(y * BlockHeight);
        }
    }
}