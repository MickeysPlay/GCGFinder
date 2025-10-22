using UnityEngine;
using GameFramework.Scene;

namespace GameMain
{
    /// <summary>
    /// 遊戲入口點
    /// 負責初始化並跳轉到主場景
    /// </summary>
    public class GameEntry : MonoBehaviour
    {
        [Header("場景設定")]
        [SerializeField] private string gameSceneName = "Game";

        private void Start()
        {
            // 目前直接跳轉到主場景
            // 未來可在此處加入 Preload 場景邏輯
            LoadGameScene();
        }

        private void LoadGameScene()
        {
            SceneLoader.LoadScene(gameSceneName);
        }
    }
}
