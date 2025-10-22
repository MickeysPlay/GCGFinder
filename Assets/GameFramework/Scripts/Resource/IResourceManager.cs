using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace GameFramework.Resource
{
    /// <summary>
    /// 資源管理器介面
    /// </summary>
    public interface IResourceManager
    {
        /// <summary>
        /// 異步載入資源（使用 AssetReference）
        /// </summary>
        /// <typeparam name="T">資源類型</typeparam>
        /// <param name="assetReference">資源引用</param>
        /// <returns>載入的資源</returns>
        Task<T> LoadAssetAsync<T>(AssetReference assetReference) where T : UnityEngine.Object;

        /// <summary>
        /// 異步載入資源（使用地址字串）
        /// </summary>
        /// <typeparam name="T">資源類型</typeparam>
        /// <param name="address">資源地址</param>
        /// <returns>載入的資源</returns>
        Task<T> LoadAssetAsync<T>(string address) where T : UnityEngine.Object;

        /// <summary>
        /// 卸載資源（使用 AssetReference）
        /// </summary>
        /// <param name="assetReference">資源引用</param>
        void UnloadAsset(AssetReference assetReference);

        /// <summary>
        /// 卸載資源（使用地址字串）
        /// </summary>
        /// <param name="address">資源地址</param>
        void UnloadAsset(string address);

        /// <summary>
        /// 釋放所有資源
        /// </summary>
        void ReleaseAllAssets();
    }
}
