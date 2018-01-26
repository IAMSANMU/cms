using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imow.Framework.Cache.DistributedCache.SystemRuntime
{
    public enum EnumCacheType
    {
        /// <summary>
        /// 本地缓存,(可靠性较差,优点是不依赖任何别的东西)
        /// </summary>
        LocalMemory,
        /// <summary>
        /// Redis 
        /// 数据存内存,适合内存大小范围内大量缓存。（若是频繁失效的缓存数据，大量热点数据，建议使用redis）
        /// </summary>
        Redis,
        /// <summary>
        /// SSDB
        /// 数据热点存内存，大量数据存磁盘。（若是命中率较低，命中热点数据，大量冷数据，建议使用ssdb）
        /// </summary>
        SSDB,
        /// <summary>
        /// Memcached
        /// </summary>
        Memcached,
    }
}
