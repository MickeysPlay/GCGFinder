using UnityEngine;
using GameFramework;
using GameFramework.Event;
using GameFramework.ObjectPool;
using GameFramework.UI;

namespace GameMain.UI
{
    /// <summary>
    /// 主界面 UI 表單（使用範例）
    /// </summary>
    public class UIMainForm : UIFormBase
    {
        private IEventManager m_EventManager;
        private IObjectPoolManager m_PoolManager;

        protected override void OnInit()
        {
            base.OnInit();

            Debug.Log("[UIMainForm] OnInit - 初始化主界面");

            // 獲取框架模組
            m_EventManager = GameFrameworkEntry.GetModule<IEventManager>();
            m_PoolManager = GameFrameworkEntry.GetModule<IObjectPoolManager>();

            // 訂閱事件
            m_EventManager.Subscribe(EventId.Card.OnCardDataLoaded, OnCardDataLoaded);
            m_EventManager.Subscribe(EventId.Card.OnFilterChanged, OnFilterChanged);

            // 創建對象池（範例：卡牌列表項目）
            // m_PoolManager.CreateObjectPool<UICardItem>(
            //     CreateCardItem,
            //     OnSpawnCardItem,
            //     OnUnspawnCardItem
            // );
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            Debug.Log("[UIMainForm] OnOpen - 打開主界面");

            // 載入卡牌資料
            // LoadCardData();
        }

        protected override void OnClose()
        {
            base.OnClose();

            Debug.Log("[UIMainForm] OnClose - 關閉主界面");
        }

        protected override void OnPause()
        {
            base.OnPause();

            Debug.Log("[UIMainForm] OnPause - 暫停主界面");
        }

        protected override void OnResume()
        {
            base.OnResume();

            Debug.Log("[UIMainForm] OnResume - 恢復主界面");
        }

        private void OnCardDataLoaded(object sender, GameEventArgs e)
        {
            Card.CardDataLoadedEventArgs args = e as Card.CardDataLoadedEventArgs;
            if (args == null)
            {
                return;
            }

            Debug.Log($"[UIMainForm] 卡牌資料載入完成，共 {args.Cards.Length} 張卡牌");

            // 顯示卡牌列表
            // DisplayCardList(args.Cards);
        }

        private void OnFilterChanged(object sender, GameEventArgs e)
        {
            Card.FilterChangedEventArgs args = e as Card.FilterChangedEventArgs;
            if (args == null)
            {
                return;
            }

            Debug.Log($"[UIMainForm] 篩選條件改變：{args.FilterType} = {args.FilterValue}");

            // 重新篩選卡牌
            // FilterCards(args.FilterType, args.FilterValue);
        }

        private void OnDestroy()
        {
            // 取消訂閱事件
            if (m_EventManager != null)
            {
                m_EventManager.Unsubscribe(EventId.Card.OnCardDataLoaded, OnCardDataLoaded);
                m_EventManager.Unsubscribe(EventId.Card.OnFilterChanged, OnFilterChanged);
            }
        }
    }
}
