using System;
using System.Threading.Tasks;

namespace GameFramework.UI
{
    /// <summary>
    /// UI 管理器介面
    /// </summary>
    public interface IUIManager
    {
        /// <summary>
        /// 設置 UI 根節點
        /// </summary>
        /// <param name="uiRoot">UI 根節點 Transform</param>
        void SetUIRoot(UnityEngine.Transform uiRoot);

        /// <summary>
        /// 打開 UI 表單
        /// </summary>
        /// <typeparam name="T">UI 表單類型</typeparam>
        /// <param name="uiGroup">UI 組</param>
        /// <param name="userData">用戶自定義數據</param>
        /// <returns>UI 表單實例</returns>
        Task<T> OpenUIForm<T>(UIGroup uiGroup, object userData = null) where T : UIFormBase;

        /// <summary>
        /// 關閉 UI 表單
        /// </summary>
        /// <typeparam name="T">UI 表單類型</typeparam>
        void CloseUIForm<T>() where T : UIFormBase;

        /// <summary>
        /// 獲取 UI 表單
        /// </summary>
        /// <typeparam name="T">UI 表單類型</typeparam>
        /// <returns>UI 表單實例</returns>
        T GetUIForm<T>() where T : UIFormBase;

        /// <summary>
        /// 檢查 UI 表單是否已打開
        /// </summary>
        /// <typeparam name="T">UI 表單類型</typeparam>
        /// <returns>是否已打開</returns>
        bool IsUIFormOpen<T>() where T : UIFormBase;
    }
}
