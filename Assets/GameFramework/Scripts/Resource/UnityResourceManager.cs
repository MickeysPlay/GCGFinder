using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameFramework.Resource
{
    /// <summary>
    /// Unity Resources 資源管理器（備用方案）
    /// </summary>
    internal sealed class UnityResourceManager : GameFrameworkModule, IResourceManager
    {
        private readonly Dictionary<string, UnityEngine.Object> m_LoadedAssets;
        private readonly Dictionary<string, int> m_AssetRefCount;

        /// <summary>
        /// 初始化資源管理器
        /// </summary>
        public UnityResourceManager()
        {
            m_LoadedAssets = new Dictionary<string, UnityEngine.Object>();
            m_AssetRefCount = new Dictionary<string, int>();

            Debug.LogWarning("[UnityResourceManager] 使用 Resources 模式（資源需放在 Assets/Resources/ 資料夾）");
        }

        /// <summary>
        /// 獲取遊戲框架模組優先級
        /// </summary>
        public override int Priority => 90;

        /// <summary>
        /// 關閉並清理資源管理器
        /// </summary>
        public override void Shutdown()
        {
            ReleaseAllAssets();
        }

        /// <summary>
        /// 異步載入資源（使用 AssetReference）
        /// Resources 模式不支援 AssetReference，會使用 RuntimeKey 作為路徑
        /// </summary>
        public async Task<T> LoadAssetAsync<T>(AssetReference assetReference) where T : UnityEngine.Object
        {
            if (assetReference == null || !assetReference.RuntimeKeyIsValid())
            {
                Debug.LogError("Asset reference is invalid.");
                return null;
            }

            // 在 Resources 模式下，嘗試使用 AssetReference 的 RuntimeKey 作為路徑
            string path = assetReference.RuntimeKey.ToString();
            Debug.LogWarning($"[UnityResourceManager] AssetReference 在 Resources 模式下會使用 RuntimeKey 作為路徑: {path}");

            return await LoadAssetAsync<T>(path);
        }

        /// <summary>
        /// 異步載入資源（使用地址字串）
        /// </summary>
        public async Task<T> LoadAssetAsync<T>(string address) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(address))
            {
                Debug.LogError("Asset address is invalid.");
                return null;
            }

            // 檢查是否已載入
            if (m_LoadedAssets.ContainsKey(address))
            {
                m_AssetRefCount[address]++;
                return m_LoadedAssets[address] as T;
            }

            // 模擬異步載入
            await Task.Yield();

            // 使用 Resources.Load 載入
            T asset = Resources.Load<T>(address);
            if (asset == null)
            {
                Debug.LogError($"Failed to load asset from Resources: {address}");
                return null;
            }

            m_LoadedAssets.Add(address, asset);
            m_AssetRefCount.Add(address, 1);

            return asset;
        }

        /// <summary>
        /// 卸載資源（使用 AssetReference）
        /// </summary>
        public void UnloadAsset(AssetReference assetReference)
        {
            if (assetReference == null || !assetReference.RuntimeKeyIsValid())
            {
                return;
            }

            string path = assetReference.RuntimeKey.ToString();
            UnloadAsset(path);
        }

        /// <summary>
        /// 卸載資源（使用地址字串）
        /// </summary>
        public void UnloadAsset(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                return;
            }

            UnloadAssetInternal(address);
        }

        /// <summary>
        /// 釋放所有資源
        /// </summary>
        public void ReleaseAllAssets()
        {
            foreach (var asset in m_LoadedAssets.Values)
            {
                if (asset != null)
                {
                    Resources.UnloadAsset(asset);
                }
            }

            m_LoadedAssets.Clear();
            m_AssetRefCount.Clear();
        }

        /// <summary>
        /// 內部卸載資源
        /// </summary>
        private void UnloadAssetInternal(string address)
        {
            if (!m_LoadedAssets.ContainsKey(address))
            {
                return;
            }

            m_AssetRefCount[address]--;

            if (m_AssetRefCount[address] <= 0)
            {
                var asset = m_LoadedAssets[address];
                if (asset != null)
                {
                    Resources.UnloadAsset(asset);
                }

                m_LoadedAssets.Remove(address);
                m_AssetRefCount.Remove(address);
            }
        }
    }
}
