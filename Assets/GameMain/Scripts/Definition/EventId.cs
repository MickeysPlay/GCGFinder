namespace GameMain
{
    /// <summary>
    /// 事件 ID 定義
    /// </summary>
    public static class EventId
    {
        /// <summary>
        /// 卡牌相關事件
        /// </summary>
        public static class Card
        {
            /// <summary>
            /// 卡牌資料載入完成
            /// </summary>
            public const int OnCardDataLoaded = 1001;

            /// <summary>
            /// 篩選條件改變
            /// </summary>
            public const int OnFilterChanged = 1002;

            /// <summary>
            /// 卡牌被選中
            /// </summary>
            public const int OnCardSelected = 1003;
        }

        /// <summary>
        /// UI 相關事件
        /// </summary>
        public static class UI
        {
            /// <summary>
            /// UI 打開完成
            /// </summary>
            public const int OnUIFormOpened = 2001;

            /// <summary>
            /// UI 關閉完成
            /// </summary>
            public const int OnUIFormClosed = 2002;
        }
    }
}
