using UnityEngine;
using UnityEngine.UI;
using GameFramework.UI;
using TMPro;

namespace GameMain.UI
{
    /// <summary>
    /// Loading 載入面板
    /// 用於場景切換和資料載入時顯示
    /// </summary>
    public class LoadingPanel : UIFormBase
    {
        [Header("UI 組件")]
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private Slider progressBar;

        protected override void OnInit()
        {
            base.OnInit();

            // 確保背景是全螢幕黑色
            if (background != null)
            {
                background.color = Color.black;
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            Debug.Log("[LoadingPanel] OnOpen - 顯示載入畫面");

            // 重置狀態
            SetLoadingText("Loading...");

            // 預設隱藏進度條（簡單的 Loading 不需要進度）
            HideProgressBar();
        }

        protected override void OnClose()
        {
            base.OnClose();

            Debug.Log("[LoadingPanel] OnClose - 關閉載入畫面");
        }

        /// <summary>
        /// 設置載入文字
        /// </summary>
        public void SetLoadingText(string text)
        {
            if (loadingText != null)
            {
                loadingText.text = text;
            }
        }

        /// <summary>
        /// 設置載入進度
        /// </summary>
        /// <param name="progress">進度值 (0~1)</param>
        public void SetProgress(float progress)
        {
            if (progressBar != null)
            {
                progressBar.gameObject.SetActive(true);
                progressBar.value = Mathf.Clamp01(progress);
            }
        }

        /// <summary>
        /// 隱藏進度條（只顯示文字）
        /// </summary>
        public void HideProgressBar()
        {
            if (progressBar != null)
            {
                progressBar.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 更新載入進度（文字 + 進度條）
        /// </summary>
        /// <param name="text">提示文字</param>
        /// <param name="progress">進度值 (0~1)</param>
        public void UpdateProgress(string text, float progress)
        {
            SetLoadingText(text);
            SetProgress(progress);
        }
    }
}
