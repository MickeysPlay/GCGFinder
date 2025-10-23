using UnityEngine;
using GameFramework.Scene;
using GameFramework;

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

        private void Start()
        {
            // 目前直接跳轉到主場景
            // 未來可在此處加入 Preload 場景邏輯：
            // - 載入設定檔
            // - 檢查更新
            // - 預載 Addressable bundles
            LoadGameScene();
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

        private void LoadGameScene()
        {
            Debug.Log($"[GameEntry] 正在載入場景: {gameSceneName}");
            SceneLoader.LoadScene(gameSceneName);
        }
    }
}
