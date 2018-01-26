using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imow.Framework.Cache.DistributedCache.Storage
{
    public class BaseCache:IDisposable
    {
        public virtual BaseConfig Config { get; set; }
        protected string _key;

        public virtual T GetValue<T>() where T:class
        {
            return default(T);
        }

        public virtual void Init()
        {
            
        }

        public virtual bool SetValue<T>(T value, TimeSpan expiretime)
        {
            return false;
        }

        public virtual bool SetValue<T>(T value)
        {
            return false;
        }

        public virtual void OpenConn(string key)
        {
 
        }

        public virtual void Delete()
        {
        }

        public virtual void Dispose()
        {
 
        }
    }
}
