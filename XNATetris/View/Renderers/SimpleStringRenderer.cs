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

using deltan.XNATetris.Control.Scene;

namespace deltan.XNATetris.View.Renderers
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SimpleStringRenderer : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public ContentManager ContentManager { get; set; }

        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        public string Text { get; set; }
        public Vector2 Position { get; set; }

        public SimpleStringRenderer(Game game, ContentManager contentManager)
            : base(game)
        {
            // TODO: Construct any child components here
            ContentManager = contentManager;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _font = ContentManager.Load<SpriteFont>("Title");

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);

            _spriteBatch.DrawString(_font, Text, Position, Color.Red);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}