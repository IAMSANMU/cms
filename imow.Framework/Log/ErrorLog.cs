using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using Imow.Framework.BaseService.Monitor.SystemRuntime;
using Imow.Framework.Engine;
using Imow.Framework.Extensions;
using Imow.Framework.Tool;
using imow.Core.config;

namespace Imow.Framework.Log
{
    public class ErrorLogInfo
    {

        /// <summary>
        /// 日志类型:一般非正常错误,系统级严重错误
        /// </summary>
        public EnumErrorLogType logtype { get; set; }

        /// <summary>
        /// 日志唯一标示(简短的方法名或者url,便于归类)
        /// </summary>
        public string logtag { get; set; }

        public string msg { get; set; }

        /// <summary>
        /// 相关开发人员
        /// </summary>
        public string developer { get; set; }

        public string remark { get; set; }
    }

    /// <summary>
    /// 简易错误日志
    /// </summary>
    public class ErrorLog
    {
        public static string FilePath = System.AppDomain.CurrentDomain.BaseDirectory.Trim('\\') + "\\" + "error.log";
        private static string CurrentIp = "";

        static ErrorLog()
        {
        }
        /// <summary>
        /// 错误日志记录
        /// </summary>
        /// <param name="errormsg"></param>
        /// <param name="msg"></param>
        public static void Write(string errormsg, Exception msg)
        {
            Write(new ErrorLogInfo()
            {
                developer = "",
                logtag = "",
                msg = errormsg,
                logtype = EnumErrorLogType.CommonError,
                remark = "",
            }, msg);

        }

        public static void Write(ErrorLogInfo errormsnginfo, Exception exp)
        {
            var imowConfig = ImowEngineContext.Current.ResolveConfig<ImowConfig>();
            try
            {
                if (errormsnginfo == null)
                    return;
                if (exp == null)
                    exp = new Exception();
                //增加参数错误记录
                string requestparamsinfo = "";
                if (System.Web.HttpContext.Current != null)
                {
                    try
                    {
                        foreach (var k in System.Web.HttpContext.Current.Request.RequestParams())
                        {
                            var v = System.Web.HttpContext.Current.Request.RequestParamValue(k);
                            if (v != null)
                            {
                                requestparamsinfo += string.Format("[{0}:{1}]", k, v);
                            }
                        }
                    }
                    catch { }
                }

                string ServerIp = "";
                if (exp is SqlException)
                {
                    ServerIp = ((SqlException)(exp)).Server;
                }

                if (string.IsNullOrWhiteSpace(CurrentIp))
                {
                    IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
                    IPAddress ipAddr = ipHost.AddressList[0];
                    CurrentIp = ipAddr.ToString();
                }
                if (string.IsNullOrWhiteSpace(errormsnginfo.logtag))
                {
                    string url = (System.Web.HttpContext.Current != null ? (System.Web.HttpContext.Current.Request.RawUrl.Tostring().SubString2(90)) : "");
                    if (url != "")
                    {
                        errormsnginfo.logtag = url;
                    }
                    else
                    {
                        errormsnginfo.logtag = errormsnginfo.msg.SubString2(10);
                    }
                }

                string splitline = "******************************************************************************************************************************************************";
                string message = //splitline + "\r\n" +
                                 "【时间】" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "\r\n" +
                                 "【服务器】" + CurrentIp + "\r\n" +
                                 "【数据库】" + ServerIp + "\r\n" +
                                 "【补充信息】" + errormsnginfo.msg + "\r\n" +
                                 "【原始信息】" + exp.Message + "\r\n" +
                                 "【request参数】" + requestparamsinfo + "\r\n" +
                                 "【堆栈打印】" + exp.StackTrace + "\r\n" +
                                 splitline + "\r\n";
                if (true)
                {
                    System.Diagnostics.Debug.WriteLine(message);
                }

                if (imowConfig.IsWriteErrorLogToLocalFile)
                {
                    string filepath = System.AppDomain.CurrentDomain.BaseDirectory.Trim('\\') + "\\errorlog\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".error.log";
                    IOHelper.CreateDirectory(filepath);
                    System.IO.File.AppendAllText(filepath, message);
                }





            }
            catch { }
        }
    }

}