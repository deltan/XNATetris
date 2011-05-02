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
    /// シーンで使うオブジェクトの管理クラス
    /// </summary>
    class SceneObjectManager
    {
        /// <summary>
        /// Gameクラス
        /// </summary>
        public Game Game { get; set; }

        /// <summary>
        /// シーンごとのコンテントマネージャー名
        /// </summary>
        private IDictionary<SceneCondition, string> _contentManagerNames =
            new Dictionary<SceneCondition, string>();

        /// <summary>
        /// コンテントマネージャー名ごとに作成される実際のコンテントマネージャー
        /// </summary>
        private IDictionary<string, ContentManager> _contentManagers =
            new Dictionary<string, ContentManager>();

        /// <summary>
        /// コンテントマネージャーを使用しているコンポーネントが所属するコンポーネント管理クラス
        /// </summary>
        private IDictionary<string, HashSet<IComponentManager>> _contentManagerUsers =
            new Dictionary<string, HashSet<IComponentManager>>();

        /// <summary>
        /// シーンごとのコンポーネント初期化クラス
        /// </summary>
        private IDictionary<SceneCondition, IComponentInitialiser> _componentInitialiser =
            new Dictionary<SceneCondition, IComponentInitialiser>();

        /// <summary>
        /// シーンごとにプリロードされるシーン名
        /// </summary>
        private IDictionary<SceneCondition, IList<SceneCondition>> _preloadSceneConditions =
            new Dictionary<SceneCondition, IList<SceneCondition>>();

        /// <summary>
        /// プリロードされたシーン情報
        /// </summary>
        private IDictionary<SceneCondition, SceneData> _preloadedCaches =
            new Dictionary<SceneCondition, SceneData>();

        /// <summary>
        /// シーンをプリロードしたシーンのコンポーネント管理クラス
        /// </summary>
        private IDictionary<SceneCondition, HashSet<IComponentManager>> _preloaders =
            new Dictionary<SceneCondition, HashSet<IComponentManager>>();

        /// <summary>
        /// 使われたシーン情報の履歴
        /// </summary>
        private LinkedList<SceneData> _historySceneData =
            new LinkedList<SceneData>();

        /// <summary>
        /// シーン情報の履歴で最新のもの
        /// </summary>
        public SceneData LastHistorySceneData
        {
            get
            {
                if (_historySceneData.Last != null)
                {
                    return _historySceneData.Last.Value;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 現在使われているシーン情報
        /// </summary>
        private SceneData _currentSceneData = null;

        /// <summary>
        /// 現在使われているシーン情報
        /// </summary>
        public SceneData CurrentSceneData
        {
            get
            {
                return _currentSceneData;
            }
        }

        /// <summary>
        /// シーンで使うオブジェクトの管理クラスを初期化する
        /// </summary>
        /// <param name="game"></param>
        public SceneObjectManager(Game game)
        {
            Game = game;
        }

        /// <summary>
        /// シーンのコンテントマネージャー名をセットする
        /// </summary>
        /// <param name="sceneCondition"></param>
        /// <param name="contentManagerName"></param>
        public void SetSceneContentManagerName(SceneCondition sceneCondition, string contentManagerName)
        {
            _contentManagerNames[sceneCondition] = contentManagerName;
        }

        /// <summary>
        /// シーンのコンポーネント初期化クラスを登録する
        /// </summary>
        /// <param name="sceneCondition"></param>
        /// <param name="componentInitialiser"></param>
        public void SetSceneInitialiser(SceneCondition sceneCondition, IComponentInitialiser componentInitialiser)
        {
            _componentInitialiser[sceneCondition] = componentInitialiser;
        }

        /// <summary>
        /// シーンが読み込まれたとき、他にプリロードするシーン名を指定する
        /// </summary>
        /// <param name="sceneCondition"></param>
        /// <param name="preloadList"></param>
        public void SetPreloadSceneConditions(SceneCondition sceneCondition, IList<SceneCondition> preloadList)
        {
            _preloadSceneConditions[sceneCondition] = preloadList;
        }

        /// <summary>
        /// 次のシーン情報を新しく取得する
        /// すでにプリロードされている場合は、それが取得される
        /// </summary>
        /// <param name="nextSceneCondition"></param>
        /// <param name="clearHistory"></param>
        /// <returns></returns>
        public SceneData NextNewScene(SceneCondition nextSceneCondition, bool clearHistory)
        {
            // 次のシーンデータ取得
            SceneData nextSceneData = PopPreloadedCache(nextSceneCondition);
            if (nextSceneData == null)
            {
                nextSceneData = GetNewSceneData(nextSceneCondition);
                nextSceneData.ComponentManager.Load();
            }

            // 履歴追加
            if (CurrentSceneData != null)
            {
                _historySceneData.AddLast(CurrentSceneData);
            }

            // 現在のシーンデータにセット
            _currentSceneData = nextSceneData;

            // 他のシーンデータをプリロードする
            PreloadSceneData(nextSceneData);

            // 履歴をすべて削除する
            if (clearHistory)
            {
                ClearAllHistory(true);
            }

            return CurrentSceneData;
        }

        /// <summary>
        /// キャッシュからプリロードされているシーン情報を取り出す（キャッシュは削除される）
        /// </summary>
        /// <param name="sceneCondition"></param>
        /// <returns></returns>
        private SceneData PopPreloadedCache(SceneCondition sceneCondition)
        {
            if (_preloadedCaches.ContainsKey(sceneCondition))
            {
                SceneData preloaded = _preloadedCaches[sceneCondition];

                _preloadedCaches.Remove(sceneCondition);

                _preloaders.Remove(sceneCondition);

                return preloaded;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 新しいシーン情報を取得
        /// </summary>
        /// <param name="sceneCondition"></param>
        /// <returns></returns>
        private SceneData GetNewSceneData(SceneCondition sceneCondition)
        {
            IComponentInitialiser componentInitialiser = GetComponentInitialiser(sceneCondition);

            // コンポーネント初期化クラスがnullだとコンポーネントが作れないのでエラー
            if (componentInitialiser == null)
            {
                throw new InvalidOperationException(sceneCondition.ToString() + "のコンポーネント初期化クラスを指定してください");
            }

            string contentManagerName = GetContentManagerName(sceneCondition);

            // コンテントマネージャー名がnullだとコンテントマネージャーが作れないのでエラー
            if (contentManagerName == null)
            {
                throw new InvalidOperationException(sceneCondition.ToString() + "のコンテントマネージャー名を指定してください");
            }

            ContentManager contentManager = GetContentManager(contentManagerName);

            SceneData newSceneData =
                new SceneData(
                    sceneCondition,
                    new ComponentManager(Game, componentInitialiser, contentManager)
                    );

            SetContentManagerUser(contentManagerName, newSceneData.ComponentManager);

            return newSceneData;
        }

        /// <summary>
        /// コンポーネント初期化クラスを取得
        /// </summary>
        /// <param name="sceneCondition"></param>
        /// <returns></returns>
        private IComponentInitialiser GetComponentInitialiser(SceneCondition sceneCondition)
        {
            if (_componentInitialiser.ContainsKey(sceneCondition))
            {
                return _componentInitialiser[sceneCondition];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// シーンで使うコンテントマネージャーを取得する。
        /// すでに生成されている場合はそれを使い
        /// 生成されていない場合は新しく生成する。
        /// </summary>
        /// <param name="contentManagerName"></param>
        /// <returns></returns>
        private ContentManager GetContentManager(string contentManagerName)
        {
            if (contentManagerName != null)
            {
                if (!_contentManagers.ContainsKey(contentManagerName))
                {
                    _contentManagers[contentManagerName] = new ContentManager(Game.Services, "Content");
                }

                return _contentManagers[contentManagerName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// シーンで使うコンテントマネージャー名を取得する
        /// </summary>
        /// <param name="sceneCondition"></param>
        /// <returns></returns>
        private string GetContentManagerName(SceneCondition sceneCondition)
        {
            if (_contentManagerNames.ContainsKey(sceneCondition))
            {
                return _contentManagerNames[sceneCondition];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// あるコンテントマネージャーを使用するコンポーネントが所属するコンポーネントマネージャーを登録する
        /// </summary>
        /// <param name="contentManager"></param>
        /// <param name="user"></param>
        private void SetContentManagerUser(string contentManager, IComponentManager user)
        {
            if (!_contentManagerUsers.ContainsKey(contentManager))
            {
                _contentManagerUsers[contentManager] = new HashSet<IComponentManager>();
            }

            _contentManagerUsers[contentManager].Add(user);      
        }

        /// <summary>
        /// シーン情報をプリロードする
        /// </summary>
        /// <param name="preloaderSceneData"></param>
        private void PreloadSceneData(SceneData preloaderSceneData)
        {
            IList<SceneCondition> preloadList = GetPreloadList(preloaderSceneData.SceneCondition);

            if (preloadList != null)
            {
                foreach (SceneCondition preloadScene in preloadList)
                {
                    if (!_preloadedCaches.ContainsKey(preloadScene))
                    {
                        SceneData preloadComponentManager = GetNewSceneData(preloadScene);
                        preloadComponentManager.ComponentManager.Load();
                        preloadComponentManager.ComponentManager.Enabled = false;
                        preloadComponentManager.ComponentManager.Visible = false;

                        _preloadedCaches[preloadScene] = preloadComponentManager;
                    }
                    SetPreloader(preloadScene, preloaderSceneData.ComponentManager);
                }
            }
        }

        /// <summary>
        /// プリロードするシーン名リストを取得する
        /// </summary>
        /// <param name="sceneCondition"></param>
        /// <returns></returns>
        private IList<SceneCondition> GetPreloadList(SceneCondition sceneCondition)
        {
            if (_preloadSceneConditions.ContainsKey(sceneCondition))
            {
                return _preloadSceneConditions[sceneCondition];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// シーンをプリロードしたシーンのコンポーネント管理クラスを登録する
        /// </summary>
        /// <param name="preloadSceneCondition"></param>
        /// <param name="preloader"></param>
        private void SetPreloader(SceneCondition preloadSceneCondition, IComponentManager preloader)
        {
            if (!_preloaders.ContainsKey(preloadSceneCondition))
            {
                _preloaders[preloadSceneCondition] = new HashSet<IComponentManager>();
            }

            _preloaders[preloadSceneCondition].Add(preloader);
        }

        /// <summary>
        /// 前のシーン情報を取得する。現在のシーン情報は破棄される
        /// 履歴がない場合取得できない
        /// </summary>
        /// <returns></returns>
        public SceneData PrevScene()
        {
            if (CurrentSceneData != null)
            {
                UnsetPreloader(CurrentSceneData.ComponentManager);

                UnsetContentManagerUser(CurrentSceneData.ComponentManager);
                CurrentSceneData.ComponentManager.Unload();

                _currentSceneData = LastHistorySceneData;
                if (_currentSceneData != null)
                {
                    _historySceneData.RemoveLast();

                    PreloadSceneData(CurrentSceneData);
                }

                DumpGarbage();                
            }

            return CurrentSceneData;         
        }

        /// <summary>
        /// 不要になったオブジェクトを破棄する
        /// </summary>
        private void DumpGarbage()
        {
            DumpGarbagePreloadCache();
            DumpGarbageContentManager();
        }

        /// <summary>
        /// 指定したコンポーネント管理クラスのシーンがプリロードしたシーンの使用をやめる
        /// </summary>
        /// <param name="releasePreloader"></param>
        private void UnsetPreloader(IComponentManager releasePreloader)
        {
            foreach (KeyValuePair<SceneCondition, HashSet<IComponentManager>> preloader in _preloaders)
            {
                preloader.Value.Remove(releasePreloader);
            }
        }

        /// <summary>
        /// 指定したコンポーネント管理クラスのコンポーネントが使用している
        /// コンテントマネージャーの使用をやめる
        /// </summary>
        /// <param name="releaseUser"></param>
        private void UnsetContentManagerUser(IComponentManager releaseUser)
        {
            foreach (KeyValuePair<string, HashSet<IComponentManager>> user in _contentManagerUsers)
            {
                user.Value.Remove(releaseUser);
            }
        }

        /// <summary>
        /// 不要になったプリロードされたシーンのコンポーネント管理クラスを破棄する
        /// </summary>
        private void DumpGarbagePreloadCache()
        {
            var garbageCacheQuery = from preloader in _preloaders
                                    where preloader.Value != null && preloader.Value.Count == 0
                                    select preloader.Key;

            SceneCondition[] garbageCaches = garbageCacheQuery.ToArray();

            foreach (SceneCondition sceneCondition in garbageCaches)
            {
                if (_preloadedCaches.ContainsKey(sceneCondition))
                {
                    SceneData sceneData = _preloadedCaches[sceneCondition];

                    UnsetContentManagerUser(sceneData.ComponentManager);
                    sceneData.ComponentManager.Unload();                    
                    _preloadedCaches.Remove(sceneCondition);
                }

                _preloaders.Remove(sceneCondition);
            }
        }

        /// <summary>
        /// 不要になったコンテントマネージャーを破棄する
        /// </summary>
        private void DumpGarbageContentManager()
        {
            var garbageContentManagerQuery = from user in _contentManagerUsers
                                             where user.Value != null && user.Value.Count == 0
                                             select user.Key;

            string[] garbageContentManagers = garbageContentManagerQuery.ToArray();

            foreach (string contentManagerName in garbageContentManagers)
            {
                if (_contentManagers.ContainsKey(contentManagerName))
                {
                    _contentManagers[contentManagerName].Unload();
                    _contentManagers.Remove(contentManagerName);
                }

                _contentManagerUsers.Remove(contentManagerName);
            }

        }

        /// <summary>
        /// シーン情報のすべての履歴を削除する
        /// </summary>
        /// <param name="dump"></param>
        public void ClearAllHistory(bool dump)
        {
            for(;_historySceneData.Count > 0;)
            {
                RemoveHistoryFirst(false);                
            }

            if (dump)
            {
                DumpGarbage();
            }
        }

        /// <summary>
        /// シーン情報の最初の履歴を削除する
        /// </summary>
        /// <param name="dump"></param>
        private void RemoveHistoryFirst(bool dump)
        {
            LinkedListNode<SceneData> sceneDataNode = _historySceneData.First;
            if (sceneDataNode != null)
            {
                SceneData sceneData = sceneDataNode.Value;

                UnsetPreloader(sceneData.ComponentManager);

                UnsetContentManagerUser(sceneData.ComponentManager);
                sceneData.ComponentManager.Unload();

                _historySceneData.RemoveFirst();
            }

            if (dump)
            {
                DumpGarbage();
            }
        }

    }
}
