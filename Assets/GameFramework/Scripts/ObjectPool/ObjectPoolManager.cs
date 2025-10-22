using System;
using System.Collections.Generic;

namespace GameFramework.ObjectPool
{
    /// <summary>
    /// 對象池管理器
    /// </summary>
    internal sealed class ObjectPoolManager : GameFrameworkModule, IObjectPoolManager
    {
        private readonly Dictionary<string, object> m_ObjectPools;

        /// <summary>
        /// 初始化對象池管理器
        /// </summary>
        public ObjectPoolManager()
        {
            m_ObjectPools = new Dictionary<string, object>();
        }

        /// <summary>
        /// 獲取遊戲框架模組優先級
        /// </summary>
        public override int Priority => 50;

        /// <summary>
        /// 獲取對象池數量
        /// </summary>
        public int Count => m_ObjectPools.Count;

        /// <summary>
        /// 關閉並清理對象池管理器
        /// </summary>
        public override void Shutdown()
        {
            foreach (var pool in m_ObjectPools.Values)
            {
                (pool as IObjectPool<object>)?.Destroy();
            }
            m_ObjectPools.Clear();
        }

        /// <summary>
        /// 創建對象池
        /// </summary>
        /// <typeparam name="T">對象類型</typeparam>
        /// <param name="name">對象池名稱</param>
        /// <param name="createFunc">創建對象函數</param>
        /// <param name="onSpawn">獲取對象回調</param>
        /// <param name="onUnspawn">歸還對象回調</param>
        /// <param name="onDestroy">銷毀對象回調</param>
        /// <returns>對象池</returns>
        public IObjectPool<T> CreateObjectPool<T>(string name, Func<T> createFunc, Action<T> onSpawn = null, Action<T> onUnspawn = null, Action<T> onDestroy = null) where T : class
        {
            string key = GetObjectPoolKey<T>(name);
            if (m_ObjectPools.ContainsKey(key))
            {
                throw new Exception($"Object pool '{key}' is already exist.");
            }

            ObjectPool<T> objectPool = new ObjectPool<T>(name, createFunc, onSpawn, onUnspawn, onDestroy);
            m_ObjectPools.Add(key, objectPool);

            return objectPool;
        }

        /// <summary>
        /// 獲取對象池
        /// </summary>
        /// <typeparam name="T">對象類型</typeparam>
        /// <param name="name">對象池名稱</param>
        /// <returns>對象池</returns>
        public IObjectPool<T> GetObjectPool<T>(string name) where T : class
        {
            string key = GetObjectPoolKey<T>(name);
            if (m_ObjectPools.TryGetValue(key, out object objectPool))
            {
                return objectPool as IObjectPool<T>;
            }

            return null;
        }

        /// <summary>
        /// 檢查是否存在對象池
        /// </summary>
        /// <typeparam name="T">對象類型</typeparam>
        /// <param name="name">對象池名稱</param>
        /// <returns>是否存在</returns>
        public bool HasObjectPool<T>(string name) where T : class
        {
            string key = GetObjectPoolKey<T>(name);
            return m_ObjectPools.ContainsKey(key);
        }

        /// <summary>
        /// 銷毀對象池
        /// </summary>
        /// <typeparam name="T">對象類型</typeparam>
        /// <param name="name">對象池名稱</param>
        public void DestroyObjectPool<T>(string name) where T : class
        {
            string key = GetObjectPoolKey<T>(name);
            if (m_ObjectPools.TryGetValue(key, out object objectPool))
            {
                (objectPool as IObjectPool<T>)?.Destroy();
                m_ObjectPools.Remove(key);
            }
        }

        private string GetObjectPoolKey<T>(string name)
        {
            return $"{typeof(T).FullName}.{name}";
        }
    }
}
