using UnityEngine;
using GameFramework;
using GameFramework.Event;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using GameFramework.UI;

namespace GameMain
{
    /// <summary>
    /// GameMain 組件
    /// 統一管理所有 GameFramework 組件
    /// </summary>
    public class GameMainComponent : MonoBehaviour
    {
        [Header("Framework Components")]
        [SerializeField] private BaseComponent m_BaseComponent = null;
        [SerializeField] private EventComponent m_EventComponent = null;
        [SerializeField] private ObjectPoolComponent m_ObjectPoolComponent = null;
        [SerializeField] private ResourceComponent m_ResourceComponent = null;
        [SerializeField] private UIComponent m_UIComponent = null;

        [Header("Test Settings")]
        [SerializeField] private bool m_OpenMainFormOnStart = true;

        private void Awake()
        {
            // 檢查必要組件
            if (m_BaseComponent == null)
            {
                Debug.LogError("BaseComponent is not assigned!");
            }

            Debug.Log("[GameMainComponent] 框架組件初始化完成");
        }

        private async void Start()
        {
            Debug.Log("[GameMainComponent] GameMain 啟動");

            // 測試：打開主界面
            if (m_OpenMainFormOnStart && m_UIComponent != null)
            {
                Debug.Log("[GameMainComponent] 正在打開主界面...");
                var mainForm = await m_UIComponent.OpenUIForm<UI.UIMainForm>(UIGroup.Default);
                if (mainForm != null)
                {
                    Debug.Log("[GameMainComponent] 主界面打開成功");
                }
                else
                {
                    Debug.LogWarning("[GameMainComponent] 主界面打開失敗");
                }
            }

            // 測試：事件系統
            TestEventSystem();
        }

        private void TestEventSystem()
        {
            if (m_EventComponent == null)
            {
                return;
            }

            Debug.Log("[GameMainComponent] 測試事件系統");

            // 訂閱事件
            m_EventComponent.Subscribe(EventId.Card.OnCardSelected, OnCardSelected);

            // 觸發事件
            var eventArgs = new Card.CardSelectedEventArgs
            {
                SelectedCard = new Card.CardData
                {
                    Id = "GD01-001",
                    Name = "測試卡牌"
                }
            };

            m_EventComponent.FireNow(this, eventArgs);

            // 取消訂閱
            m_EventComponent.Unsubscribe(EventId.Card.OnCardSelected, OnCardSelected);
        }

        private void OnCardSelected(object sender, GameFramework.Event.GameEventArgs e)
        {
            var args = e as Card.CardSelectedEventArgs;
            if (args == null)
            {
                return;
            }

            Debug.Log($"[GameMainComponent] 卡牌被選中：{args.SelectedCard.Name} (ID: {args.SelectedCard.Id})");
        }
    }
}
