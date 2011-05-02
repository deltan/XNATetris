using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

// TODO: replace these with the processor input and output types.
using TInput = Pipeline.TextMessageDescription;
using TOutput = Pipeline.TextMessageContent;

namespace Pipeline
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "テキストメッセージプロセッサ")]
    public class TextMessageProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            // contextを通してExternalReference(外部参照)のアセットをビルド処理できる。
            // 通常、プロセッサを指定するが、ここではnullを指定することで、
            // インポート直後のタイプを取得している。
            FontDescription fontDescription =
                context.BuildAndLoadAsset<FontDescription, FontDescription>(input.FontDescription, null);
            string[] messageSource =
                context.BuildAndLoadAsset<string[], string[]>(input.Message, null);

            // FontDescriptionのCharactersにmessageSourceで使用している文字コードを追加する。
            int totalCharacterCount = 0;
            foreach (string line in messageSource)
            {
                foreach (char c in line)
                {
                    totalCharacterCount++;

                    // FontDescription.Characters.Addメソッド内で
                    // 文字コード重複チェックをしているので
                    // ここでは重複を気にせずに文字を追加するだけでよい。
                    fontDescription.Characters.Add(c);
                }
            }

            // 最終結果に変換する
            TextMessageContent outContent = new TextMessageContent();

            // FontDescriptionProcessorを直接実行して、SpriteFontContentを取得する。
            FontDescriptionProcessor processor = new FontDescriptionProcessor();
            outContent.Font = processor.Process(fontDescription, context);

            // メッセージ文字列の処理。DiscardMessageフラグがTrueの場合は処理しない。
            if (!input.DiscardMessage)
                outContent.Message = ProcessMessage(messageSource);

            context.Logger.LogImportantMessage(
                String.Format("使用文字数{0}, 総文字数:{1}",
                fontDescription.Characters.Count, totalCharacterCount));

            return outContent;
        }

        /// <summary>
        /// テキストメッセージの処理
        /// このサンプルでは、単純に元のテキストを空白行を区切りとした
        /// 複数のメッセージに変換している。
        /// </summary>
        /// <param name="messageSource"></param>
        /// <returns></returns>
        string[] ProcessMessage(string[] messageSource)
        {
            List<string> messages = new List<string>();
            string curMessage = String.Empty;
            bool capturingMessage = false;

            foreach (string line in messageSource)
            {
                if (String.IsNullOrEmpty(line) == false)
                {
                    curMessage += line + "\n";
                    capturingMessage = true;
                }
                else
                {
                    if (capturingMessage)
                    {
                        messages.Add(curMessage);
                        curMessage = String.Empty;
                        capturingMessage = false;
                    }
                }
            }

            if (capturingMessage)
            {
                messages.Add(curMessage);
            }

            return messages.ToArray();
        }
    }
}