using UnityEngine;

namespace GameFramework.UI
{
    /// <summary>
    /// UI 表單基類
    /// </summary>
    public abstract class UIFormBase : MonoBehaviour
    {
        private bool m_IsInit = false;
        private bool m_IsOpen = false;

        /// <summary>
        /// 獲取 UI 表單是否已初始化
        /// </summary>
        public bool IsInit => m_IsInit;

        /// <summary>
        /// 獲取 UI 表單是否已打開
        /// </summary>
        public bool IsOpen => m_IsOpen;

        /// <summary>
        /// UI 表單初始化（僅調用一次）
        /// </summary>
        public void Init()
        {
            if (m_IsInit)
            {
                return;
            }

            OnInit();
            m_IsInit = true;
        }

        /// <summary>
        /// 打開 UI 表單
        /// </summary>
        /// <param name="userData">用戶自定義數據</param>
        public void Open(object userData = null)
        {
            if (!m_IsInit)
            {
                Init();
            }

            gameObject.SetActive(true);
            m_IsOpen = true;
            OnOpen(userData);
        }

        /// <summary>
        /// 關閉 UI 表單
        /// </summary>
        public void Close()
        {
            if (!m_IsOpen)
            {
                return;
            }

            OnClose();
            m_IsOpen = false;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 暫停 UI 表單
        /// </summary>
        public void Pause()
        {
            OnPause();
        }

        /// <summary>
        /// 恢復 UI 表單
        /// </summary>
        public void Resume()
        {
            OnResume();
        }

        /// <summary>
        /// UI 表單初始化回調
        /// </summary>
        protected virtual void OnInit()
        {
        }

        /// <summary>
        /// 打開 UI 表單回調
        /// </summary>
        /// <param name="userData">用戶自定義數據</param>
        protected virtual void OnOpen(object userData)
        {
        }

        /// <summary>
        /// 關閉 UI 表單回調
        /// </summary>
        protected virtual void OnClose()
        {
        }

        /// <summary>
        /// 暫停 UI 表單回調
        /// </summary>
        protected virtual void OnPause()
        {
        }

        /// <summary>
        /// 恢復 UI 表單回調
        /// </summary>
        protected virtual void OnResume()
        {
        }
    }
}
