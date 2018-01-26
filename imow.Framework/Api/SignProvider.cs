using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Imow.Framework.Tool;

namespace imow.Framework.Api
{
    /// <summary>
    /// 签名算法
    /// </summary>
    public class SignProvider
    {
        #region 参数
        /// <summary>
        /// 第一个分隔符
        /// </summary>
        public static readonly string splitCharOne = ":";

        /// <summary>
        /// 第二个分隔符
        /// </summary>
        public static readonly string splitCharTwo = ",";

        /// <summary>
        /// 签名字段
        /// </summary>
        public static readonly string signKey = "_sign";

        public static readonly string timestampKey = "timestamp";

        public static readonly string appsecretKey = "appsecret";

        #endregion

        /// <summary>
        /// 验证sign
        /// </summary>
        /// <param name="request"></param>
        /// <param name="err"></param>
        ///  <param name="intervalTime"></param>
        /// <returns></returns>
        public bool ValidateSign(HttpRequest request, string appSecret, out string err)
        {
            err = "";
            string sign = request.Params[signKey];
            if (string.IsNullOrWhiteSpace(sign))
            {
                err += "缺少签名" + signKey;
                return false;
            }
            /*用于规避重放攻击*/
            //long timestamp = LibConvert.StrToInt64(request.Form["timestamp"]);
            //if (ApiHelper.GetTimeStamp(DateTime.Now.AddMinutes(-intervalTime)) > timestamp)
            //{
            //    err = "请求时间超时";
            //    return false;
            //}

            Dictionary<string, string> signDic = new Dictionary<string, string>();

            string[] keys = request.Form.AllKeys;
            foreach (var m in keys)
            {
               signDic.Add(m, request.Form[m]);
            }

            string signnew = CreateSign(signDic, appSecret);

            if (sign != signnew)
            {
                err = "签名验证失败";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="parms">参数列表</param>
        /// <param name="appsecret">密钥</param>
        /// <returns></returns>
        public string CreateSign(Dictionary<string, string> param, string appsecret)
        {
            //抽取需要签名的参数
            Dictionary<string, string> signDic = new Dictionary<string, string>();
            foreach (var m in param)
            {
                if (!string.IsNullOrWhiteSpace(m.Key) && !m.Key.StartsWith("_"))//以下划线开头的参数和sign不参签名  
                {
                    signDic.Add(m.Key, m.Value);
                }
            }
            signDic.Add(appsecretKey,appsecret);

            StringBuilder signStr = new StringBuilder();
            //按照asc码升序排列
            foreach (var m in signDic.OrderBy(m => m.Key, StringComparer.Ordinal))
            {
                signStr.Append(m.Key).Append(splitCharOne).Append(m.Value).Append(splitCharTwo);
            }

            //去除最后一个多余的分隔符
            if (signStr.Length > 0)
            {
                signStr.Remove(signStr.Length - splitCharTwo.Length, splitCharTwo.Length);
            }

            string mySign = EncryptionHelper.En32MD5(signStr.ToString());
            return mySign;
        }
    }
}
