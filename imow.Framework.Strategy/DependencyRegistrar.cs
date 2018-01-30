using System;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using imow.Core.config;
using imow.Core.Job;
using imow.Framework.Interface;
using imow.Framework.Strategy.Solr;
using imow.IRepository;
using imow.IRepository.Admin;
using imow.Repository;
using imow.Repository.Repository.Admin;
using imow.Services;
using Imow.Framework.Cache.DistributedCache.Storage;
using Imow.Framework.Engine;
using Imow.Framework.Engine.DependencyManager;
using Imow.Framework.Engine.TypeFinder;
using WebImow.Common.Framework.Strategy.UI;

namespace imow.Framework.Strategy
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// 通用依赖注册模块
        /// </summary>
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            #region 底层配置
            builder.Register(c => (new HttpContextWrapper(HttpContext.Current) as HttpContextBase));
            builder.Register(c => c.Resolve<HttpContextBase>().Request).As<HttpRequestBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response).As<HttpResponseBase>().InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server).As<HttpServerUtilityBase>().InstancePerLifetimeScope();

            RegisterWithTye<IBaseService>(builder);
            //通用的数据库增删改查访问dao
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerLifetimeScope();
            builder.RegisterControllers(typeFinder.GetAssemblies().ToArray());
            builder.RegisterType<PageHeadBuilder>().As<IPageHeadBuilder>().InstancePerLifetimeScope();
            //缓存
            builder.RegisterType<CacheFactory>().SingleInstance();

            builder.RegisterType<JobManager>().SingleInstance();
            #endregion

            builder.RegisterType<WorkContext.WorkContext>().As<IWorkContext>().InstancePerLifetimeScope();
            builder.RegisterType<AdminRepository>().As<IAdminRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PermissionRepository>().As<IPermissionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyStrutsRepository>().As<ICompanyStrutsRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ClassRepository>().As<IClassRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ContextRepository>().As<IContextRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerLifetimeScope();

        }

        /// <summary>
        /// 根据特定的类型进行批量注册
        /// </summary>
        /// <typeparam name="TRegisterType"></typeparam>
        /// <param name="builder"></param>
        private void RegisterWithTye<TRegisterType>(ContainerBuilder builder)
        {
            var typeFinder = new WebAppTypeFinder();
            var drConfigTypes = typeFinder.FindClassesOfType<TRegisterType>();
            foreach (Type drConfigType in drConfigTypes)
            {
                builder.RegisterType(drConfigType).InstancePerLifetimeScope();
            }
        }


        public int Order => 0;
    }
}