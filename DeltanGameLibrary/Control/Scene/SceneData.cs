using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace deltan.XNALibrary.Control.Scene
{
    /// <summary>
    /// シーン情報
    /// </summary>
    public class SceneData
    {
        /// <summary>
        /// シーン名
        /// </summary>
        public SceneCondition SceneCondition { get; set; }
        
        /// <summary>
        /// シーンで使用するコンポーネント管理クラス
        /// </summary>
        public IComponentManager ComponentManager { get; set; }

        /// <summary>
        /// シーン情報を作成する
        /// </summary>
        /// <param name="sceneCondition"></param>
        /// <param name="componentManager"></param>
        public SceneData(SceneCondition sceneCondition, IComponentManager componentManager)
        {
            SceneCondition = sceneCondition;
            ComponentManager = componentManager;
        }
    }
}
