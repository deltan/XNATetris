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

using deltan.XNALibrary.Control.Scene;
using deltan.XNATetris.Control.Scene;

namespace deltan.XNATetris
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class XNATetris : Microsoft.Xna.Framework.Game
    {
        public static int SCREEN_WIDTH { get { return 800; } }
        public static int SCREEN_HEIGHT { get { return 600; } }

        private GraphicsDeviceManager graphics;
        private SceneManager SceneManager;

        public XNATetris()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.PreferMultiSampling = false;
            graphics.IsFullScreen = false;

            this.IsMouseVisible = true;

            SceneManager = new SceneManager(this);
            this.Services.AddService(typeof(ISceneManager), SceneManager);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Window.Title = "でるたんXNAてとりす";

            SceneManager.SetSceneInitialiser(new SceneCondition("title", "basic"), new TitleSceneInitialiser(this));          
            SceneManager.SetSceneInitialiser(new SceneCondition("tetris", "basic"), new TetrisSceneInitialiser(this));

            SceneManager.SetSceneContentManagerName(new SceneCondition("title", "basic"), "title_content");
            SceneManager.SetSceneContentManagerName(new SceneCondition("tetris", "basic"), "tetis_content");

            SceneManager.NewScene(new SceneCondition("title", "basic"), null);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            SceneManager.SceneTransition();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            this.GraphicsDevice.Clear(Color.AliceBlue);

            base.Draw(gameTime);
        }
    }
}
