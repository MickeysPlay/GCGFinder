using UnityEngine;

namespace GCGFinder.UI
{
    /// <summary>
    /// GUNDAM CARD GAME 配色方案 (基於 RX-78-2 初代鋼彈)
    /// </summary>
    public static class GundamColorScheme
    {
        // ============ 主色系 ============

        /// <summary>背景 - 淡藍白</summary>
        public static readonly Color Background = HexToColor("#EBEEFF");

        /// <summary>主色 - 高達藍 (邊框、主要按鈕)</summary>
        public static readonly Color Primary = HexToColor("#2C52B3");

        /// <summary>強調 - 高達紅 (搜尋按鈕、重要操作)</summary>
        public static readonly Color Accent = HexToColor("#FB2F38");

        /// <summary>輔助 - 高達黃 (稀有度標示、提示)</summary>
        public static readonly Color Secondary = HexToColor("#FFF867");

        /// <summary>文字 - 黑色</summary>
        public static readonly Color TextPrimary = HexToColor("#000000");

        /// <summary>邊框 - 淡灰 (分隔線、次要邊框)</summary>
        public static readonly Color Border = HexToColor("#E8E8E8");


        // ============ 按鈕狀態 ============

        /// <summary>主按鈕 - 正常狀態 (高達藍)</summary>
        public static readonly Color ButtonPrimaryNormal = Primary;

        /// <summary>主按鈕 - 懸停狀態 (深藍)</summary>
        public static readonly Color ButtonPrimaryHover = HexToColor("#1E3A8A");

        /// <summary>主按鈕 - 按下狀態 (極深藍)</summary>
        public static readonly Color ButtonPrimaryPressed = HexToColor("#152A6B");

        /// <summary>強調按鈕 - 正常狀態 (高達紅)</summary>
        public static readonly Color ButtonAccentNormal = Accent;

        /// <summary>強調按鈕 - 懸停狀態 (深紅)</summary>
        public static readonly Color ButtonAccentHover = HexToColor("#DC1F27");

        /// <summary>強調按鈕 - 按下狀態 (極深紅)</summary>
        public static readonly Color ButtonAccentPressed = HexToColor("#B51820");


        // ============ 文字層級 ============

        /// <summary>標題文字 (黑色)</summary>
        public static readonly Color TextTitle = TextPrimary;

        /// <summary>內文文字 (黑色)</summary>
        public static readonly Color TextBody = TextPrimary;

        /// <summary>次要文字 (中灰)</summary>
        public static readonly Color TextSecondary = HexToColor("#6B7280");

        /// <summary>停用文字 (淺灰)</summary>
        public static readonly Color TextDisabled = HexToColor("#9CA3AF");

        /// <summary>提示文字 (極淺灰)</summary>
        public static readonly Color TextPlaceholder = HexToColor("#D1D5DB");


        // ============ 稀有度顏色 ============

        /// <summary>Common (灰色)</summary>
        public static readonly Color RarityCommon = HexToColor("#9E9E9E");

        /// <summary>Rare (藍色)</summary>
        public static readonly Color RarityRare = HexToColor("#2196F3");

        /// <summary>Rare+ (紫色)</summary>
        public static readonly Color RarityRarePlus = HexToColor("#9C27B0");

        /// <summary>Super Rare (金色)</summary>
        public static readonly Color RaritySR = HexToColor("#FFD700");

        /// <summary>Legend Rare (彩虹漸變起點 - 紅色)</summary>
        public static readonly Color RarityLR = HexToColor("#FF1744");


        // ============ 卡牌色系 ============

        /// <summary>紅色卡牌</summary>
        public static readonly Color CardRed = HexToColor("#FB2F38");

        /// <summary>藍色卡牌</summary>
        public static readonly Color CardBlue = HexToColor("#2C52B3");

        /// <summary>黃色卡牌</summary>
        public static readonly Color CardYellow = HexToColor("#FFF867");

        /// <summary>綠色卡牌</summary>
        public static readonly Color CardGreen = HexToColor("#4CAF50");

        /// <summary>紫色卡牌</summary>
        public static readonly Color CardPurple = HexToColor("#9C27B0");

        /// <summary>白色卡牌 (無色)</summary>
        public static readonly Color CardWhite = HexToColor("#EBEEFF");


        // ============ 狀態顏色 ============

        /// <summary>成功 (綠色)</summary>
        public static readonly Color StatusSuccess = HexToColor("#10B981");

        /// <summary>警告 (高達黃)</summary>
        public static readonly Color StatusWarning = Secondary;

        /// <summary>錯誤 (高達紅)</summary>
        public static readonly Color StatusError = Accent;

        /// <summary>資訊 (高達藍)</summary>
        public static readonly Color StatusInfo = Primary;


        // ============ 背景層級 ============

        /// <summary>主背景 (淡藍白)</summary>
        public static readonly Color BackgroundPrimary = Background;

        /// <summary>次要背景 (極淺灰)</summary>
        public static readonly Color BackgroundSecondary = HexToColor("#F9FAFB");

        /// <summary>卡片背景 (淡藍白)</summary>
        public static readonly Color BackgroundCard = HexToColor("#EBEEFF");

        /// <summary>遮罩背景 (半透明黑)</summary>
        public static readonly Color BackgroundOverlay = new Color(0, 0, 0, 0.5f);


        // ============ 工具方法 ============

        /// <summary>
        /// 將十六進制顏色碼轉換為 Unity Color
        /// </summary>
        public static Color HexToColor(string hex)
        {
            // 移除 # 符號
            hex = hex.Replace("#", "");

            // 解析 RGB
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            // 處理 alpha (如果提供)
            byte a = 255;
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return new Color32(r, g, b, a);
        }

        /// <summary>
        /// 根據稀有度獲取對應顏色
        /// </summary>
        public static Color GetRarityColor(string rarity)
        {
            return rarity switch
            {
                "C" => RarityCommon,
                "R" => RarityRare,
                "R+" => RarityRarePlus,
                "SR" => RaritySR,
                "LR" => RarityLR,
                _ => RarityCommon
            };
        }

        /// <summary>
        /// 根據卡牌色系獲取對應顏色
        /// </summary>
        public static Color GetCardColor(string color)
        {
            return color switch
            {
                "Red" => CardRed,
                "Blue" => CardBlue,
                "Yellow" => CardYellow,
                "Green" => CardGreen,
                "Purple" => CardPurple,
                "White" => CardWhite,
                _ => CardWhite
            };
        }
    }
}
