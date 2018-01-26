using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using imow.Core.config;
using Newtonsoft.Json;
using Imow.Framework.Cache.DistributedCache.Storage;
using Imow.Framework.Cache.DistributedCache.Storage.LocalCache;
using Imow.Framework.Cache.DistributedCache.SystemRuntime;
using Imow.Framework.Engine;
using Imow.Framework.Extensions;
using Imow.Framework.Redis;

namespace Imow.Framework.Cache.DistributedCache.Storage
{
    /*
     * 缓存代理实现工厂
     */
    public class CacheFactory
    {

        private string _serverconfig;
        private EnumCacheType _cacheType;
        private BaseCache _baseCache;
        //todo:最好能有一台membercached,这个缓存以前也没用过 也不知道是什么鬼东西
        /// <summary>
        /// 设置默认为LocalMemory
        /// </summary>
        public CacheFactory() : this(EnumCacheType.LocalMemory)
        {
        }

        public CacheFactory(EnumCacheType cacheType)
        {
            var cacheConfigInfo = ImowEngineContext.Current.ResolveConfig<CacheConfigInfo>();
            _cacheType = cacheType;
            switch (_cacheType)
            {
                case EnumCacheType.Memcached:
                    _serverconfig = cacheConfigInfo.Membercache;
                    break;
                case EnumCacheType.Redis:
                    _serverconfig = cacheConfigInfo.KeyRedisServer;
                    break;
                case EnumCacheType.SSDB:
                    _serverconfig = cacheConfigInfo.SSDB;
                    break;
            }
        }

        private BaseCache GetCache()
        {
            if (_baseCache == null)
            {
                BaseCache c = null;
                if (_cacheType == EnumCacheType.LocalMemory)
                {
                    c = new LocalMemoryCache();
                }
                else if (_cacheType == EnumCacheType.Memcached)
                {
                    c = new MemcachedCache();
                    c.Config = new MemcachedCacheConfig();
                    c.Config.Parse(_serverconfig);
                }
                else if (_cacheType == EnumCacheType.Redis)
                {
                    c = new RedisCache();
                    c.Config = new RedisCacheConfig();
                    c.Config.Parse(_serverconfig);
                }
                else if (_cacheType == EnumCacheType.SSDB)
                {
                    c = new SSDBCache();
                    c.Config = new SSDBCacheConfig();
                    c.Config.Parse(_serverconfig);
                }
                if (c == null)
                {
                    throw new DistributedCacheException("未识别的服务器配置信息");
                }
                else
                {
                    _baseCache = c;
                }
            }
            _baseCache.Init();
            return _baseCache;

        }

        public T GetOrSetValue<T>(string key, Func<T> action) where T : class
        {
            return GetOrSetValue(key, TimeSpan.MaxValue, action);
        }

        public T GetOrSetValue<T>(string key, TimeSpan expiretime, Func<T> action) where T : class
        {
            bool hasexpiretime = expiretime < TimeSpan.MaxValue;
            if (expiretime < TimeSpan.FromSeconds(1))
                throw new DistributedCacheException("过期时间不得少于1秒");
            using (var cache = GetCache())
            {
                try
                {
                    cache.OpenConn(key);
                    T r = null;
                    try
                    {
                        r = cache.GetValue<T>();
                        if (r != null) { return r; }//假如key未过期
                    }
                    catch (DistributedCacheSerializationException exp)
                    {
                        //假如内存的序列化内容和实际的序列化结果不一致的情况,则重新序列化覆盖之,并检查反序列情况
                        T r3 = action();
                        var success = hasexpiretime? cache.SetValue(r3, expiretime) : cache.SetValue(r3);
                        if (success == true)
                        {
                            try
                            {
                                var v2 = cache.GetValue<T>();
                                return v2;
                            }
                            catch { }
                        }
                        Imow.Framework.Log.ErrorLog.Write("DistributedCache序列化出错", exp);
                        throw exp;
                    }

                    if (r == null)
                    {
                        //假如key已经过期
                        T v4 = action();
                        var success4 = hasexpiretime ? cache.SetValue(v4, expiretime) : cache.SetValue(v4);
                        if (success4 == true)
                        {
                            return v4;
                        }
                    }
                }
                catch (DistributedCacheConnectException exp)
                {
                    Imow.Framework.Log.ErrorLog.Write("DistributedCache连接出错", exp);
                    //假如缓存无法连接或连接失败
                    T v4 = action();
                    return v4;
                }
                Imow.Framework.Log.ErrorLog.Write("DistributedCache未知严重错误", new Exception());
                throw new DistributedCache.SystemRuntime.DistributedCacheException("DistributedCache未知严重错误");
            }
        }

        public T GetValue<T>(string key) where T : class
        {
            using (var cache = GetCache())
            {
                try
                {
                    cache.OpenConn(key);
                    T r = null;
                    try
                    {
                        r = cache.GetValue<T>();
                        if (r != null) { return r; }//假如key未过期
                    }
                    catch (DistributedCacheSerializationException exp)
                    {
                        Imow.Framework.Log.ErrorLog.Write("DistributedCache序列化出错", exp);
                        throw exp;
                    }
                    return r;
                }
                catch (DistributedCacheConnectException exp)
                {
                    Imow.Framework.Log.ErrorLog.Write("DistributedCache连接出错", exp);
                    throw new DistributedCache.SystemRuntime.DistributedCacheException("DistributedCache连接出错");
                }
            }
        }

        public void SetValue<T>(string key, T value) where T : class
        {
            using (var cache = GetCache())
            {
                cache.OpenConn(key);
                cache.SetValue(value);
            }
        }

        public void SetValue<T>(string key, T value, TimeSpan expiretime) where T : class
        {
            using (var cache = GetCache())
            {
                cache.OpenConn(key);
                cache.SetValue(value, expiretime);
            }
        }


        public void Delete(string[] keys)
        {
            foreach (var key in keys)
            {
                using (var cache = GetCache())
                {
                    cache.OpenConn(key);
                    cache.Delete();
                }
            }
        }



    }
}
