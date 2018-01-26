using Autofac;
using Imow.Framework.Engine.TypeFinder;
namespace Imow.Framework.Engine.DependencyManager
{
    /// <summary>
    /// 遵守IDependencyRegistrar将会在applictionStart的时候进行注册
    /// Order越大会覆盖小的注册的模块.通用模块order为0
    /// </summary>
    public interface IDependencyRegistrar
    {
        void Register(ContainerBuilder builder, ITypeFinder typeFinder);

        int Order { get; }
    }
}
