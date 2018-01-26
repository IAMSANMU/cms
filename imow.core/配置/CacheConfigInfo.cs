using System;

namespace imow.Core.config
{
    [Serializable]
    public class CacheConfigInfo:IConfig
    {

        private string _membercache;

        public string Membercache
        {
            get { return _membercache; }
            set { _membercache = value; }
        }

        private string _aliyunMemcached;

        public string AliyunMemcached
        {
            get { return _aliyunMemcached; }
            set { _aliyunMemcached = value; }
        }

        private string _ssdb;

        public string SSDB
        {
            get { return _ssdb; }
            set { _ssdb = value; }
        }

        private string _keyRedisServer;

        public string KeyRedisServer
        {
            get { return _keyRedisServer; }
            set { _keyRedisServer = value; }
        }


        public string GetConfigPath()
        {
            return "/App_Data/Cache.config";
        }
    }
}