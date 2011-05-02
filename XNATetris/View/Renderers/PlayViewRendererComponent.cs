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
using System.Diagnostics;

using deltan.XNATetris.Model.Logic;

namespace deltan.XNATetris.View.Renderers
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class PlayViewRendererComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public ContentManager ContentManager { get; set; }
        private SpriteBatch SpriteBatch { get; set; }

        // ロジック関係
        private TetrisPlaySuite _tetrisPlaySuite;
        public TetrisPlaySuite TetrisPlaySuite
        {
            get
            {
                return _tetrisPlaySuite;
            }
            set
            {
                _tetrisPlaySuite = value;

                TetrominoHolderRenderer.TetrominoHolder = _tetrisPlaySuite.TetrominoHolder;
                TetrisFieldRenderer.TetrisField = _tetrisPlaySuite.TetrisField;

                TetrisPlaySuite.AfterMinoCreated += new EventHandler(TetrisPlaySuite_AfterMinoCreated);
            }
        }

        public Stopwatch Time { get; set; }

        // 使用レンダラー関係
        private MinoOnFieldRenderer FallMinoRenderer { get; set; }
        private TetrisFieldRenderer TetrisFieldRenderer { get; set; }
        private TetrominoHolderRenderer TetrominoHolderRenderer { get; set; }
        //private MinoOnFieldRenderer GuideMinoRenderer { get; set; }

        // テクスチャ関係
        public string BackgroundContent { get; set; }
        private Texture2D _backgroundTex;
        public string ScoreContent { get; set; }
        private SpriteFont _scoreFont;
        public string TimeContent { get; set; }
        private SpriteFont _timeFont;

        public string[] FallMinoContents
        {
            get
            {
                return FallMinoRenderer.Contents;
            }
            set
            {
                FallMinoRenderer.Contents = value;
            }
        }
        public string[] TetrominoHolderContents
        {
            get
            {
                return TetrominoHolderRenderer.Contents;
            }
            set
            {
                TetrominoHolderRenderer.Contents = value;
            }
        }
        public string[] TetrisFieldContents
        {
            get
            {
                return TetrisFieldRenderer.Contents;
            }
            set
            {
                TetrisFieldRenderer.Contents = value;
            }
        }
       
        /*
        public string[] GuideMinoContents
        {
            get
            {
                return GuideMinoRenderer.Contents;
            }
            set
            {
                GuideMinoRenderer.Contents = value;
            }
        }
         */

        // 出力先関係
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
        public PositionScaleInfo[] THolderBaseInfo { get; set; }
        public PositionScaleInfo ScoreBaseInfo { get; set; }
        public PositionScaleInfo TimeBaseInfo { get; set; }

        // 画像内情報関係
        public Rectangle MaterialTetrisFieldRect { get; set; }
        public Vector2[] HolderTextureOrigins
        {
            get
            {
                return TetrominoHolderRenderer.HolderTextureOrigins;
            }
            set
            {
                TetrominoHolderRenderer.HolderTextureOrigins = value;
            }
        }

        public PlayViewRendererComponent(Game game, ContentManager contentManager)
            : base(game)
        {
            ContentManager = contentManager;
            SpriteBatch = new SpriteBatch(game.GraphicsDevice);

            FallMinoRenderer = new MinoOnFieldRenderer(contentManager, SpriteBatch);
            TetrominoHolderRenderer = new TetrominoHolderRenderer(contentManager, SpriteBatch);
            TetrisFieldRenderer = new TetrisFieldRenderer(contentManager, SpriteBatch);
            
            //GuideMinoRenderer = new MinoOnFieldRenderer(contentManager, SpriteBatch);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        void TetrisPlaySuite_AfterMinoCreated(object sender, EventArgs e)
        {
            FallMinoRenderer.MinoOnField = TetrisPlaySuite.FallMino;
        }

        private void SetupRenderers()
        {
            TetrisFieldRenderer.DestRect = TetrisFieldRect;

            TetrominoHolderRenderer.THolderInfo = new PositionScaleInfo[THolderBaseInfo.Length];
            for (int i = 0; i < THolderBaseInfo.Length; i++)
            {
                TetrominoHolderRenderer.THolderInfo[i] =
                    new PositionScaleInfo()
                    {
                        Position = new Vector2(
                            DestRect.X + THolderBaseInfo[i].Position.X * ScaleX,
                            DestRect.Y + THolderBaseInfo[i].Position.Y * ScaleY
                            ),
                        Scale = new Vector2(THolderBaseInfo[i].Scale.X * ScaleX, THolderBaseInfo[i].Scale.Y * ScaleY)
                    };
            }

            FallMinoRenderer.BlockWidth = TetrisFieldRenderer.BlockWidth;
            FallMinoRenderer.BlockHeight = TetrisFieldRenderer.BlockHeight;
            FallMinoRenderer.BaseLocation = new Point(TetrisFieldRenderer.DestRect.X, TetrisFieldRenderer.DestRect.Y);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            _backgroundTex = ContentManager.Load<Texture2D>(BackgroundContent);
            _scoreFont = ContentManager.Load<SpriteFont>(ScoreContent);
            _timeFont = ContentManager.Load<SpriteFont>(TimeContent);

            FallMinoRenderer.LoadContent();
            TetrisFieldRenderer.LoadContent();
            TetrominoHolderRenderer.LoadContent();
            //GuideMinoRenderer.LoadContent();

            SetupRenderers();

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Texture, SaveStateMode.None);
            SpriteBatch.Draw(_backgroundTex, DestRect, null, Color.White, 0.0f, new Vector2(0.0f, 0.0f), SpriteEffects.None, 0);
            SpriteBatch.End();

            SpriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Texture, SaveStateMode.None);
            FallMinoRenderer.Draw(gameTime);
            TetrisFieldRenderer.Draw(gameTime);
            SpriteBatch.End();

            SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Texture, SaveStateMode.None);
            TetrominoHolderRenderer.Draw(gameTime);
            //GuideMinoRenderer.Draw(gameTime);
            DrawScore();
            DrawTime();
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawScore()
        {
            PositionScaleInfo scorePositionScale = new PositionScaleInfo()
            {
                Position = new Vector2(
                    DestRect.X + ScoreBaseInfo.Position.X * ScaleX,
                    DestRect.Y + ScoreBaseInfo.Position.Y * ScaleY
                    ),
                Scale = new Vector2(
                    ScoreBaseInfo.Scale.X * ScaleX,
                    ScoreBaseInfo.Scale.Y * ScaleY
                    )
            };

            string score = String.Format("{0:D7}", TetrisPlaySuite.TetrisScoreCounter.Score);
            SpriteBatch.DrawString(_scoreFont, score, scorePositionScale.Position, Color.White, 0.0f, new Vector2(), scorePositionScale.Scale, SpriteEffects.None, 0);
        }

        private void DrawTime()
        {
            PositionScaleInfo timePositionScale = new PositionScaleInfo()
            {
                Position = new Vector2(
                    DestRect.X + TimeBaseInfo.Position.X * ScaleX,
                    DestRect.Y + TimeBaseInfo.Position.Y * ScaleY
                    ),
                Scale = new Vector2(
                    TimeBaseInfo.Scale.X * ScaleX,
                    TimeBaseInfo.Scale.Y * ScaleY
                    )
            };

            TimeSpan elapsedTime = Time.Elapsed;
            string time = String.Format("{0:D2}:{1:D2}:{2:D3}",
                elapsedTime.Minutes,
                elapsedTime.Seconds,
                elapsedTime.Milliseconds);

            SpriteBatch.DrawString(_timeFont, time, timePositionScale.Position, Color.White, 0.0f, new Vector2(), timePositionScale.Scale, SpriteEffects.None, 0);
        }

        private float ScaleX
        {
            get
            {
                return (_backgroundTex == null) ? 0 : (float)DestRect.Width / (float)_backgroundTex.Width;
            }
        }

        private float ScaleY
        {
            get
            {
                return (_backgroundTex == null) ? 0 : (float)DestRect.Height / (float)_backgroundTex.Height;
            }
        }

        private Rectangle TetrisFieldRect
        {
            get
            {
                return new Rectangle(
                    DestRect.X + (int)Math.Round(MaterialTetrisFieldRect.X * ScaleX),
                    DestRect.Y + (int)Math.Round(MaterialTetrisFieldRect.Y * ScaleY),
                    (int)Math.Round(MaterialTetrisFieldRect.Width * ScaleX),
                    (int)Math.Round(MaterialTetrisFieldRect.Height * ScaleY)
                    );
            }
        }
    }
}