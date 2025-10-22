using System.Threading.Tasks;
using UnityEngine;

namespace GameFramework.UI
{
    /// <summary>
    /// UI 組件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/UI")]
    public sealed class UIComponent : GameFrameworkComponent
    {
        [SerializeField] private Transform m_UIRoot = null;

        private IUIManager m_UIManager = null;

        protected override void Awake()
        {
            base.Awake();

            m_UIManager = GameFrameworkEntry.GetModule<IUIManager>();
            if (m_UIManager == null)
            {
                Debug.LogError("UI manager is invalid.");
                return;
            }

            // 設置 UI 根節點
            if (m_UIRoot == null)
            {
                m_UIRoot = transform;
            }

            m_UIManager.SetUIRoot(m_UIRoot);
        }

        /// <summary>
        /// 打開 UI 表單
        /// </summary>
        /// <typeparam name="T">UI 表單類型</typeparam>
        /// <param name="uiGroup">UI 組</param>
        /// <param name="userData">用戶自定義數據</param>
        /// <returns>UI 表單實例</returns>
        public async Task<T> OpenUIForm<T>(UIGroup uiGroup, object userData = null) where T : UIFormBase
        {
            return await m_UIManager.OpenUIForm<T>(uiGroup, userData);
        }

        /// <summary>
        /// 關閉 UI 表單
        /// </summary>
        /// <typeparam name="T">UI 表單類型</typeparam>
        public void CloseUIForm<T>() where T : UIFormBase
        {
            m_UIManager.CloseUIForm<T>();
        }

        /// <summary>
        /// 獲取 UI 表單
        /// </summary>
        /// <typeparam name="T">UI 表單類型</typeparam>
        /// <returns>UI 表單實例</returns>
        public T GetUIForm<T>() where T : UIFormBase
        {
            return m_UIManager.GetUIForm<T>();
        }

        /// <summary>
        /// 檢查 UI 表單是否已打開
        /// </summary>
        /// <typeparam name="T">UI 表單類型</typeparam>
        /// <returns>是否已打開</returns>
        public bool IsUIFormOpen<T>() where T : UIFormBase
        {
            return m_UIManager.IsUIFormOpen<T>();
        }
    }
}
