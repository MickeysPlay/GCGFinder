using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameFramework.UI
{
    /// <summary>
    /// UI 管理器
    /// </summary>
    internal sealed class UIManager : GameFrameworkModule, IUIManager
    {
        private Transform m_UIRoot;
        private readonly Dictionary<UIGroup, Transform> m_UIGroups;
        private readonly Dictionary<Type, UIFormBase> m_UIForms;
        private readonly Dictionary<Type, UIGroup> m_UIFormGroups;

        /// <summary>
        /// 初始化 UI 管理器
        /// </summary>
        public UIManager()
        {
            m_UIGroups = new Dictionary<UIGroup, Transform>();
            m_UIForms = new Dictionary<Type, UIFormBase>();
            m_UIFormGroups = new Dictionary<Type, UIGroup>();
        }

        /// <summary>
        /// 獲取遊戲框架模組優先級
        /// </summary>
        public override int Priority => 10;

        /// <summary>
        /// 關閉並清理 UI 管理器
        /// </summary>
        public override void Shutdown()
        {
            foreach (var uiForm in m_UIForms.Values)
            {
                if (uiForm != null)
                {
                    UnityEngine.Object.Destroy(uiForm.gameObject);
                }
            }

            m_UIForms.Clear();
            m_UIFormGroups.Clear();
            m_UIGroups.Clear();
        }

        /// <summary>
        /// 設置 UI 根節點
        /// </summary>
        /// <param name="uiRoot">UI 根節點 Transform</param>
        public void SetUIRoot(Transform uiRoot)
        {
            m_UIRoot = uiRoot;
            InitializeUIGroups();
        }

        /// <summary>
        /// 打開 UI 表單
        /// </summary>
        /// <typeparam name="T">UI 表單類型</typeparam>
        /// <param name="uiGroup">UI 組</param>
        /// <param name="userData">用戶自定義數據</param>
        /// <returns>UI 表單實例</returns>
        public async Task<T> OpenUIForm<T>(UIGroup uiGroup, object userData = null) where T : UIFormBase
        {
            Type formType = typeof(T);

            // 檢查是否已創建
            if (m_UIForms.TryGetValue(formType, out UIFormBase existForm))
            {
                existForm.Open(userData);
                return existForm as T;
            }

            // 載入 UI Prefab
            T uiForm = await LoadUIForm<T>(uiGroup);
            if (uiForm == null)
            {
                Debug.LogError($"Failed to load UI form '{formType.Name}'.");
                return null;
            }

            // 初始化並打開
            m_UIForms.Add(formType, uiForm);
            m_UIFormGroups.Add(formType, uiGroup);
            uiForm.Open(userData);

            return uiForm;
        }

        /// <summary>
        /// 關閉 UI 表單
        /// </summary>
        /// <typeparam name="T">UI 表單類型</typeparam>
        public void CloseUIForm<T>() where T : UIFormBase
        {
            Type formType = typeof(T);
            if (m_UIForms.TryGetValue(formType, out UIFormBase uiForm))
            {
                uiForm.Close();
            }
        }

        /// <summary>
        /// 獲取 UI 表單
        /// </summary>
        /// <typeparam name="T">UI 表單類型</typeparam>
        /// <returns>UI 表單實例</returns>
        public T GetUIForm<T>() where T : UIFormBase
        {
            Type formType = typeof(T);
            if (m_UIForms.TryGetValue(formType, out UIFormBase uiForm))
            {
                return uiForm as T;
            }
            return null;
        }

        /// <summary>
        /// 檢查 UI 表單是否已打開
        /// </summary>
        /// <typeparam name="T">UI 表單類型</typeparam>
        /// <returns>是否已打開</returns>
        public bool IsUIFormOpen<T>() where T : UIFormBase
        {
            Type formType = typeof(T);
            if (m_UIForms.TryGetValue(formType, out UIFormBase uiForm))
            {
                return uiForm.IsOpen;
            }
            return false;
        }

        /// <summary>
        /// 初始化 UI 組
        /// </summary>
        private void InitializeUIGroups()
        {
            if (m_UIRoot == null)
            {
                Debug.LogError("UI root is not set.");
                return;
            }

            // 為每個 UI 組創建容器
            foreach (UIGroup group in Enum.GetValues(typeof(UIGroup)))
            {
                CreateUIGroup(group);
            }
        }

        /// <summary>
        /// 創建 UI 組
        /// </summary>
        private void CreateUIGroup(UIGroup group)
        {
            if (m_UIGroups.ContainsKey(group))
            {
                return;
            }

            GameObject groupObj = new GameObject(group.ToString());
            groupObj.transform.SetParent(m_UIRoot, false);

            // 添加 RectTransform
            RectTransform rectTransform = groupObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;

            // 添加 Canvas Group（用於控制整組的透明度、交互等）
            groupObj.AddComponent<CanvasGroup>();

            m_UIGroups.Add(group, groupObj.transform);
        }

        /// <summary>
        /// 載入 UI 表單
        /// </summary>
        private async Task<T> LoadUIForm<T>(UIGroup uiGroup) where T : UIFormBase
        {
            // 這裡需要整合 ResourceManager 載入 UI Prefab
            // 暫時使用 Resources.Load 作為臨時方案
            string prefabPath = $"UI/{typeof(T).Name}";

            // 異步載入（模擬）
            await Task.Yield();

            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab == null)
            {
                Debug.LogError($"UI prefab not found at path: {prefabPath}");
                return null;
            }

            Transform groupTransform = m_UIGroups[uiGroup];
            GameObject uiObj = UnityEngine.Object.Instantiate(prefab, groupTransform);
            uiObj.name = typeof(T).Name;

            T uiForm = uiObj.GetComponent<T>();
            if (uiForm == null)
            {
                uiForm = uiObj.AddComponent<T>();
            }

            return uiForm;
        }
    }
}
