using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace deltan.XNATetris.Model.Logic
{
    class SimpleTetrisScoreCounter : ITetrisScoreCounter
    {
        /// <summary>
        /// 現在の点数
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 点数をリセットする
        /// </summary>
        public void Reset()
        {
            Score = 0;
        }

        /// <summary>
        /// 点数を計算する
        /// </summary>
        /// <param name="clearLines"></param>
        public void Count(int clearLines)
        {
            switch (clearLines)
            {
                case 1:
                    Score += 10;
                    break;
                case 2:
                    Score += 30;
                    break;
                case 3:
                    Score += 100;
                    break;
                case 4:
                    Score += 400;
                    break;
            }
        }
    }
}
