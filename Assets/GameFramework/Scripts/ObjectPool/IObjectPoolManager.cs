using System;

namespace GameFramework.ObjectPool
{
    /// <summary>
    /// 對象池管理器介面
    /// </summary>
    public interface IObjectPoolManager
    {
        /// <summary>
        /// 獲取對象池數量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 創建對象池（自動使用類型名稱）
        /// </summary>
        /// <typeparam name="T">對象類型</typeparam>
        /// <param name="createFunc">創建對象函數</param>
        /// <param name="onSpawn">獲取對象回調</param>
        /// <param name="onUnspawn">歸還對象回調</param>
        /// <param name="onDestroy">銷毀對象回調</param>
        /// <returns>對象池</returns>
        IObjectPool<T> CreateObjectPool<T>(Func<T> createFunc, Action<T> onSpawn = null, Action<T> onUnspawn = null, Action<T> onDestroy = null) where T : class;

        /// <summary>
        /// 獲取對象池（自動使用類型名稱）
        /// </summary>
        /// <typeparam name="T">對象類型</typeparam>
        /// <returns>對象池</returns>
        IObjectPool<T> GetObjectPool<T>() where T : class;

        /// <summary>
        /// 檢查是否存在對象池（自動使用類型名稱）
        /// </summary>
        /// <typeparam name="T">對象類型</typeparam>
        /// <returns>是否存在</returns>
        bool HasObjectPool<T>() where T : class;

        /// <summary>
        /// 銷毀對象池（自動使用類型名稱）
        /// </summary>
        /// <typeparam name="T">對象類型</typeparam>
        void DestroyObjectPool<T>() where T : class;
    }
}
