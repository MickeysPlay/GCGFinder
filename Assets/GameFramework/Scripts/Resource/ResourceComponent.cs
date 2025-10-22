using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameFramework.Resource
{
    /// <summary>
    /// 資源組件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Resource")]
    public sealed class ResourceComponent : GameFrameworkComponent
    {
        /// <summary>
        /// 資源管理器類型
        /// </summary>
        public enum ResourceManagerType
        {
            /// <summary>
            /// Addressables 模式（推薦）
            /// </summary>
            Addressable,

            /// <summary>
            /// Resources 模式（備用）
            /// </summary>
            UnityResource
        }

        [Header("資源管理器設定")]
        [SerializeField]
        [Tooltip("選擇資源管理器類型：\n- Addressable: 使用 Addressables 系統（推薦）\n- UnityResource: 使用 Resources 資料夾（備用）")]
        private ResourceManagerType m_ManagerType = ResourceManagerType.Addressable;

        private IResourceManager m_ResourceManager = null;

        /// <summary>
        /// 獲取當前使用的資源管理器類型
        /// </summary>
        public ResourceManagerType ManagerType => m_ManagerType;

        protected override void Awake()
        {
            base.Awake();

            // 根據設定獲取或創建對應的資源管理器
            if (m_ManagerType == ResourceManagerType.Addressable)
            {
                m_ResourceManager = GameFrameworkEntry.GetModule<IResourceManager, AddressableResourceManager>();
            }
            else
            {
                m_ResourceManager = GameFrameworkEntry.GetModule<IResourceManager, UnityResourceManager>();
            }

            if (m_ResourceManager == null)
            {
                Debug.LogError("Resource manager is invalid.");
                return;
            }
        }

        /// <summary>
        /// 異步載入資源（使用 AssetReference）
        /// </summary>
        /// <typeparam name="T">資源類型</typeparam>
        /// <param name="assetReference">資源引用</param>
        /// <returns>載入的資源</returns>
        public async Task<T> LoadAssetAsync<T>(AssetReference assetReference) where T : UnityEngine.Object
        {
            return await m_ResourceManager.LoadAssetAsync<T>(assetReference);
        }

        /// <summary>
        /// 異步載入資源（使用地址字串）
        /// </summary>
        /// <typeparam name="T">資源類型</typeparam>
        /// <param name="address">資源地址</param>
        /// <returns>載入的資源</returns>
        public async Task<T> LoadAssetAsync<T>(string address) where T : UnityEngine.Object
        {
            return await m_ResourceManager.LoadAssetAsync<T>(address);
        }

        /// <summary>
        /// 卸載資源（使用 AssetReference）
        /// </summary>
        /// <param name="assetReference">資源引用</param>
        public void UnloadAsset(AssetReference assetReference)
        {
            m_ResourceManager.UnloadAsset(assetReference);
        }

        /// <summary>
        /// 卸載資源（使用地址字串）
        /// </summary>
        /// <param name="address">資源地址</param>
        public void UnloadAsset(string address)
        {
            m_ResourceManager.UnloadAsset(address);
        }

        /// <summary>
        /// 釋放所有資源
        /// </summary>
        public void ReleaseAllAssets()
        {
            m_ResourceManager.ReleaseAllAssets();
        }
    }
}
