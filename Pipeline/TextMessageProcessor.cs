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
    [ContentProcessor(DisplayName = "�e�L�X�g���b�Z�[�W�v���Z�b�T")]
    public class TextMessageProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {
            // context��ʂ���ExternalReference(�O���Q��)�̃A�Z�b�g���r���h�����ł���B
            // �ʏ�A�v���Z�b�T���w�肷�邪�A�����ł�null���w�肷�邱�ƂŁA
            // �C���|�[�g����̃^�C�v���擾���Ă���B
            FontDescription fontDescription =
                context.BuildAndLoadAsset<FontDescription, FontDescription>(input.FontDescription, null);
            string[] messageSource =
                context.BuildAndLoadAsset<string[], string[]>(input.Message, null);

            // FontDescription��Characters��messageSource�Ŏg�p���Ă��镶���R�[�h��ǉ�����B
            int totalCharacterCount = 0;
            foreach (string line in messageSource)
            {
                foreach (char c in line)
                {
                    totalCharacterCount++;

                    // FontDescription.Characters.Add���\�b�h����
                    // �����R�[�h�d���`�F�b�N�����Ă���̂�
                    // �����ł͏d�����C�ɂ����ɕ�����ǉ����邾���ł悢�B
                    fontDescription.Characters.Add(c);
                }
            }

            // �ŏI���ʂɕϊ�����
            TextMessageContent outContent = new TextMessageContent();

            // FontDescriptionProcessor�𒼐ڎ��s���āASpriteFontContent���擾����B
            FontDescriptionProcessor processor = new FontDescriptionProcessor();
            outContent.Font = processor.Process(fontDescription, context);

            // ���b�Z�[�W������̏����BDiscardMessage�t���O��True�̏ꍇ�͏������Ȃ��B
            if (!input.DiscardMessage)
                outContent.Message = ProcessMessage(messageSource);

            context.Logger.LogImportantMessage(
                String.Format("�g�p������{0}, ��������:{1}",
                fontDescription.Characters.Count, totalCharacterCount));

            return outContent;
        }

        /// <summary>
        /// �e�L�X�g���b�Z�[�W�̏���
        /// ���̃T���v���ł́A�P���Ɍ��̃e�L�X�g���󔒍s����؂�Ƃ���
        /// �����̃��b�Z�[�W�ɕϊ����Ă���B
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