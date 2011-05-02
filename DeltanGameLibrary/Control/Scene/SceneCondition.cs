using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace deltan.XNALibrary.Control.Scene
{
    /// <summary>
    /// シーン条件
    /// </summary>
    public class SceneCondition
    {
        /// <summary>
        /// シーン名
        /// </summary>
        public string SceneName { get; set; }

        /// <summary>
        /// シーン属性
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// シーン条件を作成する
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="attritbute"></param>
        public SceneCondition(string sceneName, string attritbute)
        {
            SceneName = sceneName;
            Attribute = attritbute;
        }

        /// <summary>
        /// ハッシュ値を取得する
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return SceneName.GetHashCode() ^ Attribute.GetHashCode();
        }

        /// <summary>
        /// 同じか
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            SceneCondition other = obj as SceneCondition;

            if (other == null)
            {
                return false;
            }

            return (this.SceneName == other.SceneName && this.Attribute == other.Attribute); 
        }

        public override string ToString()
        {
            string str = string.Format("シーン(シーン名:{0} 属性:{1})", SceneName, Attribute);

            return str;
        }
    }
}
