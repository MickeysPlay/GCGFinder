using System;

namespace GameFramework.ObjectPool
{
    /// <summary>
    /// 對象池介面
    /// </summary>
    public interface IObjectPool<T> where T : class
    {
        /// <summary>
        /// 獲取對象池名稱
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 獲取對象池中對象的數量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 獲取對象池中可使用對象的數量
        /// </summary>
        int CanSpawnCount { get; }

        /// <summary>
        /// 從對象池獲取對象
        /// </summary>
        /// <returns>對象</returns>
        T Spawn();

        /// <summary>
        /// 將對象歸還對象池
        /// </summary>
        /// <param name="obj">對象</param>
        void Unspawn(T obj);

        /// <summary>
        /// 釋放對象池中的所有未使用對象
        /// </summary>
        void ReleaseAllUnused();

        /// <summary>
        /// 銷毀對象池
        /// </summary>
        void Destroy();
    }
}
