using System;
using System.Collections.Generic;
using System.Web;
using Imow.Framework.Serialization;

namespace imow.Framework.Api
{
    /// <summary>
    /// api帮助类
    /// </summary>
    public class ApiHelper
    {

        public static ClientResult<T> Post<T>(string url, Dictionary<string, Object> param)
        {
            Dictionary<string, string> dataDic = GetDictionaryOfParam(param);
            return HttpPost<T>(url, dataDic, true);
        }
        /// <summary>
        /// 获取api结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        /// 
        public static ClientResult<T> Post<T>(string url, Object param,bool isJosn= true)
        {
            Dictionary<string, string> dataDic = GetDictionaryOfParam(param);

            return HttpPost<T>(url, dataDic, isJosn);
        }

        public static ClientResult<T> PostWithSign<T>(string url, string appsecret, Object param)
        {
            Dictionary<string, string> dataDic = GetDictionaryOfParam(param);
            string sign = new SignProvider().CreateSign(dataDic, appsecret);
            dataDic.Add(SignProvider.signKey, sign);

            return HttpPost<T>(url, dataDic,true);
        }

        private static Dictionary<string, string> GetDictionaryOfParam(Object param)
        {
            Dictionary<string, string> dataDic = new Dictionary<string, string>();
            foreach (var p in param.GetType().GetProperties())
            {
                var value = p.GetValue(param, null);
                string strValue = null;
                if (value != null)
                    strValue = value.ToString();
                dataDic.Add(p.Name, strValue);
            }
            return dataDic;
        }

        private static Dictionary<string, string> GetDictionaryOfParam(Dictionary<string, object> param)
        {
            Dictionary<string, string> dataDic = new Dictionary<string, string>();
            foreach (var p in param)
            {
                var value = p.Value;
                string strValue = null;
                if (value != null)
                    strValue = value.ToString();
                dataDic.Add(p.Key, strValue);
            }
            return dataDic;
        }

        private static ClientResult<T> HttpPost<T>(string url, Dictionary<string, string> dataDic, bool isJson )
        {
            //可以记录api调用的网络耗时，便于优化以及一些错误日志的记录拦截
            //兼容后续版本的api通信机制
           
                //TimeWatchLog watch = new TimeWatchLog();//网络耗时打印
                string msg = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    msg = HttpContext.Current.Request.RawUrl.ToString();
                string clientResultText = null;
                try
                {
                     clientResultText = new HttpProvider().Post(url, dataDic);
                }
                catch (Exception exp)
                {
                    //ErrorLog.Write("api调用出错:", exp);
                    throw new Exception("api调用出错:" + exp.Message);
                }

                ClientResult<T> clientResult = null;
                try
                {
                    if (isJson)
                    {
                        clientResult = JsonProvider.Deserialize<ClientResult<T>>(clientResultText);
                    }
                    else
                    {
                        clientResult = new ClientResult<T>();
                        clientResult.data = new XmlProvider<T>().Deserialize(clientResultText);
                    }
                }
                catch (Exception e)
                {
                    clientResult=new ClientResult<T>();
                }
                clientResult.responsetext = clientResultText;

                return clientResult;
            }
          

    }
}
