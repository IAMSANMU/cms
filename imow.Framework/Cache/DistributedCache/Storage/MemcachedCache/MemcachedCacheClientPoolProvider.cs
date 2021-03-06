﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using ServiceStack;

namespace Imow.Framework.Cache.DistributedCache.Storage
{
    /// <summary>
    /// 客户端连接池
    /// </summary>
    public class MemcachedClientPoolProvider
    {
        private static Dictionary<MemcachedCacheConfig, MemcachedClient> _pools = new Dictionary<MemcachedCacheConfig, MemcachedClient>();
        private static object _lockpool = new object();

        public MemcachedClient GetPool(MemcachedCacheConfig config)
        {
            if (_pools.ContainsKey(config))
            {
                return _pools[config];
            }
            lock (_lockpool)
            {
                if (_pools.ContainsKey(config))
                {
                    return _pools[config];
                }
                //初始化缓存
                MemcachedClientConfiguration memConfig = new MemcachedClientConfiguration();
                IPAddress newaddress = Dns.GetHostByName(config.NetworkAddress).AddressList[0];//your_instanceid替换为你的OCS实例的ID
                IPEndPoint ipEndPoint = new IPEndPoint(newaddress, config.Port);

                // 配置文件 - ip
                memConfig.Servers.Add(ipEndPoint);
                // 配置文件 - 协议
                memConfig.Protocol = MemcachedProtocol.Binary;
                //如果没有账号密码就是非aliyunmembercache
                if (!config.UserName.IsNullOrEmpty() && !config.UserName.IsNullOrEmpty())
                {     
                    // 配置文件-权限
                    memConfig.Authentication.Type = typeof(PlainTextAuthenticator);
                    memConfig.Authentication.Parameters["zone"] = "";
                    memConfig.Authentication.Parameters["userName"] = config.UserName;
                    memConfig.Authentication.Parameters["password"] = config.Password;
                }
                //下面请根据实例的最大连接数进行设置
                memConfig.SocketPool.MaxPoolSize = config.MaxPoolSize;
                memConfig.SocketPool.MinPoolSize = config.MinPoolSize;
                memConfig.SocketPool.ConnectionTimeout = TimeSpan.FromMilliseconds(config.ConnectionTimeout);

                MemcachedClient pool = new MemcachedClient(memConfig);
                _pools.Add(config, pool);
                return _pools[config];

            }
        }

        public void Dispose()
        { }
    }
}
