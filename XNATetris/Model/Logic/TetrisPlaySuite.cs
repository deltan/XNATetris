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


namespace deltan.XNATetris.Model.Logic
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class TetrisPlaySuite : Microsoft.Xna.Framework.GameComponent
    {
        public Mino FallMino { get; private set; }
        public TetrisField TetrisField { get; set; }
        public TetrominoHolder TetrominoHolder { get; set; }
        public Point InitFallMinoLocation { get; set; }
        public float InitFallMinoSpeed { get; set; }
        public bool Finished { get; set; }
        public ITetrisScoreCounter TetrisScoreCounter { get; set; }

        public event EventHandler AfterFinished;
        public event EventHandler AfterMinoCreated;

        public TetrisPlaySuite(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            NewMino();

            base.Initialize();
        }

        public void NewMino()
        {
            FallMino = TetrominoHolder.GetNextMino();
            FallMino.Location = InitFallMinoLocation;
            FallMino.FallSpeed = InitFallMinoSpeed;
            FallMino.TetrisField = TetrisField;
            FallMino.FitTop();

            if (AfterMinoCreated != null)
            {
                AfterMinoCreated(this, new EventArgs());
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            if (FallMino.Finished)
            {
                int clearLines = TetrisField.ClearLines();
                TetrisScoreCounter.Count(clearLines);

                NewMino();

                if (FallMino.IsDuplicative())
                {
                    Finished = true;
                    AfterFinished(this, new EventArgs());
                }
            }

            if (!Finished)
            {
                FallMino.AutoFall();
            }

            base.Update(gameTime);
        }



    }
}