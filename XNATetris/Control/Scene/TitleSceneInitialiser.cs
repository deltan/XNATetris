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
using deltan.XNATetris.View.Renderers;

namespace deltan.XNATetris.Control.Scene
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class TitleSceneInitialiser : IComponentInitialiser
    {
        public Game Game { get; set; }

        public TitleSceneInitialiser(Game game)
        {
            Game = game;
        }

        public void Initialise(IComponentManager componentManager, ContentManager contentManager)
        {
            componentManager.AddComponent(new KeyDownMoveScene(Game) 
            { 
                Key = Keys.Enter,
                TransitionOrder = TransitionOrder.New,
                MoveCondition = new SceneCondition("tetris", "basic"),
                InitSceneInfo = new TransitionInfo()
                {
                    CurrentSceneEnabled = false,
                    CurrentSceneVisible = true,
                    NewDrawOrderAssignment = DrawOrderAssignment.ADD_CURRENT_SCENE,
                    NewUpdateOrderAssignment = UpdateOrderAssignment.ADD_CURRENT_SCENE,
                    Backable = false,
                },
            });
            componentManager.AddComponent(new SimpleStringRenderer(Game, contentManager) { Text = "It's Title!", Position = new Vector2(0, 0), });
        }

    }
}