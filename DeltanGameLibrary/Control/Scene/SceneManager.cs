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
    /// シーン遷移の指定
    /// </summary>
    public enum TransitionOrder
    {
        None,       // 遷移なし
        New,        // 新しいシーンの作成
        Back,       // 前のシーンに戻る
    }

    /// <summary>
    /// シーン管理クラス
    /// </summary>
    public class SceneManager : ISceneManager
    {
        /// <summary>
        /// Gameクラス
        /// </summary>
        public Game Game { get; set; }

        /// <summary>
        /// シーンで使用するオブジェクトの管理を行うクラス
        /// </summary>
        private SceneObjectManager SceneObjectManager { get; set; }

        /// <summary>
        /// 次のシーン遷移の指定
        /// </summary>
        private TransitionOrder _nextOrder = TransitionOrder.None;

        /// <summary>
        /// 次のシーン名
        /// </summary>
        private SceneCondition _nextSceneCondition = null;

        /// <summary>
        /// シーン遷移情報
        /// </summary>
        private TransitionInfo _nextTransitionInfo = null;

        /// <summary>
        /// シーン管理クラスを作成する
        /// </summary>
        /// <param name="game"></param>
        public SceneManager(Game game)
        {
            Game = game;

            SceneObjectManager = new SceneObjectManager(game);
        }

        /// <summary>
        /// シーンのコンテントマネージャー名をセットする
        /// </summary>
        /// <param name="sceneCondition"></param>
        /// <param name="contentManagerName"></param>
        public void SetSceneContentManagerName(SceneCondition sceneCondition, string contentManagerName)
        {
            SceneObjectManager.SetSceneContentManagerName(sceneCondition, contentManagerName);
        }

        /// <summary>
        /// シーンのコンポーネント初期化クラスを登録する
        /// </summary>
        /// <param name="sceneCondition"></param>
        /// <param name="componentInitialiser"></param>
        public void SetSceneInitialiser(SceneCondition sceneCondition, IComponentInitialiser componentInitialiser)
        {
            SceneObjectManager.SetSceneInitialiser(sceneCondition, componentInitialiser);
        }

        /// <summary>
        /// シーンが読み込まれたとき、他にプリロードするシーン名を指定する
        /// </summary>
        /// <param name="sceneCondition"></param>
        /// <param name="preloadList"></param>
        public void SetPreloadSceneConditions(SceneCondition sceneCondition, IList<SceneCondition> preloadList)
        {
            SceneObjectManager.SetPreloadSceneConditions(sceneCondition, preloadList);
        }

        /// <summary>
        /// シーン遷移処理
        /// </summary>
        public void SceneTransition()
        {
            switch (_nextOrder)
            {
                case TransitionOrder.New:
                    NewTransition();
                    break;
                case TransitionOrder.Back:
                    BackTransition();
                    break;
            }

            _nextOrder = TransitionOrder.None;
            _nextSceneCondition = null;
            _nextTransitionInfo = null;
        }

        /// <summary>
        /// シーン遷移処理：新しいシーンの作成
        /// </summary>
        private void NewTransition()
        {
            if (_nextTransitionInfo == null)
            {
                _nextTransitionInfo = new TransitionInfo();
            }

            bool clearHistory = !_nextTransitionInfo.Backable;

            SceneData nextSceneData = SceneObjectManager.NextNewScene(_nextSceneCondition, clearHistory);

            SceneData lastSceneData = SceneObjectManager.LastHistorySceneData;
            if (lastSceneData != null)
            {
                lastSceneData.ComponentManager.Enabled = _nextTransitionInfo.CurrentSceneEnabled;
                lastSceneData.ComponentManager.Visible = _nextTransitionInfo.CurrentSceneVisible;
            }

            nextSceneData.ComponentManager.Enabled = true;
            nextSceneData.ComponentManager.Visible = true;

            switch (_nextTransitionInfo.NewUpdateOrderAssignment)
            {
                case UpdateOrderAssignment.INCREMENT_FROM_BASE_VALUE:
                    nextSceneData.ComponentManager.IncUpdateOrder(_nextTransitionInfo.NewBaseUpdateOrder);
                    break;
                case UpdateOrderAssignment.INCREMENT_FROM_CURRENT_SCENE:
                    nextSceneData.ComponentManager.IncUpdateOrder(lastSceneData != null ? lastSceneData.ComponentManager : null);
                    break;
                case UpdateOrderAssignment.ADD_BASE_VALUE:
                    nextSceneData.ComponentManager.AddUpdateOrder(_nextTransitionInfo.NewBaseUpdateOrder);
                    break;
                case UpdateOrderAssignment.ADD_CURRENT_SCENE:
                    nextSceneData.ComponentManager.AddUpdateOrder(lastSceneData != null ? lastSceneData.ComponentManager : null);
                    break;
            }

            switch (_nextTransitionInfo.NewDrawOrderAssignment)
            {
                case DrawOrderAssignment.INCREMENT_FROM_BASE_VALUE:
                    nextSceneData.ComponentManager.IncDrawOrder(_nextTransitionInfo.NewBaseDrawOrder);
                    break;
                case DrawOrderAssignment.INCREMENT_FROM_CURRENT_SCENE:
                    nextSceneData.ComponentManager.IncDrawOrder(lastSceneData != null ? lastSceneData.ComponentManager : null);
                    break;
                case DrawOrderAssignment.ADD_BASE_VALUE:
                    nextSceneData.ComponentManager.AddDrawOrder(_nextTransitionInfo.NewBaseDrawOrder);
                    break;
                case DrawOrderAssignment.ADD_CURRENT_SCENE:
                    nextSceneData.ComponentManager.AddDrawOrder(lastSceneData != null ? lastSceneData.ComponentManager : null);
                    break;
            }
        }

        /// <summary>
        /// シーン遷移処理：前のシーンに戻る
        /// </summary>
        private void BackTransition()
        {
            SceneData backSceneData = SceneObjectManager.PrevScene();

            if (backSceneData != null)
            {
                backSceneData.ComponentManager.Enabled = true;
                backSceneData.ComponentManager.Visible = true;
            }
        }

        /// <summary>
        /// シーン遷移指定：新しいシーンの作成
        /// 遷移情報はデフォルトが使われる
        /// </summary>
        /// <param name="nextSceneCondition"></param>
        public void NewScene(SceneCondition nextSceneCondition)
        {
            NewScene(nextSceneCondition, null);
        }

        /// <summary>
        /// シーン遷移指定：新しいシーンの作成
        /// </summary>
        /// <param name="nextSceneCondition"></param>
        /// <param name="transitionInfo"></param>
        public void NewScene(SceneCondition nextSceneCondition, TransitionInfo transitionInfo)
        {
            _nextOrder = TransitionOrder.New;
            _nextSceneCondition = nextSceneCondition;
            _nextTransitionInfo = transitionInfo;
        }

        /// <summary>
        /// シーン遷移指定：前のシーンに戻る
        /// </summary>
        public void BackScene()
        {
            _nextOrder = TransitionOrder.Back;
            _nextSceneCondition = null;
        }

    }
}
