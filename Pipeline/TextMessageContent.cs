using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content;

namespace Pipeline
{
    /// <summary>
    /// 処理されたテキストメッセージを保持するコンテントクラス
    /// </summary>
    // ランタイムの型をContentSerializerに知らせる
    [ContentSerializerRuntimeType("XNATetris.TextMessage, XNATetris")]
    public class TextMessageContent
    {
        /// <summary>
        /// スプライトフォントコンテント
        /// </summary>
        public SpriteFontContent Font;

        /// <summary>
        /// メッセージ配列
        /// </summary>
        public string[] Message;
    }
}
