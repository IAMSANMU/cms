using System.Runtime.CompilerServices;



namespace Imow.Framework.Engine
{
    /// <summary>
    /// 拿到引擎可以拿到所有注册好的模块
    /// </summary>
    public class ImowEngineContext
    {
        #region Utilities

        /// <summary>
        /// 获得引擎单例
        /// </summary>
        protected static IEngine CreateEngineInstance()
        {
            return new ImowEngine();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 初始化引擎
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate)
        {
            if (Singleton<IEngine>.Instance == null || forceRecreate)
            {
                Singleton<IEngine>.Instance = CreateEngineInstance();
                Singleton<IEngine>.Instance.Initialize();
            }
            return Singleton<IEngine>.Instance;
        }

        /// <summary>
        /// 替换引擎的实现
        /// </summary>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获得单例引擎 可以获得所有注册进来的策略
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize(false);
                }
                return Singleton<IEngine>.Instance;
            }
        }

        #endregion
    }
}
