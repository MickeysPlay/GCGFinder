using System;

namespace GameFramework.Event
{
    /// <summary>
    /// 事件管理器介面
    /// </summary>
    public interface IEventManager
    {
        /// <summary>
        /// 獲取事件處理器數量
        /// </summary>
        int EventHandlerCount { get; }

        /// <summary>
        /// 獲取事件數量
        /// </summary>
        int EventCount { get; }

        /// <summary>
        /// 訂閱事件
        /// </summary>
        /// <param name="id">事件 ID</param>
        /// <param name="handler">事件處理函數</param>
        void Subscribe(int id, EventHandler<GameEventArgs> handler);

        /// <summary>
        /// 取消訂閱事件
        /// </summary>
        /// <param name="id">事件 ID</param>
        /// <param name="handler">事件處理函數</param>
        void Unsubscribe(int id, EventHandler<GameEventArgs> handler);

        /// <summary>
        /// 觸發事件（立即模式）
        /// </summary>
        /// <param name="sender">事件發送者</param>
        /// <param name="e">事件參數</param>
        void Fire(object sender, GameEventArgs e);

        /// <summary>
        /// 觸發事件（延遲模式，下一幀處理）
        /// </summary>
        /// <param name="sender">事件發送者</param>
        /// <param name="e">事件參數</param>
        void FireNow(object sender, GameEventArgs e);
    }
}
