using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imow.Framework.Engine.DependencyManager;
using imow.Core.config;

namespace Imow.Framework.Engine
{
    public interface IEngine
    {
        /// <summary>
        /// 容易管理器
        /// </summary>
        ContainerManager ContainerManager { get; }

        /// <summary>
        ///初始化
        /// </summary>
        void Initialize();

        T ResolveConfig<T>()where T : class, IConfig;

        /// <summary>
        ///获得策略
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : class;
        /// <summary>
        ///获得策略
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        T Resolve<T>(string key) where T : class;
        /// <summary>
        ///  获得策略
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        object Resolve(Type type);

        /// <summary>
        /// 获得策略
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        T[] ResolveAll<T>();
    }
}
