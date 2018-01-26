using System.Collections.Generic;

namespace imow.Framework.Cache.DistributedCache.LoadBalance
{
    public class LoadBalanceFactory
    {
        public static string ChooseServer(List<string> serviceconfiglist, string key) 
        {
            return new HashBalance().ChooseServer(serviceconfiglist, key);
        }
    }
}
