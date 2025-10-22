using System;
using System.Collections.Generic;

namespace GameFramework.ObjectPool
{
    /// <summary>
    /// 對象池
    /// </summary>
    /// <typeparam name="T">對象類型</typeparam>
    internal sealed class ObjectPool<T> : IObjectPool<T> where T : class
    {
        private readonly string m_Name;
        private readonly Stack<T> m_Objects;
        private readonly HashSet<T> m_SpawnedObjects;
        private readonly Func<T> m_CreateFunc;
        private readonly Action<T> m_OnSpawn;
        private readonly Action<T> m_OnUnspawn;
        private readonly Action<T> m_OnDestroy;

        /// <summary>
        /// 初始化對象池
        /// </summary>
        /// <param name="name">對象池名稱</param>
        /// <param name="createFunc">創建對象函數</param>
        /// <param name="onSpawn">獲取對象回調</param>
        /// <param name="onUnspawn">歸還對象回調</param>
        /// <param name="onDestroy">銷毀對象回調</param>
        public ObjectPool(string name, Func<T> createFunc, Action<T> onSpawn = null, Action<T> onUnspawn = null, Action<T> onDestroy = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Object pool name is invalid.");
            }

            if (createFunc == null)
            {
                throw new Exception("Create function is invalid.");
            }

            m_Name = name;
            m_Objects = new Stack<T>();
            m_SpawnedObjects = new HashSet<T>();
            m_CreateFunc = createFunc;
            m_OnSpawn = onSpawn;
            m_OnUnspawn = onUnspawn;
            m_OnDestroy = onDestroy;
        }

        /// <summary>
        /// 獲取對象池名稱
        /// </summary>
        public string Name => m_Name;

        /// <summary>
        /// 獲取對象池中對象的數量
        /// </summary>
        public int Count => m_Objects.Count + m_SpawnedObjects.Count;

        /// <summary>
        /// 獲取對象池中可使用對象的數量
        /// </summary>
        public int CanSpawnCount => m_Objects.Count;

        /// <summary>
        /// 從對象池獲取對象
        /// </summary>
        /// <returns>對象</returns>
        public T Spawn()
        {
            T obj;
            if (m_Objects.Count > 0)
            {
                obj = m_Objects.Pop();
            }
            else
            {
                obj = m_CreateFunc();
            }

            m_SpawnedObjects.Add(obj);
            m_OnSpawn?.Invoke(obj);

            return obj;
        }

        /// <summary>
        /// 將對象歸還對象池
        /// </summary>
        /// <param name="obj">對象</param>
        public void Unspawn(T obj)
        {
            if (obj == null)
            {
                throw new Exception("Object is invalid.");
            }

            if (!m_SpawnedObjects.Remove(obj))
            {
                throw new Exception("Object is not spawned from this pool.");
            }

            m_OnUnspawn?.Invoke(obj);
            m_Objects.Push(obj);
        }

        /// <summary>
        /// 釋放對象池中的所有未使用對象
        /// </summary>
        public void ReleaseAllUnused()
        {
            while (m_Objects.Count > 0)
            {
                T obj = m_Objects.Pop();
                m_OnDestroy?.Invoke(obj);
            }
        }

        /// <summary>
        /// 銷毀對象池
        /// </summary>
        public void Destroy()
        {
            ReleaseAllUnused();
            m_SpawnedObjects.Clear();
        }
    }
}
