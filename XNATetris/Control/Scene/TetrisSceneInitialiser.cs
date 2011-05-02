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

using deltan.XNALibrary.Control.Scene;
using deltan.XNATetris.View.Renderers;
using deltan.XNATetris.Model.Logic;
using deltan.XNATetris.Control.Controllers;
using deltan.XNATetris.Control.Scene;

namespace deltan.XNATetris.Control.Scene
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class TetrisSceneInitialiser : IComponentInitialiser
    {
        public Game Game { get; set; }

        public ISceneManager SceneManager
        {
            get
            {
                return (ISceneManager)Game.Services.GetService(typeof(ISceneManager));
            }
        }

        private const int MINO_MAX = 7;
        private const int BLOCK_MAX = 7;
        private const int FIELD_WIDTH = 10;
        private const int FIELD_HEIGHT = 20;
        private const int START_LOCATION_X = 4;
        private const int START_LOCATION_Y = 0;
        private const float START_FALL_SPEED = 0.0167f;

        private const int PLAYVIEW_X = 100;
        private const int PLAYVIEW_Y = 10;
        private const int PLAYVIEW_WIDTH = 380;
        private const int PLAYVIEW_HEIGHT = 540;
        //private const int PLAYVIEW_WIDTH = 280;
        //private const int PLAYVIEW_HEIGHT = 440;

        private const int MATERIAL_FIELD_X = 8;
        private const int MATERIAL_FIELD_Y = 9;
        private const int MATERIAL_FIELD_WIDTH = 260;
        private const int MATERIAL_FIELD_HEIGHT = 520;

        private Stopwatch _time = new Stopwatch();

        public TetrisSceneInitialiser(Game game)
        {
            Game = game;
        }

        public void Initialise(IComponentManager componentManager, ContentManager contentManager)
        {
            InitTetris(componentManager, contentManager);
        }

        private void InitTetris(IComponentManager componentManager, ContentManager contentManager)
        {
            TetrisPlaySuite tetrisPlaySuite;
            tetrisPlaySuite = new TetrisPlaySuite(Game);
            tetrisPlaySuite.TetrominoHolder = new TetrominoHolder(new TetrominoGeneratorRandom());
            tetrisPlaySuite.TetrisField = new TetrisField(FIELD_WIDTH, FIELD_HEIGHT);
            tetrisPlaySuite.InitFallMinoLocation = new Point(START_LOCATION_X, START_LOCATION_Y);
            tetrisPlaySuite.InitFallMinoSpeed = START_FALL_SPEED;
            tetrisPlaySuite.TetrisScoreCounter = new SimpleTetrisScoreCounter();
            tetrisPlaySuite.AfterFinished += new EventHandler(_tetrisPlayLogic_AfterFinished);
            tetrisPlaySuite.UpdateOrder = 1;
            componentManager.AddComponent(tetrisPlaySuite);

            TetrisKeyboardController keyboardController;
            keyboardController = new TetrisKeyboardController(Game);
            keyboardController.TetrisPlaySuite = tetrisPlaySuite;
            keyboardController.UpdateOrder = 2;
            componentManager.AddComponent(keyboardController);


            PlayViewRendererComponent playViewRenderer;
            playViewRenderer = new PlayViewRendererComponent(Game, contentManager);

            playViewRenderer.UpdateOrder = 3;
            playViewRenderer.DrawOrder = 1;
            playViewRenderer.TetrisPlaySuite = tetrisPlaySuite;

            playViewRenderer.THolderBaseInfo =
                new PositionScaleInfo[]
                {
                    new PositionScaleInfo()
                    {
                        Position = new Vector2 { X = 324, Y = 50 },
                        Scale = new Vector2 { X = 0.5f, Y = 0.5f },
                    },
                    new PositionScaleInfo()
                    {
                        Position = new Vector2 { X = 324, Y = 126 },
                        Scale = new Vector2 { X = 0.5f, Y = 0.5f },
                    },
                    new PositionScaleInfo()
                    {
                        Position = new Vector2 { X = 324, Y = 202 },
                        Scale = new Vector2 { X = 0.5f, Y = 0.5f },
                    },
                    new PositionScaleInfo()
                    {
                        Position = new Vector2 { X = 324, Y = 278 },
                        Scale = new Vector2 { X = 0.5f, Y = 0.5f },
                    },
                };

            playViewRenderer.BackgroundContent = "playviewback";

            playViewRenderer.ScoreBaseInfo = new PositionScaleInfo()
            {
                Position = new Vector2(279, 457),
                Scale = new Vector2(1.0f, 1.0f),
            };
            playViewRenderer.ScoreContent = "ScoreFont";

            playViewRenderer.TimeBaseInfo = new PositionScaleInfo()
            {
                Position = new Vector2(280, 507),
                Scale = new Vector2(1.0f, 1.0f),
            };
            playViewRenderer.TimeContent = "TimeFont";

            playViewRenderer.DestRect = new Rectangle(PLAYVIEW_X, PLAYVIEW_Y, PLAYVIEW_WIDTH, PLAYVIEW_HEIGHT);
            playViewRenderer.MaterialTetrisFieldRect = new Rectangle(MATERIAL_FIELD_X, MATERIAL_FIELD_Y, MATERIAL_FIELD_WIDTH, MATERIAL_FIELD_HEIGHT);

            playViewRenderer.Time = _time;

            playViewRenderer.TetrisFieldContents = new string[] 
            {
                "block0", "block1", "block2", "block3", "block4", "block5", "block6" 
            };

            playViewRenderer.TetrominoHolderContents = new string[]
            {
                "mino0", "mino1", "mino2", "mino3", "mino4", "mino5", "mino6",
            };

            playViewRenderer.HolderTextureOrigins = new Vector2[]
            {
                new Vector2 { X = 52, Y = 16 },
                new Vector2 { X = 26, Y = 26 },
                new Vector2 { X = 39, Y = 26 },
                new Vector2 { X = 39, Y = 26 },
                new Vector2 { X = 39, Y = 26 },
                new Vector2 { X = 39, Y = 26 },
                new Vector2 { X = 39, Y = 26 },
            };

            playViewRenderer.FallMinoContents = new string[] 
            {
                "block0", "block1", "block2", "block3", "block4", "block5", "block6" 
            };

            componentManager.AddComponent(playViewRenderer);
            
            _time.Reset();
            _time.Start();
        }

        /// <summary>
        /// テトリス終了時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _tetrisPlayLogic_AfterFinished(object sender, EventArgs e)
        {
            _time.Stop();

            Game.Window.Title = "でるたんXNAてとりす ゲームオーバー";

            SceneManager.NewScene(new SceneCondition("title", "basic"));
        }
    }
}