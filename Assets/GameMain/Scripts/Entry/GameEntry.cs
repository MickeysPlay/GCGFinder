using System;
using System.Threading.Tasks;
using UnityEngine;
using GameFramework.Scene;
using GameFramework;
using GameFramework.UI;
using GameMain.UI;

namespace GameMain
{
    /// <summary>
    /// 遊戲入口點
    /// 負責檢查必要元件並跳轉到主場景
    /// </summary>
    public class GameEntry : MonoBehaviour
    {
        [Header("場景設定")]
        [SerializeField] private string gameSceneName = "GameMain";

        [Header("必要元件檢查")]
        [SerializeField] private BaseComponent baseComponent;

        private void Awake()
        {

            // 檢查必要元件是否掛載
            if (!ValidateComponents())
            {
                Debug.LogError("[GameEntry] 必要元件未正確掛載，請檢查場景設定！");
                return;
            }

            // 設置為不銷毀
            DontDestroyOnLoad(gameObject);
        }

        private async void Start()
        {
            try
            {
                // 初始化 UI 系統
                InitializeUISystem();

                // 打開載入面板
                var loadingPanel = await OpenLoadingPanel();

                // 載入遊戲場景
                await LoadGameSceneAsync(loadingPanel);
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameEntry] 啟動失敗: {e.Message}");
                await ShowErrorMessage("啟動失敗", e.Message);
            }
        }

        /// <summary>
        /// 驗證所有必要元件是否已掛載
        /// </summary>
        private bool ValidateComponents()
        {
            bool isValid = true;

            if (baseComponent == null)
            {
                Debug.LogError("[GameEntry] BaseComponent 未掛載！");
                isValid = false;
            }

            // 未來可加入其他元件檢查，例如：
            // - EventComponent
            // - ResourceComponent
            // - UIComponent
            // - ObjectPoolComponent

            return isValid;
        }

        /// <summary>
        /// 初始化 UI 系統
        /// </summary>
        private void InitializeUISystem()
        {
            // 獲取 UI Root
            Transform uiRoot = null;
            // 嘗試查找場景中的 Canvas
            var canvas = FindFirstObjectByType<Canvas>();
            if (canvas != null)
            {
                uiRoot = canvas.transform;
            }

            if (uiRoot == null)
            {
                Debug.LogError("[GameEntry] 找不到 UI Canvas！");
                return;
            }

            // 設置 UIManager 的 UI Root
            var uiManager = GameFrameworkEntry.GetModule<IUIManager>();
            uiManager.SetUIRoot(uiRoot);

            Debug.Log("[GameEntry] UI 系統初始化完成");
        }

        /// <summary>
        /// 打開載入面板
        /// </summary>
        private async Task<LoadingPanel> OpenLoadingPanel()
        {
            var uiManager = GameFrameworkEntry.GetModule<IUIManager>();
            var loadingPanel = await uiManager.OpenUIForm<LoadingPanel>(UIGroup.Tips);

            if (loadingPanel != null)
            {
                loadingPanel.UpdateProgress("正在初始化...", 0.1f);
            }

            return loadingPanel;
        }

        /// <summary>
        /// 載入遊戲場景（異步）
        /// </summary>
        private async Task LoadGameSceneAsync(LoadingPanel loadingPanel)
        {
            Debug.Log($"[GameEntry] 正在載入場景: {gameSceneName}");

            if (loadingPanel != null)
            {
                loadingPanel.UpdateProgress("正在載入場景...", 0.5f);
            }

            // 模擬載入延遲
            await Task.Delay(500);

            // 載入場景
            SceneLoader.LoadScene(gameSceneName);

            if (loadingPanel != null)
            {
                loadingPanel.UpdateProgress("載入完成", 1.0f);
                await Task.Delay(300);

                // 關閉載入面板
                var uiManager = GameFrameworkEntry.GetModule<IUIManager>();
                uiManager.CloseUIForm<LoadingPanel>();
            }
        }

        /// <summary>
        /// 顯示錯誤訊息
        /// </summary>
        private async Task ShowErrorMessage(string title, string message)
        {
            try
            {
                var uiManager = GameFrameworkEntry.GetModule<IUIManager>();
                await uiManager.OpenUIForm<MessageBox>(UIGroup.Tips, new MessageBoxData
                {
                    Title = title,
                    Message = message,
                    Type = MessageBoxType.Error
                });
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameEntry] 無法顯示錯誤訊息: {e.Message}");
            }
        }
    }
}
