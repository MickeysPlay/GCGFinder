using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameFramework.Resource
{
    /// <summary>
    /// Addressables 資源管理器
    /// </summary>
    internal sealed class AddressableResourceManager : GameFrameworkModule, IResourceManager
    {
        private readonly Dictionary<string, AsyncOperationHandle> m_LoadedAssets;
        private readonly Dictionary<string, int> m_AssetRefCount;

        /// <summary>
        /// 初始化資源管理器
        /// </summary>
        public AddressableResourceManager()
        {
            m_LoadedAssets = new Dictionary<string, AsyncOperationHandle>();
            m_AssetRefCount = new Dictionary<string, int>();

            Debug.Log("[AddressableResourceManager] 使用 Addressables 模式");
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
        /// </summary>
        public async Task<T> LoadAssetAsync<T>(AssetReference assetReference) where T : UnityEngine.Object
        {
            if (assetReference == null || !assetReference.RuntimeKeyIsValid())
            {
                Debug.LogError("Asset reference is invalid.");
                return null;
            }

            string key = assetReference.AssetGUID;
            return await LoadAssetAsyncInternal<T>(key, assetReference.LoadAssetAsync<T>());
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

            return await LoadAssetAsyncInternal<T>(address, Addressables.LoadAssetAsync<T>(address));
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

            string key = assetReference.AssetGUID;
            UnloadAssetInternal(key);
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
            foreach (var handle in m_LoadedAssets.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }

            m_LoadedAssets.Clear();
            m_AssetRefCount.Clear();
        }

        /// <summary>
        /// 內部異步載入資源
        /// </summary>
        private async Task<T> LoadAssetAsyncInternal<T>(string key, AsyncOperationHandle<T> handle) where T : UnityEngine.Object
        {
            // 檢查是否已載入
            if (m_LoadedAssets.ContainsKey(key))
            {
                m_AssetRefCount[key]++;
                var existHandle = m_LoadedAssets[key];
                return existHandle.Result as T;
            }

            // 等待載入完成
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                m_LoadedAssets.Add(key, handle);
                m_AssetRefCount.Add(key, 1);
                return handle.Result;
            }
            else
            {
                Debug.LogError($"Failed to load asset: {key}. Error: {handle.OperationException}");
                return null;
            }
        }

        /// <summary>
        /// 內部卸載資源
        /// </summary>
        private void UnloadAssetInternal(string key)
        {
            if (!m_LoadedAssets.ContainsKey(key))
            {
                return;
            }

            m_AssetRefCount[key]--;

            if (m_AssetRefCount[key] <= 0)
            {
                var handle = m_LoadedAssets[key];
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }

                m_LoadedAssets.Remove(key);
                m_AssetRefCount.Remove(key);
            }
        }
    }
}
