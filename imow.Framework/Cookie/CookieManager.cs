using System;
using System.Web;
using imow.Core.cookieModel;
using Imow.Framework.Engine;

namespace imow.Framework.Cookie
{
    public class CookieManager<T> where T : class, ICookieModel
    {
        private readonly string _cookieKey;
        private readonly HttpContextBase _httpContextBase;

        public CookieManager(HttpContextBase httpContextBase)
        {
            _httpContextBase = httpContextBase;
            _cookieKey = ImowEngineContext.Current.Resolve<T>().GetCookieKey();
        }


        public HttpCookie Get()
        {
            return _httpContextBase.Request.Cookies[_cookieKey];
        }

        public bool Exists()
        {
            var cookie = Get();
            return cookie != null && !String.IsNullOrEmpty(cookie.Value);
        }


        public void Remove()
        {
            var cookie = Get();
            if (cookie != null)
            {
                cookie.Values.Clear();
                cookie.Expires = DateTime.Now.AddYears(-100);
                _httpContextBase.Response.Cookies.Add(cookie);
                _httpContextBase.Request.Cookies.Add(cookie);
            }
        }


        public void SetCookie(string value)
        {
            var cookie = new HttpCookie(_cookieKey);
            cookie.Value = value;
            Remove();
            _httpContextBase.Response.Cookies.Add(cookie);
            _httpContextBase.Request.Cookies.Add(cookie);
        }

        public void SetCookie(bool httoOnly, string path, string value, bool secure, string domain)
        {
            var cookie = new HttpCookie(_cookieKey);
            cookie.HttpOnly = httoOnly;
            cookie.Path = path;
            cookie.Value = value;
            cookie.Domain = domain;
            cookie.Secure = secure;
            Remove();
            _httpContextBase.Response.Cookies.Add(cookie);
            _httpContextBase.Request.Cookies.Add(cookie);
        }

        public void SetCookie(string value, DateTime expires)
        {
            var cookie = new HttpCookie(_cookieKey);
            cookie.Value = value;
            cookie.Expires = expires;
            Remove();
            _httpContextBase.Response.Cookies.Add(cookie);
            _httpContextBase.Request.Cookies.Add(cookie);
        }
    }
}