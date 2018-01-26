using System;
using System.Collections.Generic;

namespace imow.Framework.Cache.DistributedCache.LoadBalance
{
    public class HashBalance : IBalance
    {
        /// <summary>
        /// hs算法把不同的key存入到不同的数据列表中去
        /// </summary>
        /// <param name="serviceconfiglist"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ChooseServer(List<string> serviceconfiglist, string key) 
        {
            if (serviceconfiglist == null||serviceconfiglist.Count==0)
                return null;
            return serviceconfiglist[Math.Abs(key.GetHashCode()) % serviceconfiglist.Count];
        }
    }
}
