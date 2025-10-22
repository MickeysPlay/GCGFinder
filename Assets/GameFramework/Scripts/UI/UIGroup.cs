namespace GameFramework.UI
{
    /// <summary>
    /// UI 組類型
    /// </summary>
    public enum UIGroup
    {
        /// <summary>
        /// 預設組（主界面、常駐 UI）
        /// </summary>
        Default = 0,

        /// <summary>
        /// 彈窗組（對話框、詳情頁）
        /// </summary>
        Popup = 100,

        /// <summary>
        /// 提示組（Toast、提示訊息）
        /// </summary>
        Tips = 200
    }
}
