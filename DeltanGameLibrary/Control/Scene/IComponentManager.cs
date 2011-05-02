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
    /// コンポーネント管理インターフェース
    /// </summary>
    public interface IComponentManager
    {
        /// <summary>
        /// Gameクラス
        /// </summary>
        Game Game { get; set; }

        /// <summary>
        /// 所属するコンポーネントが使用できるコンテントマネージャー
        /// </summary>
        ContentManager ContentManager { get; set; }

        /// <summary>
        /// コンポーネント初期化クラス
        /// </summary>
        IComponentInitialiser SceneInitialiser { get; set; }

        /// <summary>
        /// コンポーネントがGame.Componentsに登録されているか
        /// </summary>
        bool Loaded{ get; }

        /// <summary>
        /// コンポーネントの描画処理が実行されるか
        /// </summary>
        bool Visible{get;set;}

        /// <summary>
        /// コンポーネントの更新処理が実行されるか
        /// </summary>
        bool Enabled{get;set;}

        /// <summary>
        /// 所属するコンポーネントのなかで最大の更新順位
        /// </summary>
        int MaxUpdateOrder{get;}

        /// <summary>
        /// 所属するコンポーネントのなかで最小の更新順位
        /// </summary>
        int MinUpdateOrder{get;}

        /// <summary>
        /// 所属するコンポーネントのなかで最大の描画順位
        /// </summary>
        int MaxDrawOrder{get;}

        /// <summary>
        /// 所属するコンポーネントの中で最小の描画順位
        /// </summary>
        int MinDrawOrder{get;}

        /// <summary>
        /// コンポーネントを追加する
        /// Loadedがtrueの場合、Game.Componentsに追加で登録される
        /// Loadedがfalseの場合、RegistInitialiseComponentメソッドが実行されるまで登録されない
        /// </summary>
        /// <param name="component"></param>
        void AddComponent(IGameComponent component);

        /// <summary>
        /// コンポーネント初期化クラスを呼び出してから、
        /// 追加されたクラスをGame.Componentsに登録する
        /// </summary>
        void Load();

        /// <summary>
        /// 所属するコンポーネントの更新順位を追加した順にインクリメントで設定する
        /// </summary>
        /// <param name="beforeScene"></param>
        void IncUpdateOrder(IComponentManager beforeScene);

        /// <summary>
        /// 所属するコンポーネントの更新順位を追加した順にインクリメントで設定する
        /// </summary>
        /// <param name="baseUpdateOrder"></param>
        void IncUpdateOrder(int baseUpdateOrder);

        /// <summary>
        /// 所属するコンポーネントの描画順位を追加した順にインクリメントで設定する
        /// </summary>
        /// <param name="beforeScene"></param>
        void IncDrawOrder(IComponentManager beforeScene);

        /// <summary>
        /// 所属するコンポーネントの描画順位を追加した順にインクリメントで設定する
        /// </summary>
        /// <param name="baseDrawOrder"></param>
        void IncDrawOrder(int baseDrawOrder);

        /// <summary>
        /// 所属するコンポーネントの更新順位に加算する
        /// </summary>
        /// <param name="beforeScene"></param>
        void AddUpdateOrder(IComponentManager beforeScene);

        /// <summary>
        /// 所属するコンポーネントの更新順位に加算する
        /// </summary>
        /// <param name="beforeScene"></param>
        void AddUpdateOrder(int baseUpdateOrder);

        /// <summary>
        /// 所属するコンポーネントの描画順位に加算する
        /// </summary>
        /// <param name="beforeScene"></param>
        void AddDrawOrder(IComponentManager beforeScene);

        /// <summary>
        /// 所属するコンポーネントの描画順位に加算する
        /// </summary>
        /// <param name="beforeScene"></param>
        void AddDrawOrder(int baseDrawOrder);

        /// <summary>
        /// 所属するコンポーネントをGame.Componentsから解除し、すべて破棄する
        /// </summary>
        void Unload();
    }
}
