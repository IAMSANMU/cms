using System.Collections.Generic;

namespace imow.Framework.Cache.DistributedCache.LoadBalance
{
    public interface IBalance
    {
        string ChooseServer(List<string> serviceconfiglist, string key);
    }
}
