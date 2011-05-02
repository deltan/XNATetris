using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace deltan.XNALibrary.Control.Scene
{
    /// <summary>
    /// コンポーネント管理クラス
    /// </summary>
    public class ComponentManager : IComponentManager
    {
        /// <summary>
        /// Gameクラス
        /// </summary>
        public Game Game { get; set; }

        /// <summary>
        /// 所属するコンポーネントが使用できるコンテントマネージャー
        /// </summary>
        public ContentManager ContentManager { get; set; }

        /// <summary>
        /// コンポーネント初期化クラス
        /// </summary>
        public IComponentInitialiser SceneInitialiser { get; set; }

        /// <summary>
        /// 所属するコンポーネント（すべてのリスト）
        /// </summary>
        private HashSet<GameComponent> _baseComponents = new HashSet<GameComponent>();
        
        /// <summary>
        /// 所属するコンポーネント（DrawableGameComponentのリスト）
        /// </summary>
        private HashSet<DrawableGameComponent> _drawableComponents = new HashSet<DrawableGameComponent>();

        /// <summary>
        /// コンポーネントがGame.Componentsに登録されているか
        /// </summary>
        private bool _loaded;

        /// <summary>
        /// コンポーネントがGame.Componentsに登録されているか
        /// </summary>
        public bool Loaded
        {
            get
            {
                return _loaded;
            }
        }

        /// <summary>
        /// コンポーネントの描画処理が実行されるか
        /// </summary>
        private bool _visible;

        /// <summary>
        /// コンポーネントの描画処理が実行されるか
        /// </summary>
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;

                foreach (DrawableGameComponent drawableComponent in _drawableComponents)
                {
                    drawableComponent.Visible = _visible;                 
                }
            }
        }

        /// <summary>
        /// コンポーネントの更新処理が実行されるか
        /// </summary>
        private bool _enabled;

        /// <summary>
        /// コンポーネントの更新処理が実行されるか
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;

                foreach (GameComponent baseComponent in _baseComponents)
                {
                    baseComponent.Enabled = _enabled;
                }
            }
        }

        /// <summary>
        /// 所属するコンポーネントのなかで最大の更新順位
        /// </summary>
        public int MaxUpdateOrder
        {
            get
            {
                return _baseComponents.Max(baseComonent => baseComonent.UpdateOrder);
            }
        }

        /// <summary>
        /// 所属するコンポーネントのなかで最小の更新順位
        /// </summary>
        public int MinUpdateOrder
        {
            get
            {
                return _baseComponents.Min(baseComponent => baseComponent.UpdateOrder);
            }
        }

        /// <summary>
        /// 所属するコンポーネントのなかで最大の描画順位
        /// </summary>
        public int MaxDrawOrder
        {
            get
            {
                return _drawableComponents.Max(drawComponent => drawComponent.DrawOrder);
            }
        }

        /// <summary>
        /// 所属するコンポーネントの中で最小の描画順位
        /// </summary>
        public int MinDrawOrder
        {
            get
            {
                return _drawableComponents.Min(drawComponent => drawComponent.DrawOrder);
            }
        }

        /// <summary>
        /// コンポーネント管理クラスを初期化する
        /// </summary>
        /// <param name="game"></param>
        /// <param name="sceneInitialiser"></param>
        /// <param name="contentManager"></param>
        public ComponentManager(Game game, IComponentInitialiser sceneInitialiser, ContentManager contentManager)
        {
            Game = game;
            SceneInitialiser = sceneInitialiser;
            ContentManager = contentManager;
        }

        /// <summary>
        /// コンポーネントを追加する
        /// Loadedがtrueの場合、Game.Componentsに追加で登録される
        /// Loadedがfalseの場合、RegistInitialiseComponentメソッドが実行されるまで登録されない
        /// </summary>
        /// <param name="component"></param>
        public void AddComponent(IGameComponent component)
        {
            GameComponent baseComponent = component as GameComponent;
            if (baseComponent != null)
            {
                _baseComponents.Add(baseComponent);
            }

            DrawableGameComponent drawableComponent = component as DrawableGameComponent;
            if (drawableComponent != null)
            {
                _drawableComponents.Add(drawableComponent);
            }

            // シーン読み込み後に追加されたコンポーネントを登録
            if (Loaded)
            {
                Game.Components.Add(component);
            }
        }

        /// <summary>
        /// 初期化時に追加されたコンポーネントを追加する
        /// </summary>
        private void RegistInitialiseComponents()
        {
            if (!Loaded)
            {
                foreach (GameComponent component in _baseComponents)
                {
                    Game.Components.Add(component);
                }
            }
        }

        /// <summary>
        /// コンポーネント初期化クラスを呼び出してから、
        /// 追加されたクラスをGame.Componentsに登録する
        /// </summary>
        public void Load()
        {
            if (!Loaded)
            {
                SceneInitialiser.Initialise(this, ContentManager);
                RegistInitialiseComponents();

                _loaded = true;  
            }
        }

        /// <summary>
        /// 所属するコンポーネントの更新順位を追加した順にインクリメントで設定する
        /// </summary>
        /// <param name="beforeScene"></param>
        public void IncUpdateOrder(IComponentManager beforeScene)
        {
            int baseUpdateOrder;
            if (beforeScene == null)
            {
                baseUpdateOrder = 0;
            }
            else
            {
                baseUpdateOrder = beforeScene.MaxUpdateOrder + 1;
            }

            IncUpdateOrder(baseUpdateOrder);
        }

        /// <summary>
        /// 所属するコンポーネントの更新順位を追加した順にインクリメントで設定する
        /// </summary>
        /// <param name="beforeScene"></param>
        public void IncUpdateOrder(int baseUpdateOrder)
        {
            int updateOrder = baseUpdateOrder;
            foreach (GameComponent baseComponent in _baseComponents)
            {
                baseComponent.UpdateOrder = updateOrder;
                updateOrder++;
            }
        }

        /// <summary>
        /// 所属するコンポーネントの描画順位を追加した順にインクリメントで設定する
        /// </summary>
        /// <param name="beforeScene"></param>
        public void IncDrawOrder(IComponentManager beforeScene)
        {
            int baseDrawOrder;
            if (beforeScene == null)
            {
                baseDrawOrder = 0;
            }
            else
            {
                baseDrawOrder = beforeScene.MaxDrawOrder + 1;
            }

            IncDrawOrder(baseDrawOrder);
        }

        /// <summary>
        /// 所属するコンポーネントの描画順位を追加した順にインクリメントで設定する
        /// </summary>
        /// <param name="beforeScene"></param>
        public void IncDrawOrder(int baseDrawOrder)
        {
            int drawOrder = baseDrawOrder;
            foreach (DrawableGameComponent drawableComponent in _drawableComponents)
            {
                drawableComponent.DrawOrder = drawOrder;
                drawOrder++;
            }
        }

        /// <summary>
        /// 所属するコンポーネントの更新順位に加算する
        /// </summary>
        /// <param name="beforeScene"></param>
        public void AddUpdateOrder(IComponentManager beforeScene)
        {
            int baseUpdateOrder;
            if (beforeScene == null)
            {
                baseUpdateOrder = 0;
            }
            else
            {
                baseUpdateOrder = beforeScene.MaxUpdateOrder + 1;
            }

            AddUpdateOrder(baseUpdateOrder);
        }

        /// <summary>
        /// 所属するコンポーネントの更新順位に加算する
        /// </summary>
        /// <param name="beforeScene"></param>
        public void AddUpdateOrder(int baseUpdateOrder)
        {
            foreach (GameComponent baseComponent in _baseComponents)
            {
                baseComponent.UpdateOrder += baseUpdateOrder;
            }
        }

        /// <summary>
        /// 所属するコンポーネントの描画順位に加算する
        /// </summary>
        public void AddDrawOrder(IComponentManager beforeScene)
        {
            int baseDrawOrder;
            if (beforeScene == null)
            {
                baseDrawOrder = 0;
            }
            else
            {
                baseDrawOrder = beforeScene.MaxDrawOrder + 1;
            }

            AddDrawOrder(baseDrawOrder);
        }

        /// <summary>
        /// 所属するコンポーネントの描画順位に加算する
        /// </summary>
        public void AddDrawOrder(int baseDrawOrder)
        {
            foreach (DrawableGameComponent drawableComponent in _drawableComponents)
            {
                drawableComponent.DrawOrder += baseDrawOrder;
            }
        }

        /// <summary>
        /// 所属するコンポーネントをGame.Componentsから解除し、すべて破棄する
        /// </summary>
        public void Unload()
        {
            if (Loaded)
            {
                foreach (IGameComponent baseComponent in _baseComponents)
                {
                    Game.Components.Remove(baseComponent);
                }

                _baseComponents.Clear();
                _drawableComponents.Clear();

                _loaded = false;       
            }
        }

    }
}
