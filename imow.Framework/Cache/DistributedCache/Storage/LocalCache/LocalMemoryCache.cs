using System;
using System.Runtime.Caching;
using System.Runtime.Remoting.Messaging;
using Imow.Framework.Cache.DistributedCache.SystemRuntime;

namespace Imow.Framework.Cache.DistributedCache.Storage.LocalCache
{
    public class LocalMemoryCache : BaseCache
    {
        private ObjectCache _cache;
        protected string _key;

        public override T GetValue<T>()
        {
            return Catch(() =>
            {
                T result = null;
                var val = _cache[_key];
                if (val != null) result = val as T;
                return result;
            });

        }

        public override bool SetValue<T>(T value, TimeSpan expiretime)
        {
            return Catch(() =>
            {
                Delete();
                return _cache.Add(_key, value, new DateTimeOffset(DateTime.Now.AddSeconds(expiretime.TotalSeconds)));
            });

        }

        public override bool SetValue<T>(T value)
        {
            return Catch(() =>
            {
                Delete();
                _cache[_key] = value;
                return true;
            });
        }

        public override void Delete()
        {
            Catch(() =>
            {
                _cache.Remove(_key);
                return 1;
            });
        }

        public override void OpenConn(string key)
        {
            Catch(() =>
            {
                _key = key;
                return 1;
            });
        }

        public override void Init()
        {
            Catch(() =>
            {
                _cache = MemoryCache.Default;
                return true;
            });
        }

        private T Catch<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (System.Net.Sockets.SocketException e)
            {
                throw new DistributedCacheConnectException(e.Message, e);
            }
            catch (Newtonsoft.Json.JsonException e2)
            {
                throw new DistributedCacheSerializationException(e2);
            }
            catch (Enyim.Caching.Memcached.MemcachedClientException e3)
            {
                throw new DistributedCacheConnectException(e3.Message, e3);
            }
        }


    }
}