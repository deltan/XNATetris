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
    /// テトリスフィールドを描画するクラス
    /// </summary>
    public class TetrisFieldRenderer
    {
        public ContentManager ContentManager { get; set; }
        public SpriteBatch SpriteBatch { get; set; }

        private Rectangle _destRect;
        public Rectangle DestRect
        {
            get
            {
                return _destRect;
            }
            set
            {
                _destRect = value;
            }
        }

        public TetrisField TetrisField { get; set; }

        public string[] Contents { get; set; }
        private Texture2D[] _blockTex;

        public float BlockWidth
        {
            get
            {
                return (float)DestRect.Width / TetrisField.Width;
            }
        }
        public float BlockHeight
        {
            get
            {
                return (float)DestRect.Height / TetrisField.Height;
            }
        }

        public TetrisFieldRenderer(ContentManager contentManager, SpriteBatch spriteBatch)
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
            for (int h = 0; h < TetrisField.Height; h++)
            {
                for (int w = 0; w < TetrisField.Width; w++)
                {
                    if (TetrisField[h, w].IsBlock)
                    {
                        int blockID = TetrisField[h, w].BlockID;

                        Rectangle dest = new Rectangle();
                        dest.X = DestRect.X + GetDrawX(w);
                        dest.Y = DestRect.Y + GetDrawY(h);
                        dest.Width = GetDrawX(w + 1) - GetDrawX(w);
                        dest.Height = GetDrawY(h + 1) - GetDrawY(h);

                        SpriteBatch.Draw(_blockTex[blockID], dest, null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), SpriteEffects.None, 0);
                    }
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