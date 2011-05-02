using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNATetris
{
    /// <summary>
    /// テキストメッセージとフォントを保持するTextMessageクラス
    /// </summary>
    public class TextMessage
    {
        /// <summary>
        /// スプライトフォント
        /// </summary>
        public SpriteFont Font;

        /// <summary>
        /// テキストメッセージ
        /// </summary>
        public string[] Message;
    }
}
