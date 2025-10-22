using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 遊戲框架組件抽象類
    /// </summary>
    public abstract class GameFrameworkComponent : MonoBehaviour
    {
        /// <summary>
        /// 遊戲框架組件初始化
        /// </summary>
        protected virtual void Awake()
        {
        }

        /// <summary>
        /// 遊戲框架組件啟動
        /// </summary>
        protected virtual void Start()
        {
        }

        /// <summary>
        /// 遊戲框架組件銷毀
        /// </summary>
        protected virtual void OnDestroy()
        {
        }
    }
}
