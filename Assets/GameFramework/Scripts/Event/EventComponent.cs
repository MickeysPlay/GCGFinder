using System;
using UnityEngine;

namespace GameFramework.Event
{
    /// <summary>
    /// 事件組件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Event")]
    public sealed class EventComponent : GameFrameworkComponent
    {
        private IEventManager m_EventManager = null;

        /// <summary>
        /// 獲取事件處理器數量
        /// </summary>
        public int EventHandlerCount
        {
            get
            {
                return m_EventManager.EventHandlerCount;
            }
        }

        /// <summary>
        /// 獲取事件數量
        /// </summary>
        public int EventCount
        {
            get
            {
                return m_EventManager.EventCount;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_EventManager = GameFrameworkEntry.GetModule<IEventManager>();
            if (m_EventManager == null)
            {
                Debug.LogError("Event manager is invalid.");
                return;
            }
        }

        /// <summary>
        /// 訂閱事件
        /// </summary>
        /// <param name="id">事件 ID</param>
        /// <param name="handler">事件處理函數</param>
        public void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            m_EventManager.Subscribe(id, handler);
        }

        /// <summary>
        /// 取消訂閱事件
        /// </summary>
        /// <param name="id">事件 ID</param>
        /// <param name="handler">事件處理函數</param>
        public void Unsubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            m_EventManager.Unsubscribe(id, handler);
        }

        /// <summary>
        /// 觸發事件（延遲模式，下一幀處理）
        /// </summary>
        /// <param name="sender">事件發送者</param>
        /// <param name="e">事件參數</param>
        public void Fire(object sender, GameEventArgs e)
        {
            m_EventManager.Fire(sender, e);
        }

        /// <summary>
        /// 觸發事件（立即模式）
        /// </summary>
        /// <param name="sender">事件發送者</param>
        /// <param name="e">事件參數</param>
        public void FireNow(object sender, GameEventArgs e)
        {
            m_EventManager.FireNow(sender, e);
        }
    }
}
