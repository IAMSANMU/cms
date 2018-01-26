using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Imow.Framework.Engine.DependencyManager;
using Imow.Framework.Engine.TypeFinder;
using imow.Core.config;
using imow.Core.cookieModel;
using imow.Framework.Config;
using imow.Framework.Cookie;


namespace Imow.Framework.Engine
{
    /// <summary>
    /// 依赖注入引擎
    /// </summary>
    public class ImowEngine : IEngine
    {
        #region Fields

        private ContainerManager _containerManager;

        #endregion

        #region Utilities

        /// <summary>
        /// 注册策略
        /// </summary>
        protected virtual void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();
            //从程序集中获得策略
            var typeFinder = new WebAppTypeFinder();
            builder = new ContainerBuilder();
            builder.RegisterInstance(this).As<IEngine>().SingleInstance();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            ////注册配置文件
            builder.RegisterGeneric(typeof(ConfigManager<>)).InstancePerLifetimeScope();
            ////注册config
            var drConfigTypes = typeFinder.FindClassesOfType<IConfig>();
            foreach (Type drConfigType in drConfigTypes)
            {
                builder.RegisterType(drConfigType).InstancePerLifetimeScope();
            }

            //注册cookiehelp
            builder.RegisterGeneric(typeof(CookieManager<>)).InstancePerLifetimeScope();
            ////注册cookie
            var drCookieTypes = typeFinder.FindClassesOfType<ICookieModel>();
            foreach (Type drCookieType in drCookieTypes)
            {
                builder.RegisterType(drCookieType).SingleInstance();
            }
            builder.Update(container);
            this._containerManager = new ContainerManager(container);
            //从其他程序集中注册策略
            builder = new ContainerBuilder();
            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var drInstances = new List<IDependencyRegistrar>();
            foreach (var drType in drTypes)
                drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();

            foreach (var dependencyRegistrar in drInstances)
                dependencyRegistrar.Register(builder, typeFinder);
            builder.Update(container);

            this._containerManager = new ContainerManager(container);

            //启用策略
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        #endregion

        #region Methods

        public void Initialize()
        {
            RegisterDependencies();
        }

        /// <summary>
        /// 获得config
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ResolveConfig<T>() where T : class, IConfig
        {
            return ImowEngineContext.Current.Resolve<ConfigManager<T>>().GetConfigInfo();
        }

        /// <summary>
        ///获得策略
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }
        /// <summary>
        ///获得策略
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T Resolve<T>(string key) where T : class
        {
            return ContainerManager.Resolve<T>(key);
        }
        /// <summary>
        ///获得策略
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }
        /// <summary>
        /// 获得策略
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns></returns>
        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 容器管理器
        /// </summary>
        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        #endregion



    }
}
