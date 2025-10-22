# GUNDAM CARD GAME 卡牌查詢應用 - 開發規劃

## 📋 項目概述

- **名稱**：GUNDAM CARD GAME 卡牌查詢應用
- **平台**：Unity (C#)
- **資料來源**：https://www.gundam-gcg.com/jp/cards/index.php
- **初期功能**：卡牌查詢與篩選

---

## 🎨 設計方案

### 配色方案（RX-78-2 初代鋼彈）

| 元素 | 顏色 | 十六進制 | 用途 |
|------|------|---------|------|
| 背景 | 純白 | #FFFFFF | 主背景 |
| 主色 | 高達藍 | #2C52B3 | 邊框、主要按鈕 |
| 強調 | 高達紅 | #FB2F38 | 搜尋按鈕、強調 |
| 輔助 | 高達黃 | #FFF867 | 稀有度標示 |
| 文字 | 黑色 | #000000 | 主要文字 |
| 邊框 | 淡灰 | #E8E8E8 | 分隔線、次要邊框 |

### 響應式佈局（側邊欄模式）

**電腦版** (寬屏)
- 左側：篩選面板固定（25%）
- 右側：搜尋結果主區（75%）

**手機版** (窄屏)
- 篩選隱藏（預設）
- 點擊 [篩選▼] 按鈕展開側欄
- 搜尋結果占滿螢幕

---

## 🔍 功能設計

### 頁面結構

```
┌─ 搜尋欄 ─┐
│ [搜尋...] [篩選▼] │  ← 頂部固定
├──────────┤
│          │
│ 【左】   │ 【右】
│ 篩選    │ 卡牌
│ 面板    │ 列表
│ (可隱藏)  │ (3列網格)
│          │
└──────────┘
```

### 篩選條件

| 類別 | 篩選項 | 類型 |
|------|--------|------|
| 基本 | 卡牌名稱 | 文字搜尋 |
| | 卡牌類型 | 下拉菜單 (UNIT/PILOT/COMMAND/BASE) |
| | 色系 | 下拉菜單 (紅/藍/黃/綠/紫) |
| | 系列 | 下拉菜單 |
| 戰鬥 | 稀有度 | 下拉菜單 (C/R/R+/SR/LR) |
| | 等級 | 下拉菜單 (1-5) |
| | 成本 | 下拉菜單 |
| | 攻擊力 | 下拉菜單 |
| | HP | 下拉菜單 |
| 其他 | 特徵 | 下拉菜單 |
| | 效果 | 下拉菜單 |

### 操作流程

1. 應用啟動 → 顯示首頁（篩選隱藏，顯示全部卡牌）
2. 使用者調整篩選條件（通過下拉菜單或搜尋欄）
3. 點擊 [搜尋] 按鈕 → 執行複合篩選
4. 顯示結果列表 → 點擊卡牌進入詳情頁
5. [重設] 按鈕清除所有篩選

---

## 📊 開發階段

### 第一階段：資料準備
- [ ] 分析官網結構
- [ ] 編寫爬蟲腳本 (Python)
- [ ] 抓取卡牌資料
- [ ] 轉換為 JSON 格式

### 第二階段：Unity 基礎
- [ ] 建立項目結構
- [ ] 匯入 TextMesh Pro 與字型
- [ ] 建立資料模型 (Card 類別)
- [ ] 實現資料載入

### 第三階段：核心邏輯
- [ ] 實現搜尋引擎 (LINQ)
- [ ] 複合篩選邏輯
- [ ] 排序功能

### 第四階段：UI 開發
- [ ] 搜尋欄實現
- [ ] 篩選面板 UI
- [ ] 卡牌列表顯示
- [ ] 詳情頁面
- [ ] 響應式佈局

### 第五階段：測試與優化
- [ ] 功能測試
- [ ] 效能優化
- [ ] UI 微調

---

## 💾 資料格式

### JSON 結構範例

```json
{
  "cards": [
    {
      "id": "GD01-024",
      "name": "Wing Gundam Zero",
      "type": "UNIT",
      "color": "White",
      "rarity": "LR",
      "resourceCost": 5,
      "level": 4,
      "attack": 5,
      "defense": 4,
      "effect": "效果描述...",
      "series": "Mobile Suit Gundam Wing",
      "imageUrl": "https://...",
      "tags": ["Transform", "Draw"]
    }
  ]
}
```

### Card 類別定義

```csharp
public class Card
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }           // UNIT/PILOT/COMMAND/BASE
    public string Color { get; set; }
    public string Rarity { get; set; }         // C/R/R+/SR/LR
    public int ResourceCost { get; set; }
    public int Level { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public string Effect { get; set; }
    public string Series { get; set; }
    public string ImageUrl { get; set; }
    public List<string> Tags { get; set; }
}
```

---

## 🛠️ 推薦使用資源

### Unity 內建
- UGUI (UI 系統)
- TextMesh Pro (字型渲染)
- Standard Assets (基礎組件)

### 免費外部
- TextMesh Pro 字型：Google Fonts (Roboto Condensed / Inter)
- 配色工具：Coolors.co

### 爬蟲工具
- Python + BeautifulSoup / Selenium
- 或 C# + HtmlAgilityPack

---

## 🎯 下一步

**立即開始：資料爬蟲**

1. 分析官網卡牌頁面結構
2. 編寫爬蟲腳本提取卡牌資料
3. 生成 JSON 檔案

需要我幫忙分析官網結構或編寫爬蟲代碼嗎？