# Resource Manager 使用指南

資源管理器提供兩種實作，可在 Unity Inspector 中切換。

---

## 🎯 兩種實作

### 1. AddressableResourceManager（推薦）
- ✅ 使用 Unity Addressables 系統
- ✅ 支援 AssetReference
- ✅ 支援遠端資源更新
- ✅ 自動引用計數管理

### 2. UnityResourceManager（備用）
- ✅ 使用 Resources 資料夾
- ⚠️ 不建議用於正式專案
- ⚠️ 資源必須放在 `Assets/Resources/` 資料夾

---

## 🔧 設定方式

### 在 Inspector 中選擇

1. 選擇掛載 `ResourceComponent` 的 GameObject
2. 在 Inspector 找到 **Resource Manager Type**
3. 選擇模式：
   - **Addressable**（推薦）
   - **UnityResource**（備用）

![Resource Component Inspector]
```
┌─ Resource Component ─────────────┐
│  Resource Manager Type            │
│  ├─ Addressable    ✓              │
│  └─ UnityResource                 │
└───────────────────────────────────┘
```

---

## 📖 使用方式

### 方法 1：使用 AssetReference（推薦）

```csharp
using UnityEngine.AddressableAssets;

[SerializeField] private AssetReferenceSprite cardImage;

var resMgr = GameFrameworkEntry.GetModule<IResourceManager>();
var sprite = await resMgr.LoadAssetAsync<Sprite>(cardImage);
```

**優點：**
- 在 Inspector 中拖拽設定
- 類型安全
- 自動管理依賴

### 方法 2：使用地址字串

```csharp
var resMgr = GameFrameworkEntry.GetModule<IResourceManager>();

// Addressable 模式：使用 Address
var sprite = await resMgr.LoadAssetAsync<Sprite>("CardImages/GD01-001");

// UnityResource 模式：使用相對於 Resources 的路徑
var sprite = await resMgr.LoadAssetAsync<Sprite>("CardImages/GD01-001");
```

---

## 💾 CardData 使用範例

CardData 同時支援兩種方式：

```csharp
public class CardData
{
    public AssetReferenceSprite ImageReference;  // Addressables
    public string ImagePath;                      // Resources 備用
}
```

### 載入範例

```csharp
var resMgr = GameFrameworkEntry.GetModule<IResourceManager>();

// 優先使用 AssetReference
if (cardData.ImageReference != null && cardData.ImageReference.RuntimeKeyIsValid())
{
    var sprite = await resMgr.LoadAssetAsync<Sprite>(cardData.ImageReference);
}
// 備用：使用路徑
else if (!string.IsNullOrEmpty(cardData.ImagePath))
{
    var sprite = await resMgr.LoadAssetAsync<Sprite>(cardData.ImagePath);
}
```

---

## 📁 資源組織

### Addressable 模式

資源可以放在任意位置：

```
Assets/
├── Art/
│   └── Cards/
│       ├── GD01-001.png  ← 標記為 Addressable
│       └── GD01-002.png  ← 標記為 Addressable
└── Data/
    └── cards.json        ← 標記為 Addressable
```

**標記資源：**
1. 選擇資源
2. Inspector 勾選 "Addressable"
3. 設定 Address（可選，預設使用路徑）

### UnityResource 模式

資源**必須**放在 `Assets/Resources/` 資料夾：

```
Assets/
└── Resources/
    ├── CardImages/
    │   ├── GD01-001.png
    │   └── GD01-002.png
    └── Data/
        └── cards.json
```

**載入路徑：**
- 不包含 "Resources/" 前綴
- 不包含副檔名
- 範例：`"CardImages/GD01-001"`

---

## 🔄 資源卸載

```csharp
var resMgr = GameFrameworkEntry.GetModule<IResourceManager>();

// 卸載單個資源
resMgr.UnloadAsset(cardImage);           // AssetReference
resMgr.UnloadAsset("CardImages/GD01-001"); // 地址字串

// 卸載所有資源
resMgr.ReleaseAllAssets();
```

**注意：**
- 資源使用引用計數管理
- 多次載入同一資源只會增加計數
- 必須卸載相同次數才會真正釋放

---

## ⚙️ 進階：切換實作

### 運行時切換（不建議）

資源管理器在 Awake 時創建，運行時切換需要重啟場景。

### 建議做法

針對不同環境使用不同設定：
- 開發環境：UnityResource（快速測試）
- 正式環境：Addressable（完整功能）

---

## 🐛 疑難排解

### AddressableResourceManager 載入失敗

**檢查項目：**
1. 資源是否標記為 Addressable？
2. Address 是否正確？
3. AssetReference 是否有效？

**查看 Console：**
```
[AddressableResourceManager] 使用 Addressables 模式
Failed to load asset: xxx. Error: ...
```

### UnityResourceManager 載入失敗

**檢查項目：**
1. 資源是否在 `Assets/Resources/` 資料夾？
2. 路徑是否正確（不含 "Resources/" 和副檔名）？
3. 資源類型是否匹配？

**查看 Console：**
```
[UnityResourceManager] 使用 Resources 模式
Failed to load asset from Resources: xxx
```

### 意外的管理器類型

如果看到警告：
```
AssetReference 在 Resources 模式下會使用 RuntimeKey 作為路徑
```

**原因：** 使用 UnityResource 模式但傳入了 AssetReference

**解決：** 改用 Addressable 模式或使用字串路徑

---

## 📊 效能建議

### Addressable 模式
- ✅ 使用 AssetReference 避免字串查找
- ✅ 合理分組資源（AddressableAssetSettings）
- ✅ 預載常用資源

### UnityResource 模式
- ⚠️ Resources.Load 會阻塞主執行緒
- ⚠️ 不適合載入大量資源
- ⚠️ 建議僅用於原型開發

---

## 🚀 最佳實踐

1. **正式專案使用 Addressable**
2. **CardData 同時保留兩個欄位**（相容性）
3. **載入時優先使用 AssetReference**
4. **及時卸載不用的資源**
5. **使用對象池減少重複載入**

---

## 📚 相關文檔

- [Unity Addressables 官方文檔](https://docs.unity3d.com/Packages/com.unity.addressables@latest)
- [GameFramework README](README.md)
