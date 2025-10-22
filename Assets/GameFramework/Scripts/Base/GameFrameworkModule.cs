namespace GameFramework
{
    /// <summary>
    /// 遊戲框架模組介面
    /// </summary>
    public interface IGameFrameworkModule
    {
        /// <summary>
        /// 獲取模組優先級
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 模組輪詢
        /// </summary>
        /// <param name="elapseSeconds">邏輯流逝時間</param>
        /// <param name="realElapseSeconds">真實流逝時間</param>
        void Update(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 關閉並清理模組
        /// </summary>
        void Shutdown();
    }

    /// <summary>
    /// 遊戲框架模組抽象類
    /// </summary>
    public abstract class GameFrameworkModule : IGameFrameworkModule
    {
        /// <summary>
        /// 獲取模組優先級
        /// 優先級較高的模組會優先輪詢，並且關閉操作會後進行
        /// </summary>
        public virtual int Priority => 0;

        /// <summary>
        /// 模組輪詢
        /// </summary>
        /// <param name="elapseSeconds">邏輯流逝時間</param>
        /// <param name="realElapseSeconds">真實流逝時間</param>
        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {
        }

        /// <summary>
        /// 關閉並清理模組
        /// </summary>
        public virtual void Shutdown()
        {
        }
    }
}
