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

namespace deltan.XNATetris.Control.Scene
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class KeyDownMoveScene : Microsoft.Xna.Framework.GameComponent
    {
        public Keys Key { get; set; }
        public TransitionOrder TransitionOrder { get; set; }
        public SceneCondition MoveCondition { get; set; }
        public ISceneManager SceneManager
        {
            get
            {
                return (ISceneManager)Game.Services.GetService(typeof(ISceneManager));
            }
        }
        public TransitionInfo InitSceneInfo { get; set; }

        public KeyDownMoveScene(Game game)
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
            if (Keyboard.GetState().IsKeyDown(Key))
            {
                switch (TransitionOrder)
                {
                    case TransitionOrder.New:
                        SceneManager.NewScene(MoveCondition, InitSceneInfo);
                        break;
                    case TransitionOrder.Back:
                        SceneManager.BackScene();
                        break;
                }
            }

            base.Update(gameTime);
        }
    }
}