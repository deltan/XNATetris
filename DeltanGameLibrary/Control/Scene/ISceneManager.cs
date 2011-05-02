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
    /// シーン管理インターフェース
    /// </summary>
    public interface ISceneManager
    {
        /// <summary>
        /// シーン遷移
        /// ・現在のすべてのシーンの更新・描画の無効化
        /// ・指定したシーンの初期化・更新・描画の有効化
        /// </summary>
        /// <param name="nextSceneName"></param>
        /// <param name="attribute"></param>
        void NewScene(SceneCondition nextSceneCondition, TransitionInfo transitionInfo);
        void NewScene(SceneCondition nextSceneCondition);

        /// <summary>
        /// シーン遷移（バックシーン）
        /// ・現在のシーンの描画・更新の無効化
        /// ・前のシーンの初期化・描画・更新の有効化
        /// </summary>
        void BackScene();
    }
}
