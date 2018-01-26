using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imow.Framework.Engine;
using imow.Core.config;

namespace Imow.Framework.Redis
{
    public class RedisHelper
    {
        public static long GetID(string key)
        {
            string redis = ImowEngineContext.Current.ResolveConfig<CacheConfigInfo>().KeyRedisServer;
            RedisManager pool = new RedisManager();
            using (var client = pool.GetPoolClient(redis))
            {
                return client.Incr(key);
            }
        }

        public static bool SetID(string key, long value)
        {

            string redis = ImowEngineContext.Current.ResolveConfig<CacheConfigInfo>().KeyRedisServer;
            RedisManager pool = new RedisManager();
            using (var client = pool.GetPoolClient(redis))
            {
                return client.SetValue<long>(key, value);
            }
        }

        public static bool Exists(string key)
        {
            string redis = ImowEngineContext.Current.ResolveConfig<CacheConfigInfo>().KeyRedisServer;
            RedisManager pool = new RedisManager();
            using (var client = pool.GetPoolClient(redis))
            {
                return !string.IsNullOrEmpty(client.getValueString(key));
            }
        }

        public static T GetValue<T>(string key)
        {
            string redis = ImowEngineContext.Current.ResolveConfig<CacheConfigInfo>().KeyRedisServer;
            RedisManager pool = new RedisManager();
            using (var client = pool.GetPoolClient(redis))
            {
                return client.GetValue<T>(key);
            }
        }

        public static bool SetValue<T>(string key, T value)
        {
            string redis = ImowEngineContext.Current.ResolveConfig<CacheConfigInfo>().KeyRedisServer;
            RedisManager pool = new RedisManager();
            using (var client = pool.GetPoolClient(redis))
            {
                return client.SetValue<T>(key, value);
            }
        }

    }
}
