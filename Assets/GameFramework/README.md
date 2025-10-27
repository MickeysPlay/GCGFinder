# GCGFinder GameFramework

基於 **GameFramework** 理念的簡化版遊戲框架，專為 GUNDAM CARD GAME 卡牌查詢應用設計。

---

## 📋 核心特性

- ✅ **模組化設計** - 解耦、可擴展
- ✅ **泛型支援** - 類型安全，避免字串硬編碼
- ✅ **事件驅動** - 模組間解耦通訊
- ✅ **對象池** - 優化性能
- ✅ **Addressables 整合** - 支援遠端資源更新

---

## 🏗️ 架構設計

```
GameFramework/               # 可復用框架層
├── Base/                   # 核心基類
│   ├── GameFrameworkEntry  # 框架入口
│   ├── GameFrameworkModule # 模組基類
│   └── BaseComponent       # 驅動組件
├── Event/                  # 事件系統
├── ObjectPool/             # 對象池
├── UI/                     # UI 管理
└── Resource/               # 資源管理

GameMain/                   # 業務邏輯層
├── Component/              # 整合組件
├── Card/                   # 卡牌邏輯
├── UI/                     # UI 實作
└── Definition/             # 常量定義
```

---

## 🚀 快速開始

### 1. 場景設置

1. 在場景中創建 GameObject `GameFramework`
2. 添加以下組件：
   - `BaseComponent`（必須，驅動框架）
   - `EventComponent`
   - `ObjectPoolComponent`
   - `ResourceComponent`
   - `UIComponent`

3. 創建 GameObject `GameMain`
4. 添加 `GameMainComponent`
5. 將上述組件拖拽到 GameMainComponent 的引用欄位

### 2. UI 設置

在 UIComponent 上設置 UI Root（Canvas Transform）。

---

## 📖 使用範例

### 事件系統

#### 定義事件 ID（靜態常量類）
```csharp
public static class EventId
{
    public static class Card
    {
        public const int OnCardLoaded = 1001;
        public const int OnFilterChanged = 1002;
    }
}
```

#### 定義事件參數
```csharp
public class CardLoadedEventArgs : GameEventArgs
{
    public override int Id => EventId.Card.OnCardLoaded;

    public CardData[] Cards { get; set; }

    public override void Clear()
    {
        Cards = null;
    }
}
```

#### 使用事件
```csharp
// 獲取事件管理器
var eventMgr = GameFrameworkEntry.GetModule<IEventManager>();

// 訂閱事件
eventMgr.Subscribe(EventId.Card.OnCardLoaded, OnCardLoaded);

// 觸發事件
var args = new CardLoadedEventArgs { Cards = loadedCards };
eventMgr.Fire(this, args);

// 取消訂閱
eventMgr.Unsubscribe(EventId.Card.OnCardLoaded, OnCardLoaded);
```

---

### 對象池（自動類型名稱）

```csharp
// 獲取對象池管理器
var poolMgr = GameFrameworkEntry.GetModule<IObjectPoolManager>();

// 創建對象池（自動使用類型名稱）
var pool = poolMgr.CreateObjectPool<UICardItem>(
    CreateCardItem,       // 創建函數
    OnSpawnCardItem,      // 獲取回調
    OnUnspawnCardItem     // 歸還回調
);

// 從對象池獲取對象
var item = pool.Spawn();

// 歸還對象池
pool.Unspawn(item);

// 獲取已創建的對象池
var existPool = poolMgr.GetObjectPool<UICardItem>();
```

---

### UI 管理（泛型 + 枚舉）

#### UI 組枚舉
```csharp
public enum UIGroup
{
    Default = 0,   // 主界面
    Popup = 100,   // 彈窗
    Tips = 200     // 提示
}
```

#### 創建 UI 表單
```csharp
public class UIMainForm : UIFormBase
{
    protected override void OnInit()
    {
        // 初始化（僅一次）
    }

    protected override void OnOpen(object userData)
    {
        // 打開時調用
    }

    protected override void OnClose()
    {
        // 關閉時調用
    }
}
```

#### 使用 UI 管理器
```csharp
// 獲取 UI 管理器
var uiMgr = GameFrameworkEntry.GetModule<IUIManager>();

// 打開 UI（泛型，類型安全）
var mainForm = await uiMgr.OpenUIForm<UIMainForm>(UIGroup.Default);
var detailForm = await uiMgr.OpenUIForm<UICardDetail>(UIGroup.Popup, cardData);

// 關閉 UI
uiMgr.CloseUIForm<UICardDetail>();

// 獲取 UI
var form = uiMgr.GetUIForm<UIMainForm>();

// 檢查是否打開
bool isOpen = uiMgr.IsUIFormOpen<UIMainForm>();
```

---

### 資源管理（AssetReference）

#### 使用 AssetReference
```csharp
[SerializeField] private AssetReference cardDataJson;
[SerializeField] private AssetReferenceSprite cardImage;

// 獲取資源管理器
var resMgr = GameFrameworkEntry.GetModule<IResourceManager>();

// 載入資源（AssetReference）
var data = await resMgr.LoadAssetAsync<TextAsset>(cardDataJson);
var sprite = await resMgr.LoadAssetAsync<Sprite>(cardImage);

// 卸載資源
resMgr.UnloadAsset(cardImage);
```

#### 使用地址字串
```csharp
// 載入資源（地址字串）
var sprite = await resMgr.LoadAssetAsync<Sprite>("CardImages/GD01-024");

// 卸載資源
resMgr.UnloadAsset("CardImages/GD01-024");
```

---

## 🎯 與官方 GameFramework 的差異

| 項目 | 官方 GameFramework | GCGFinder 簡化版 |
|------|-------------------|-----------------|
| **模組數量** | 19+ 模組 | 4 核心模組 |
| **複雜度** | 高（適合大型遊戲） | 低（適合工具應用） |
| **UI 訪問** | 字串名稱 | 泛型類型 |
| **事件 ID** | 整數 | 靜態常量類 |
| **對象池名稱** | 字串 | 自動類型名稱 |
| **資源系統** | 自定義 | Addressables |
| **學習成本** | 高 | 低 |

---

## 🔧 擴展指南

### 添加新模組

1. 定義介面繼承 `IGameFrameworkModule`
2. 實作類繼承 `GameFrameworkModule`
3. 創建 Component 繼承 `GameFrameworkComponent`
4. 通過 `GameFrameworkEntry.GetModule<T>()` 獲取

---

## 📝 注意事項

1. **BaseComponent 必須存在** - 它驅動整個框架的 Update 循環
2. **UI Prefab 路徑** - 目前暫時使用 Resources，未來需整合 Addressables
3. **類型名稱作為 Key** - 對象池、UI 表單都使用類型名稱，確保類型名稱唯一
4. **AssetReference 需要配置** - 確保資源已標記為 Addressable

---

## 📚 進階主題

### 模組優先級

模組按優先級從高到低更新：
- Event: 100
- Resource: 90
- ObjectPool: 50
- UI: 10

### 事件延遲 vs 立即

```csharp
// 延遲觸發（下一幀處理）
eventMgr.Fire(this, eventArgs);

// 立即觸發（當前幀處理）
eventMgr.FireNow(this, eventArgs);
```

---

## 🐛 疑難排解

### UI 無法打開

1. 檢查 UIComponent 是否設置了 UI Root
2. 確認 UI Prefab 路徑是否正確（Resources/UI/[類型名稱]）
3. 檢查 Prefab 上是否有對應的 UIFormBase 組件

### 資源載入失敗

1. 確認資源已標記為 Addressable
2. 檢查 AssetReference 是否有效
3. 查看 Console 錯誤訊息

### 對象池獲取失敗

1. 確認對象池已創建
2. 檢查類型名稱是否正確
3. 使用 `HasObjectPool<T>()` 檢查是否存在

---

## ✨ 致謝

本框架設計靈感來自 [GameFramework](https://github.com/EllanJiang/GameFramework) by Ellan Jiang。
