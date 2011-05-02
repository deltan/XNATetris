using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace deltan.XNATetris.Model.Logic
{
    public interface ITetrisScoreCounter
    {
        /// <summary>
        /// 現在の点数
        /// </summary>
        int Score { get; set; }

        /// <summary>
        /// 点数をリセットする
        /// </summary>
        void Reset();

        /// <summary>
        /// 点数を計算する
        /// </summary>
        /// <param name="clearLines">消したライン</param>
        void Count(int clearLines);
    }
}
