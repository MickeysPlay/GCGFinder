using System;
using System.Collections.Generic;

namespace GameFramework
{
    /// <summary>
    /// 遊戲框架入口
    /// </summary>
    public static class GameFrameworkEntry
    {
        private static readonly LinkedList<GameFrameworkModule> s_GameFrameworkModules = new LinkedList<GameFrameworkModule>();

        /// <summary>
        /// 獲取遊戲框架模組
        /// </summary>
        /// <typeparam name="T">要獲取的遊戲框架模組類型</typeparam>
        /// <returns>要獲取的遊戲框架模組</returns>
        public static T GetModule<T>() where T : class
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new Exception($"You must get module by interface, but '{interfaceType.FullName}' is not.");
            }

            // 檢查是否為 GameFramework 命名空間下的介面
            if (!interfaceType.FullName.StartsWith("GameFramework."))
            {
                throw new Exception($"You must get a Game Framework module, but '{interfaceType.FullName}' is not.");
            }

            // 檢查介面名稱是否以 I 開頭
            if (!interfaceType.Name.StartsWith("I"))
            {
                throw new Exception($"You must get module by interface, but '{interfaceType.Name}' does not start with 'I'.");
            }

            // 檢查是否已經創建
            foreach (GameFrameworkModule module in s_GameFrameworkModules)
            {
                if (interfaceType.IsAssignableFrom(module.GetType()))
                {
                    return module as T;
                }
            }

            // 尚未創建，使用預設規則創建
            // 從完整命名空間中提取類別名稱，移除 I 前綴
            string moduleName = interfaceType.Name.Substring(1); // 移除 "I" 前綴
            string moduleNamespace = interfaceType.Namespace; // 例如: GameFramework.UI
            string fullTypeName = $"{moduleNamespace}.{moduleName}";

            // 先嘗試從當前程式集查找類型
            Type moduleType = interfaceType.Assembly.GetType(fullTypeName);

            // 如果找不到，嘗試從所有已載入的程式集查找
            if (moduleType == null)
            {
                foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies())
                {
                    moduleType = assembly.GetType(fullTypeName);
                    if (moduleType != null)
                    {
                        break;
                    }
                }
            }

            if (moduleType == null)
            {
                throw new Exception($"Can not find Game Framework module type '{fullTypeName}'.");
            }

            return GetModule(moduleType) as T;
        }

        /// <summary>
        /// 獲取或創建指定類型的遊戲框架模組
        /// </summary>
        /// <typeparam name="TInterface">介面類型</typeparam>
        /// <typeparam name="TImplementation">實作類型</typeparam>
        /// <returns>要獲取的遊戲框架模組</returns>
        public static TInterface GetModule<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : GameFrameworkModule, TInterface
        {
            Type interfaceType = typeof(TInterface);
            Type implementationType = typeof(TImplementation);

            // 檢查是否已經創建
            foreach (GameFrameworkModule module in s_GameFrameworkModules)
            {
                if (interfaceType.IsAssignableFrom(module.GetType()))
                {
                    return module as TInterface;
                }
            }

            // 尚未創建，創建指定實作
            return GetModule(implementationType) as TInterface;
        }

        /// <summary>
        /// 獲取遊戲框架模組
        /// </summary>
        /// <param name="moduleType">要獲取的遊戲框架模組類型</param>
        /// <returns>要獲取的遊戲框架模組</returns>
        private static GameFrameworkModule GetModule(Type moduleType)
        {
            foreach (GameFrameworkModule module in s_GameFrameworkModules)
            {
                if (module.GetType() == moduleType)
                {
                    return module;
                }
            }

            return CreateModule(moduleType);
        }

        /// <summary>
        /// 創建遊戲框架模組
        /// </summary>
        /// <param name="moduleType">要創建的遊戲框架模組類型</param>
        /// <returns>要創建的遊戲框架模組</returns>
        private static GameFrameworkModule CreateModule(Type moduleType)
        {
            GameFrameworkModule module = (GameFrameworkModule)Activator.CreateInstance(moduleType);
            if (module == null)
            {
                throw new Exception($"Can not create module '{moduleType.FullName}'.");
            }

            LinkedListNode<GameFrameworkModule> current = s_GameFrameworkModules.First;
            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                s_GameFrameworkModules.AddBefore(current, module);
            }
            else
            {
                s_GameFrameworkModules.AddLast(module);
            }

            return module;
        }

        /// <summary>
        /// 輪詢所有遊戲框架模組
        /// </summary>
        /// <param name="elapseSeconds">邏輯流逝時間</param>
        /// <param name="realElapseSeconds">真實流逝時間</param>
        internal static void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (GameFrameworkModule module in s_GameFrameworkModules)
            {
                module.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 關閉並清理所有遊戲框架模組
        /// </summary>
        internal static void Shutdown()
        {
            for (LinkedListNode<GameFrameworkModule> current = s_GameFrameworkModules.Last; current != null; current = current.Previous)
            {
                current.Value.Shutdown();
            }

            s_GameFrameworkModules.Clear();
        }
    }
}
