using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework.Scene
{
    /// <summary>
    /// 場景載入工具類
    /// </summary>
    public static class SceneLoader
    {
        /// <summary>
        /// 同步載入場景
        /// </summary>
        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// 異步載入場景
        /// </summary>
        public static AsyncOperation LoadSceneAsync(string sceneName)
        {
            return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
