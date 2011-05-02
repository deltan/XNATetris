using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace deltan.XNALibrary.Control.Services
{
    /// <summary>
    /// キーボードサービス
    /// </summary>
    public class KeyboardService
    {
        /// <summary>
        /// 最初にRepeatフラグがtrueになってから、2回目にRepeatフラグがtrueになるまでの時間（フレーム数）
        /// </summary>
        public IDictionary<Keys, int> RepeatLatencyFrame { get; set; }
        
        /// <summary>
        /// 2回目にRepeatフラグがtrueになった後、3回目以降にRepeatフラグがtrueになる間隔（フレーム数）
        /// </summary>
        public IDictionary<Keys, int> RepeatIntervalFrame { get; set; }
        
        /// <summary>
        /// 前回のキーの状態 
        /// </summary>
        private KeyboardState _lastKeyboardState;

        /// <summary>
        /// 今回のキーの状態
        /// </summary>
        private KeyboardState _currentKeyboardState;

        /// <summary>
        /// 各キーのDownフラグ
        /// trueならそのフレームでキーが押された
        /// </summary>
        private IDictionary<Keys, bool> _isDown = new Dictionary<Keys, bool>();

        /// <summary>
        /// 各キーのUpフラグ
        /// trueならそのフレームでキーが離された
        /// </summary>
        private IDictionary<Keys, bool> _isUp = new Dictionary<Keys, bool>();

        /// <summary>
        /// 各キーのKeepフラグ
        /// trueなら前回のフレーム以前からキーが押されている
        /// </summary>
        private IDictionary<Keys, bool> _isKeep = new Dictionary<Keys, bool>();

        /// <summary>
        /// 各キーのRepeatフラグ。
        /// trueなら前回のフレーム以前からキーが押されている。
        /// このフラグはRepeatLatencyFrameプロパティとRepeatIntervalFrameプロパティで設定された
        /// 時間ごとにtrueになり、処理を行うべきタイミングを表している。
        /// </summary>
        private IDictionary<Keys, bool> _isRepeat = new Dictionary<Keys, bool>();

        /// <summary>
        /// 2回目にRepeatフラグを立てるまでの待ち時間用のカウンタ
        /// </summary>
        private IDictionary<Keys, int> _repeatLatencyFrameCount = new Dictionary<Keys, int>();

        /// <summary>
        /// 3回目以降にRepeatフラグを立てる間隔用のカウンタ
        /// </summary>
        private IDictionary<Keys, int> _repeatIntervalFrameCount = new Dictionary<Keys, int>();
        

        /// <summary>
        /// プレイヤー番号
        /// </summary>
        private PlayerIndex _playerIndex;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="playerIndex"></param>
        public KeyboardService(PlayerIndex playerIndex)
        {
            _playerIndex = playerIndex;
        }

        /// <summary>
        /// このフレームのキー入力を開始します
        /// </summary>
        public void Begin()
        {
            _currentKeyboardState = Keyboard.GetState(_playerIndex);

            // すべてのキーについてキーの変化をチェックする
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                CheckKeyChange(key);
            }
        }

        /// <summary>
        /// キーの変化をチェックします
        /// </summary>
        private void CheckKeyChange(Keys key)
        {
            if (_lastKeyboardState[key] == KeyState.Up && _currentKeyboardState[key] == KeyState.Down)
            {
                UpToDown(key);
            }
            else if (_lastKeyboardState[key] == KeyState.Down && _currentKeyboardState[key] == KeyState.Up)
            {
                DownToUp(key);
            }
            else if (_lastKeyboardState[key] == KeyState.Down && _currentKeyboardState[key] == KeyState.Down)
            {
                DownKeep(key);
            }
            else if (_lastKeyboardState[key] == KeyState.Up && _currentKeyboardState[key] == KeyState.Up)
            {
                UpKeep(key);
            }
        }

        /// <summary>
        /// キーがUpからDownになったときの処理
        /// </summary>
        /// <param name="key"></param>
        private void UpToDown(Keys key)
        {
            _isDown[key] = true;
            _isUp[key] = false;
            _isKeep[key] = true;
            _isRepeat[key] = true;

            _repeatIntervalFrameCount[key] = 1;
            _repeatLatencyFrameCount[key] = 1;
        }

        /// <summary>
        /// キーがDownからUpになったときの処理
        /// </summary>
        /// <param name="key"></param>
        private void DownToUp(Keys key)
        {
            _isDown[key] = false;
            _isUp[key] = true;
            _isKeep[key] = false;
            _isRepeat[key] = false;

            _repeatIntervalFrameCount[key] = 1;
            _repeatLatencyFrameCount[key] = 1;
        }

        /// <summary>
        /// キーがDownのままのときの処理
        /// </summary>
        /// <param name="key"></param>
        private void DownKeep(Keys key)
        {
            _isDown[key] = false;
            _isUp[key] = false;
            _isKeep[key] = true;

            SetRepeat(key);
        }

        /// <summary>
        /// Repeatフラグをセットする。
        /// </summary>
        /// <param name="key"></param>
        private void SetRepeat(Keys key)
        {
            if (RepeatLatencyFrame != null && RepeatLatencyFrame.ContainsKey(key))
            {
                if (_repeatLatencyFrameCount[key] >= RepeatLatencyFrame[key])
                {
                    SetRepeatInterval(key);
                }
                else
                {
                    _repeatLatencyFrameCount[key]++;
                    _isRepeat[key] = false;
                }
            }
            else
            {
                SetRepeatInterval(key);
            }
        }

        /// <summary>
        /// 設定された間隔ごとにrepeatフラグを立てる
        /// </summary>
        /// <param name="key"></param>
        private void SetRepeatInterval(Keys key)
        {
            if (RepeatIntervalFrame != null && RepeatIntervalFrame.ContainsKey(key))
            {
                if (_repeatIntervalFrameCount[key] >= RepeatIntervalFrame[key])
                {
                    _isRepeat[key] = true;
                    _repeatIntervalFrameCount[key] = 1;
                }
                else
                {
                    _repeatIntervalFrameCount[key]++;
                    _isRepeat[key] = false;
                }
            }
            else
            {
                _isRepeat[key] = true;
                _repeatIntervalFrameCount[key] = 1;
            }
        }

        /// <summary>
        /// キーがUpのままのときの処理
        /// </summary>
        /// <param name="key"></param>
        private void UpKeep(Keys key)
        {
            _isDown[key] = false;
            _isUp[key] = false;
            _isKeep[key] = false;
            _isRepeat[key] = false;

            _repeatIntervalFrameCount[key] = 1;
            _repeatLatencyFrameCount[key] = 1;
        }

        /// <summary>
        /// このフレームのキー入力を終了します
        /// </summary>
        public void End()
        {
            _lastKeyboardState = _currentKeyboardState;
        }

        /// <summary>
        /// キーが押されたかを取得します。
        /// キーが押された1回だけtrueになり、
        /// 次にキーが離され押されるまでfalseになります。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyDown(Keys key)
        {
            if (_isDown.ContainsKey(key))
            {
                return _isDown[key];
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// キーが離されたかを取得します。
        /// キーが離された1回だけtrueになり、
        /// 次にキーが押され離されるまでfalseになります。
        /// </summary>
        /// <param name="key"></param>
        /// <returns>離されたときに1回trueになる</returns>
        public bool IsKeyUp(Keys key)
        {
            if (_isUp.ContainsKey(key))
            {
                return _isUp[key];
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// キーが押され続けているかを取得します。
        /// キーが押され続けていればtrueになり、離されればfalseになります。
        /// </summary>
        /// <param name="key"></param>
        /// <returns>押されている間はずっとtrueになる</returns>
        public bool IsKeyKeep(Keys key)
        {
            if (_isKeep.ContainsKey(key))
            {
                return _isKeep[key];
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// キーが押され続けていて、設定した時間が経過したかを取得します。
        /// 設定した時間が経過すればtrueになり、
        /// キーが押されていないか、設定した時間が経過していなければfalseになります。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsKeyRepeat(Keys key)
        {
            if (_isRepeat.ContainsKey(key))
            {
                return _isRepeat[key];
            }
            else
            {
                return false;
            }
        }
    }
}
