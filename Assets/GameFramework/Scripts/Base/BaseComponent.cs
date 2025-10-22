using UnityEngine;

namespace GameFramework
{
    /// <summary>
    /// 基礎組件，負責驅動框架更新
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Base")]
    public sealed class BaseComponent : GameFrameworkComponent
    {
        private void Update()
        {
            GameFrameworkEntry.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void OnApplicationQuit()
        {
            GameFrameworkEntry.Shutdown();
        }
    }
}
