using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Imow.Framework.Extensions;
using Imow.Framework.Tool;

namespace Imow.Framework.Redis
{
    /// <summary>
    /// Redis管理类
    /// </summary>
    public class RedisManager
    {
        private static Dictionary<int, PooledRedisClientManager> ConnPools = new Dictionary<int, PooledRedisClientManager>();
        private static object _connpoollock = new object();


        private PooledRedisClientManager GetPool(RedisConfig config)
        {
            int hash = config.GetHashCode();
            if (ConnPools.ContainsKey(hash))
                return ConnPools[hash];
            else
            {
                lock (_connpoollock)
                {
                    if (!ConnPools.ContainsKey(hash))
                    {
                        var pool = new PooledRedisClientManager(config.ReadWriteHosts, config.ReadOnlyHosts, new RedisClientManagerConfig
                        {
                            MaxWritePoolSize = config.MaxWritePoolSize,//“写”链接池链接数
                            MaxReadPoolSize = config.MaxReadPoolSize,//“写”链接池链接数
                            AutoStart = config.AutoStart,
                        });
                        ConnPools.Add(hash, pool);
                    }
                    return ConnPools[hash];
                }
            }
        }

        /// <summary>
        /// 获取连接池客户端
        /// </summary>
        /// <returns></returns>
        public RedisDb GetPoolClient(string redisHost)
        {
            var redisclient = (RedisClient)GetPool(new RedisConfig(redisHost)).GetClient();
            RedisDb db = new RedisDb(redisclient);
            return db;
        } 
        
        /// <summary>
        /// 获取连接池客户端
        /// </summary>
        /// <returns></returns>
        public RedisDb GetPoolClient(string redisHost, int maxWritePoolSize, int maxReadPoolSize)
        {
            var redisclient = (RedisClient)GetPool(new RedisConfig(redisHost) { MaxReadPoolSize = maxReadPoolSize, MaxWritePoolSize = maxWritePoolSize }).GetClient();
            RedisDb db = new RedisDb(redisclient);
            return db;
        }

        /// <summary>
        /// 独立创建一个客户端
        /// </summary>
        /// <param name="hostip"></param>
        /// <param name="port"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [Obsolete("不建议在项目中使用,请使用连接池")]
        public RedisDb CreateClient(string hostip, int port, string password)
        {
            var client = new RedisClient(hostip, port);
            if (!string.IsNullOrEmpty(password))
            {
                client.Password = password;
            }
            RedisDb db = new RedisDb(client);
            return db;
        }
    }

    public class RedisConfig : RedisClientManagerConfig
    {
        public string[] ReadWriteHosts { get; set; }
        public string[] ReadOnlyHosts { get; set; }
        public RedisConfig(string redisHost)
            : base()
        {
            string[] readWriteHosts = null;
            string[] readOnlyHosts = null;
            MaxWritePoolSize = 5; MaxReadPoolSize = 5; AutoStart = true;
            foreach (string param in StringHelper.SplitString(redisHost, "|"))
            {
                string[] paramDetails = StringHelper.SplitString(param, "=");
                string paramName = paramDetails[0];
                string paramValue = paramDetails[1];

                if (paramName == "readwritehosts")
                {
                    readWriteHosts = StringHelper.SplitString(paramValue);
                }
                else if (paramName == "readonlyhosts")
                {
                    readOnlyHosts = StringHelper.SplitString(paramValue);
                }
                else if (paramName == "maxreadpoolsize")
                {
                    this.MaxReadPoolSize = paramValue.Toint();
                }
                else if (paramName == "maxwritepoolsize")
                {
                    this.MaxWritePoolSize = paramValue.Toint();
                }
            }
            if (this.MaxWritePoolSize == 0 || this.MaxReadPoolSize == 0)
            {
                this.MaxWritePoolSize = readWriteHosts.Length * 10;
                this.MaxReadPoolSize = readOnlyHosts.Length * 10;
            }
            ReadWriteHosts = readWriteHosts;
            ReadOnlyHosts = readOnlyHosts;
        }
        public override int GetHashCode()
        {
            string hash = "";
            if (ReadWriteHosts != null)
                hash += string.Join(",", ReadWriteHosts);
            if (ReadOnlyHosts != null)
                hash += string.Join(",", ReadOnlyHosts);
            hash += "" + MaxWritePoolSize + MaxReadPoolSize + AutoStart;
            return hash.GetHashCode();
        }
    }
}
