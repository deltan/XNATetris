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

using deltan.XNALibrary.Control.Services;
using deltan.XNATetris.Model.Logic;

namespace deltan.XNATetris.Control.Controllers
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class TetrisKeyboardController : Microsoft.Xna.Framework.GameComponent
    {
        public TetrisPlaySuite TetrisPlaySuite { get; set; }

        private KeyboardService ks = new KeyboardService(PlayerIndex.One)
        {
            RepeatIntervalFrame = new Dictionary<Keys, int>
            {
                {Keys.Right, 1},
                {Keys.Left, 1},
                {Keys.Down, 1},
            },
            RepeatLatencyFrame = new Dictionary<Keys, int>
            {
                {Keys.Right, 5},
                {Keys.Left, 5},
                {Keys.Down, 5},
            }
        };

        public TetrisKeyboardController(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (!TetrisPlaySuite.Finished)
            {
                // TODO: Add your update logic here
                ks.Begin();
                try
                {
                    if (ks.IsKeyDown(Keys.Z))
                    {
                        TetrisPlaySuite.FallMino.RotateLeft();
                    }
                    if (ks.IsKeyDown(Keys.X))
                    {
                        TetrisPlaySuite.FallMino.RotateRight();
                    }
                    if (ks.IsKeyRepeat(Keys.Down))
                    {
                        TetrisPlaySuite.FallMino.MoveBottom();
                    }
                    if (ks.IsKeyRepeat(Keys.Right))
                    {
                        TetrisPlaySuite.FallMino.MoveRight();
                    }
                    if (ks.IsKeyRepeat(Keys.Left))
                    {
                        TetrisPlaySuite.FallMino.MoveLeft();
                    }
                    if (ks.IsKeyDown(Keys.Up))
                    {
                        TetrisPlaySuite.FallMino.FallBottom();
                    }
                }
                finally
                {
                    ks.End();
                }
            }

            base.Update(gameTime);
        }
    }
}