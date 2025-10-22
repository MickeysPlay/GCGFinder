using System;
using System.Collections.Generic;

namespace GameFramework.Event
{
    /// <summary>
    /// 事件管理器
    /// </summary>
    internal sealed class EventManager : GameFrameworkModule, IEventManager
    {
        private readonly Dictionary<int, EventHandler<GameEventArgs>> m_EventHandlers;
        private readonly Queue<Event> m_Events;

        private struct Event
        {
            public object Sender;
            public GameEventArgs EventArgs;

            public Event(object sender, GameEventArgs e)
            {
                Sender = sender;
                EventArgs = e;
            }
        }

        /// <summary>
        /// 初始化事件管理器
        /// </summary>
        public EventManager()
        {
            m_EventHandlers = new Dictionary<int, EventHandler<GameEventArgs>>();
            m_Events = new Queue<Event>();
        }

        /// <summary>
        /// 獲取遊戲框架模組優先級
        /// </summary>
        public override int Priority => 100;

        /// <summary>
        /// 獲取事件處理器數量
        /// </summary>
        public int EventHandlerCount => m_EventHandlers.Count;

        /// <summary>
        /// 獲取事件數量
        /// </summary>
        public int EventCount => m_Events.Count;

        /// <summary>
        /// 事件管理器輪詢
        /// </summary>
        /// <param name="elapseSeconds">邏輯流逝時間</param>
        /// <param name="realElapseSeconds">真實流逝時間</param>
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            while (m_Events.Count > 0)
            {
                Event e = m_Events.Dequeue();
                HandleEvent(e.Sender, e.EventArgs);
            }
        }

        /// <summary>
        /// 關閉並清理事件管理器
        /// </summary>
        public override void Shutdown()
        {
            m_Events.Clear();
            m_EventHandlers.Clear();
        }

        /// <summary>
        /// 訂閱事件
        /// </summary>
        /// <param name="id">事件 ID</param>
        /// <param name="handler">事件處理函數</param>
        public void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (handler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            if (!m_EventHandlers.ContainsKey(id))
            {
                m_EventHandlers[id] = handler;
            }
            else
            {
                m_EventHandlers[id] += handler;
            }
        }

        /// <summary>
        /// 取消訂閱事件
        /// </summary>
        /// <param name="id">事件 ID</param>
        /// <param name="handler">事件處理函數</param>
        public void Unsubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (handler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            if (m_EventHandlers.ContainsKey(id))
            {
                m_EventHandlers[id] -= handler;
            }
        }

        /// <summary>
        /// 觸發事件（延遲模式，下一幀處理）
        /// </summary>
        /// <param name="sender">事件發送者</param>
        /// <param name="e">事件參數</param>
        public void Fire(object sender, GameEventArgs e)
        {
            if (e == null)
            {
                throw new Exception("Event is invalid.");
            }

            m_Events.Enqueue(new Event(sender, e));
        }

        /// <summary>
        /// 觸發事件（立即模式）
        /// </summary>
        /// <param name="sender">事件發送者</param>
        /// <param name="e">事件參數</param>
        public void FireNow(object sender, GameEventArgs e)
        {
            if (e == null)
            {
                throw new Exception("Event is invalid.");
            }

            HandleEvent(sender, e);
        }

        private void HandleEvent(object sender, GameEventArgs e)
        {
            if (m_EventHandlers.TryGetValue(e.Id, out EventHandler<GameEventArgs> handlers))
            {
                handlers?.Invoke(sender, e);
            }
        }
    }
}
