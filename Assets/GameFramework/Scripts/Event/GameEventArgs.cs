using System;

namespace GameFramework.Event
{
    /// <summary>
    /// 遊戲邏輯事件基類
    /// </summary>
    public abstract class GameEventArgs : EventArgs
    {
        /// <summary>
        /// 獲取事件 ID
        /// </summary>
        public abstract int Id { get; }

        /// <summary>
        /// 清理事件參數
        /// </summary>
        public virtual void Clear()
        {
        }
    }
}
