using System;
using UnityEngine.AddressableAssets;

namespace GameMain.Card
{
    /// <summary>
    /// 卡牌資料
    /// </summary>
    [Serializable]
    public class CardData
    {
        /// <summary>
        /// 卡牌 ID
        /// </summary>
        public string Id;

        /// <summary>
        /// 卡牌名稱
        /// </summary>
        public string Name;

        /// <summary>
        /// 卡牌類型 (UNIT/PILOT/COMMAND/BASE)
        /// </summary>
        public string Type;

        /// <summary>
        /// 色系
        /// </summary>
        public string Color;

        /// <summary>
        /// 稀有度 (C/R/R+/SR/LR)
        /// </summary>
        public string Rarity;

        /// <summary>
        /// 資源成本
        /// </summary>
        public int ResourceCost;

        /// <summary>
        /// 等級
        /// </summary>
        public int Level;

        /// <summary>
        /// 攻擊力
        /// </summary>
        public int Attack;

        /// <summary>
        /// 防禦力
        /// </summary>
        public int Defense;

        /// <summary>
        /// 效果描述
        /// </summary>
        public string Effect;

        /// <summary>
        /// 系列
        /// </summary>
        public string Series;

        /// <summary>
        /// 卡牌圖片資源引用（Addressables）
        /// </summary>
        public AssetReferenceSprite ImageReference;

        /// <summary>
        /// 卡牌圖片路徑（Resources 備用）
        /// </summary>
        public string ImagePath;
    }
}
