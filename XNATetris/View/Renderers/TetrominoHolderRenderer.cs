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
    /// 保持されたミノを描画するクラス
    /// </summary>
    public class TetrominoHolderRenderer
    {
        /// <summary>
        /// コンテントマネージャー
        /// </summary>
        public ContentManager ContentManager { get; set; }

        /// <summary>
        /// スプライトバッチ
        /// </summary>
        public SpriteBatch SpriteBatch { get; set; }

        // ロジック関係
        public TetrominoHolder TetrominoHolder { get; set; }

        // テクスチャ関係
        public string[] Contents { get; set; }
        private Texture2D[] _minoTex;

        // 出力先関係
        public PositionScaleInfo[] THolderInfo { get; set; }

        // 画像内情報関係
        public Vector2[] HolderTextureOrigins { get; set; }

        public TetrominoHolderRenderer(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            // TODO: Construct any child components here
            ContentManager = contentManager;
            SpriteBatch = spriteBatch;
        }

        public void LoadContent()
        {
            _minoTex = new Texture2D[Contents.Length];
            for (int i = 0; i < Contents.Length; i++)
            {
                _minoTex[i] = ContentManager.Load<Texture2D>(Contents[i]);
            }
        }


        public void Draw(GameTime gameTime)
        {
            IList<Mino> HoldTetrominos = TetrominoHolder.HoldTetrominoArray;
            for (int i = 0; i < THolderInfo.Length; i++)
            {
                Mino mino = HoldTetrominos[i];

                SpriteBatch.Draw(
                    _minoTex[mino.ID],
                    THolderInfo[i].Position,
                    null,
                    Color.White,
                    0.0f,
                    HolderTextureOrigins[mino.ID],
                    THolderInfo[i].Scale,
                    SpriteEffects.None,
                    0);
            }
        }
    }
}