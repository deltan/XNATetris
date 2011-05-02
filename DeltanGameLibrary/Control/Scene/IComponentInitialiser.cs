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
    /// コンポーネント初期化インターフェース
    /// </summary>
    public interface IComponentInitialiser
    {
        /// <summary>
        /// Gameクラス
        /// </summary>
        Game Game { get; set; }

        /// <summary>
        /// 渡されたコンポーネント管理クラスを初期化する
        /// </summary>
        /// <param name="componentManager"></param>
        /// <param name="contentManager"></param>
        void Initialise(IComponentManager componentManager, ContentManager contentManager);
    }
}
