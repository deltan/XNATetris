using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace deltan.XNALibrary.Control.Scene
{
    /// <summary>
    /// 更新順位の設定方法
    /// </summary>
    public enum UpdateOrderAssignment
    {
        INCREMENT_FROM_BASE_VALUE,      // ベース値からのインクリメント
        INCREMENT_FROM_CURRENT_SCENE,   // 現在のシーンの最大値からのインクリメント
        ADD_BASE_VALUE,                 // コンポーネントの各値にベース値を加算する
        ADD_CURRENT_SCENE,              // コンポーネントの各値に現在のシーンの最大値を加算する
    }

    /// <summary>
    /// 描画順位の設定方法
    /// </summary>
    public enum DrawOrderAssignment
    {
        INCREMENT_FROM_BASE_VALUE,      // ベース値からのインクリメント
        INCREMENT_FROM_CURRENT_SCENE,   // 現在のシーンの最大値からのインクリメント
        ADD_BASE_VALUE,                 // コンポーネントの各値にベース値を加算する
        ADD_CURRENT_SCENE,              // コンポーネントの各値に現在のシーンの最大値を加算する
    }

    /// <summary>
    /// シーン遷移情報
    /// </summary>
    public class TransitionInfo
    {
        /// <summary>
        /// シーン遷移後に、現在のシーンの更新処理が実行されるか
        /// </summary>
        public bool CurrentSceneEnabled { get; set; }

        /// <summary>
        /// シーン遷移後に、現在のシーンの描画処理が実行されるか
        /// </summary>
        public bool CurrentSceneVisible { get; set; }

        /// <summary>
        /// 新しいシーンのコンポーネントの更新順位の設定方法
        /// </summary>
        public UpdateOrderAssignment NewUpdateOrderAssignment { get; set; }

        /// <summary>
        /// 新しいシーンのコンポーネントの描画順位の設定方法
        /// </summary>
        public DrawOrderAssignment NewDrawOrderAssignment { get; set; }

        /// <summary>
        /// 新しいシーンのコンポーネントの更新順位設定のベース値
        /// </summary>
        public int NewBaseUpdateOrder { get; set; }

        /// <summary>
        /// 新しいシーンのコンポーネントの描画順位設定のベース値
        /// </summary>
        public int NewBaseDrawOrder { get; set; }

        /// <summary>
        /// シーン遷移後に戻れるか
        /// </summary>
        public bool Backable { get; set; }

        /// <summary>
        /// シーン遷移情報を作成する
        /// </summary>
        public TransitionInfo()
        {
            CurrentSceneEnabled = false;
            CurrentSceneVisible = false;
            NewUpdateOrderAssignment = UpdateOrderAssignment.INCREMENT_FROM_BASE_VALUE;
            NewDrawOrderAssignment = DrawOrderAssignment.INCREMENT_FROM_BASE_VALUE;
            NewBaseUpdateOrder = 0;
            NewBaseDrawOrder = 0;
            Backable = false;
        }
    }
}
