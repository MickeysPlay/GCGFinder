using System;
using UnityEngine;

namespace GameFramework.ObjectPool
{
    /// <summary>
    /// 對象池組件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Object Pool")]
    public sealed class ObjectPoolComponent : GameFrameworkComponent
    {
        private IObjectPoolManager m_ObjectPoolManager = null;

        /// <summary>
        /// 獲取對象池數量
        /// </summary>
        public int Count
        {
            get
            {
                return m_ObjectPoolManager.Count;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_ObjectPoolManager = GameFrameworkEntry.GetModule<IObjectPoolManager>();
            if (m_ObjectPoolManager == null)
            {
                Debug.LogError("Object pool manager is invalid.");
                return;
            }
        }

        /// <summary>
        /// 創建對象池（自動使用類型名稱）
        /// </summary>
        /// <typeparam name="T">對象類型</typeparam>
        /// <param name="createFunc">創建對象函數</param>
        /// <param name="onSpawn">獲取對象回調</param>
        /// <param name="onUnspawn">歸還對象回調</param>
        /// <param name="onDestroy">銷毀對象回調</param>
        /// <returns>對象池</returns>
        public IObjectPool<T> CreateObjectPool<T>(Func<T> createFunc, Action<T> onSpawn = null, Action<T> onUnspawn = null, Action<T> onDestroy = null) where T : class
        {
            return m_ObjectPoolManager.CreateObjectPool(createFunc, onSpawn, onUnspawn, onDestroy);
        }

        /// <summary>
        /// 獲取對象池（自動使用類型名稱）
        /// </summary>
        /// <typeparam name="T">對象類型</typeparam>
        /// <returns>對象池</returns>
        public IObjectPool<T> GetObjectPool<T>() where T : class
        {
            return m_ObjectPoolManager.GetObjectPool<T>();
        }

        /// <summary>
        /// 檢查是否存在對象池（自動使用類型名稱）
        /// </summary>
        /// <typeparam name="T">對象類型</typeparam>
        /// <returns>是否存在</returns>
        public bool HasObjectPool<T>() where T : class
        {
            return m_ObjectPoolManager.HasObjectPool<T>();
        }

        /// <summary>
        /// 銷毀對象池（自動使用類型名稱）
        /// </summary>
        /// <typeparam name="T">對象類型</typeparam>
        public void DestroyObjectPool<T>() where T : class
        {
            m_ObjectPoolManager.DestroyObjectPool<T>();
        }
    }
}
