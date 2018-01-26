using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Imow.Framework.BaseService.Monitor.SystemRuntime;
using Imow.Framework.Engine;
using Imow.Framework.Extensions;
using Imow.Framework.Tool;
using imow.Core.config;

namespace Imow.Framework.Log
{
    public class CommonLogInfo
    {

        /// <summary>
        /// 日志类型:一般非正常错误,系统级严重错误,一般业务日志,系统日志
        /// </summary>
        public EnumCommonLogType logtype { get; set; }

        /// <summary>
        /// 日志唯一标示(简短的方法名或者url,便于归类)
        /// </summary>
        public string logtag { get; set; }

        public string msg { get; set; }
    }
    /// <summary>
    /// 一般日志记录类
    /// </summary>
    public class CommLog
    {
        //public static string FilePath = System.AppDomain.CurrentDomain.BaseDirectory.Trim('\\') + "\\" + "error.log";
        private static string CurrentIp = "";

        static CommLog()
        {
        }

        /// <summary>
        /// 日常一般日志记录
        /// </summary>
        /// <param name="msg"></param>
        public static void Write(string msg)
        {
            Write(new CommonLogInfo() { logtype = EnumCommonLogType.CommonLog, logtag = "", msg = msg });
        }
        /// <summary>
        /// 日常一般日志记录
        /// </summary>
        /// <param name="msg"></param>
        public static void Write(CommonLogInfo commonloginfo)
        {
           var imowConfig= ImowEngineContext.Current.ResolveConfig<ImowConfig>();
            try
            {
                if (string.IsNullOrWhiteSpace(CurrentIp))
                {
                    IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
                    IPAddress ipAddr = ipHost.AddressList[0];
                    CurrentIp = ipAddr.ToString();
                }
                if (string.IsNullOrWhiteSpace(commonloginfo.logtag))
                {
                    string url = (System.Web.HttpContext.Current != null ? (System.Web.HttpContext.Current.Request.RawUrl.Tostring().SubString2(90)) : "");
                    if (url != "")
                    {
                        commonloginfo.logtag = url;
                    }
                    else
                    {
                        commonloginfo.logtag = commonloginfo.msg.SubString2(10);
                    }
                }
                if (imowConfig.IsWriteCommonLogToLocalFile)
                {
                    string splitline = "******************************************************************************************************************************************************";
                    string message = //splitline + "\r\n" +
                                     "【时间】" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "\r\n" +
                                     "【内容】" + commonloginfo.msg + "\r\n" +
                                      splitline + "\r\n";

                    string filepath = System.AppDomain.CurrentDomain.BaseDirectory.Trim('\\') + "\\commonlog\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".common.log";
                    IOHelper.CreateDirectory(filepath);
                    System.IO.File.AppendAllText(filepath, message);
                }
         }
            catch { }
        }
    }
}
