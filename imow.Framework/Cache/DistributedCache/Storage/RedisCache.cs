using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using imow.Framework.Cache.DistributedCache.Compress;
using Newtonsoft.Json;
using Imow.Framework.Cache.DistributedCache.SystemRuntime;
using Imow.Framework.Extensions;


namespace Imow.Framework.Cache.DistributedCache.Storage
{
    /*
     * Redis存储
     *序列化方面未来需要重写，并适应不同的存储方式；并提高序列化性能
     */
    public class RedisCache : BaseCache
    {
        private Redis.RedisDb redisdb = null;

        public override T GetValue<T>()
        {
            return Catch(() =>
            {
                bool isstring = typeof(T) == typeof(string);
                var bs = redisdb.getValueByte(_key);
                if (bs == null)
                    return null;
                string s = Encoding.UTF8.GetString(bs);
                if (s == "")//约定""为null的情况。
                    return null;
                try
                {
                    s = CompressFactory.DecompressString(this.Config.Compress, s);
                    return isstring ? s as T : JsonConvert.DeserializeObject<T>(s);
                }
                catch (Exception exp)
                {
                    throw new DistributedCacheSerializationException(s.NullToEmpty(), exp);
                }
            });
        }

        public override bool SetValue<T>(T value, TimeSpan expiretime)
        {
            return Catch(() =>
            {
                bool isstring = typeof(T) == typeof(string);

                string s = null;
                try
                {
                    s = (value == null ? "" : (isstring ? value.ToString() : JsonConvert.SerializeObject(value)));//约定""为null的情况。
                }
                catch (Exception exp)
                {
                    throw new DistributedCacheSerializationException(typeof(T), exp);
                }

                s = CompressFactory.CompressString(this.Config.Compress, s);
                return redisdb.SetValue(_key, Encoding.UTF8.GetBytes(s), expiretime);
            });

        }


        public override bool SetValue<T>(T value)
        {
            return Catch(() =>
            {
                bool isstring = typeof(T) == typeof(string);

                string s = null;
                try
                {
                    s = (value == null ? "" : (isstring ? value.ToString() : JsonConvert.SerializeObject(value)));//约定""为null的情况。
                }
                catch (Exception exp)
                {
                    throw new DistributedCacheSerializationException(typeof(T), exp);
                }

                s = CompressFactory.CompressString(this.Config.Compress, s);
                return redisdb.SetValue(_key, Encoding.UTF8.GetBytes(s));
            });

        }

        public override void Delete()
        {
            Catch(() =>
            {
                redisdb.Delete(_key);
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
                RedisCacheConfig serverconfig = this.Config as RedisCacheConfig;
                var manager = new Imow.Framework.Redis.RedisManager();
                redisdb = manager.GetPoolClient(serverconfig.ToRedisConfig());
                return 1;
            });
        }

        private T Catch<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (ServiceStack.Redis.RedisException e)
            {
                throw new DistributedCacheConnectException(e.Message, e);
            }
            catch (Newtonsoft.Json.JsonException e2)
            {
                throw new DistributedCacheSerializationException(e2);
            }

        }

        public override void Dispose()
        {
            if (redisdb != null)
            {
                redisdb.Dispose();
                redisdb = null;
            }
        }
    }

    public class RedisCacheConfig : BaseConfig
    {

        private string _config;

        public override void Parse(string config)
        {
            _config = config;
        }


        public string ToRedisConfig()
        {
            return _config;
        }
    }

}
