using GameFramework.Event;

namespace GameMain.Card
{
    /// <summary>
    /// 卡牌資料載入完成事件
    /// </summary>
    public class CardDataLoadedEventArgs : GameEventArgs
    {
        public override int Id => EventId.Card.OnCardDataLoaded;

        public CardData[] Cards { get; set; }

        public override void Clear()
        {
            Cards = null;
        }
    }

    /// <summary>
    /// 篩選條件改變事件
    /// </summary>
    public class FilterChangedEventArgs : GameEventArgs
    {
        public override int Id => EventId.Card.OnFilterChanged;

        public string FilterType { get; set; }
        public string FilterValue { get; set; }

        public override void Clear()
        {
            FilterType = null;
            FilterValue = null;
        }
    }

    /// <summary>
    /// 卡牌選中事件
    /// </summary>
    public class CardSelectedEventArgs : GameEventArgs
    {
        public override int Id => EventId.Card.OnCardSelected;

        public CardData SelectedCard { get; set; }

        public override void Clear()
        {
            SelectedCard = null;
        }
    }
}
