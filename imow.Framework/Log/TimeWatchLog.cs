using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using Imow.Framework.BaseService.Monitor.SystemRuntime;
using Imow.Framework.Engine;
using Imow.Framework.Extensions;
using Imow.Framework.Tool;
using imow.Core.config;

namespace Imow.Framework.Log
{
    public class TimeWatchLogInfo
    {

        /// <summary>
        /// 耗时日志类型：普通日志=0，api接口日志=1，sql日志=2
        /// </summary>
        public EnumTimeWatchLogType logtype { get; set; }

        /// <summary>
        /// 日志标识,sql类型则为sql哈希 string.hash(),api类型则为url,普通日志则为方法名
        /// </summary>
        public int logtag { get; set; }

        /// <summary>
        /// 当前url
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 当前信息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 来源ip(代码执行ip)
        /// </summary>
        public string fromip { get; set; }

        /// <summary>
        /// sqlip地址
        /// </summary>
        public string sqlip { get; set; }

        /// <summary>
        /// 其他记录标记信息
        /// </summary>
        public string remark { get; set; }
    }
    /// <summary>
    /// 简易耗时打印
    /// </summary>
    public class TimeWatchLog
    {
        public static string FilePath = System.AppDomain.CurrentDomain.BaseDirectory.Trim('\\') + "\\" + "timewatch.log";
        private static string CurrentIp { get; set; }
        public static bool IfWatch = false;
        public DateTime StartTime;
        static TimeWatchLog()
        {

        }
        public TimeWatchLog()
        {
            Start();
        }
        /// <summary>
        /// 开启
        /// </summary>
        public void Start()
        {
            StartTime = DateTime.Now;
        }

        public double Debug()
        {
            return (DateTime.Now - StartTime).TotalSeconds;
        }

        public static double Debug(Action action)
        {
            var startTime = DateTime.Now;
            action.Invoke();
            return (DateTime.Now - startTime).TotalSeconds;
        }

        /// <summary>
        /// 写普通耗时日志(url哈希为logtag)
        /// </summary>
        /// <param name="msg"></param>
        public void Write(string msg)
        {
            EnumTimeWatchLogType type = EnumTimeWatchLogType.Common;
            string url = (System.Web.HttpContext.Current != null ? (System.Web.HttpContext.Current.Request.RawUrl.ToString().SubString2(90)) : "");
            Write(new TimeWatchLogInfo()
            {
                fromip = "",
                logtype = type,
                logtag = url.GetHashCode(),
                url=url,
                msg = msg,
                remark = "",
                sqlip = "",
            });
        }

        /// <summary>
        /// 写普通耗时日志(方法名哈希为logtag)
        /// </summary>
        /// <param name="msg"></param>
        public void Write(string methodname,string msg)
        {
            EnumTimeWatchLogType type = EnumTimeWatchLogType.Common;
            Write(new TimeWatchLogInfo()
            {
                fromip = "",
                logtype = type,
                logtag = methodname.GetHashCode(),
                url = methodname.SubString2(90),
                msg = msg,
                remark = "",
                sqlip = "",
            });
        }

        /// <summary>
        /// 写耗时日志
        /// </summary>
        /// <param name="paramBinds"></param>
        //[Obsolete("此方法已经过期,后续将被逐步替换,请谨慎使用")]    梅应祥注释
        //public void Write(ParamBinds paramBinds)
        //{
        //    EnumTimeWatchLogType type = EnumTimeWatchLogType.Common; string remark = "";
        //    string url = (System.Web.HttpContext.Current != null ? (System.Web.HttpContext.Current.Request.RawUrl.ToString().SubString2(90)) : "");
        //    if (paramBinds.RequseUrl.StartsWith(url) && url != "")
        //    { 
        //        type = EnumTimeWatchLogType.ApiUrl;
        //        if(!string.IsNullOrWhiteSpace(paramBinds.Dyduniquetag))
        //            remark += "Dyduniquetag:" + paramBinds.Dyduniquetag+",";
        //        if(!string.IsNullOrWhiteSpace(paramBinds.Dydphoneversion))
        //            remark += "Dydphoneversion:" + paramBinds.Dydphoneversion+",";
        //        if(!string.IsNullOrWhiteSpace(paramBinds.Dydphonedevice))
        //            remark += "Dydphonedevice:" + paramBinds.Dydphonedevice+",";
        //        if (!string.IsNullOrWhiteSpace(paramBinds.Dydtoken))
        //            remark += "Dydtoken:" + paramBinds.Dydtoken + ",";
                
        //        remark = remark.TrimEnd(',');
        //    }
           
        //    Write(new TimeWatchLogInfo() {
        //        fromip = "",
        //        logtype = type,
        //        logtag = url.GetHashCode(),
        //        url = url,
        //        msg = paramBinds.RequseUrl,
        //        remark = remark,
        //        sqlip = paramBinds.ServerIp,
        //    });
           
        //}

        public void Write(TimeWatchLogInfo timewatchloginfo)
        {
            var imowConfig = ImowEngineContext.Current.ResolveConfig<ImowConfig>();
            try
            {

                if (imowConfig.IsWriteTimeWatchLogToLocalFile)
                {
                    string filepath = System.AppDomain.CurrentDomain.BaseDirectory.Trim('\\') + "\\timewatchlog\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".time.log";
                    IOHelper.CreateDirectory(filepath);

                    System.IO.File.AppendAllText(filepath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " " + timewatchloginfo.msg + "-耗时: " + (DateTime.Now - StartTime).TotalSeconds + " s \r\n");
                }

            }
            catch { }

//#if DEBUG
//            string info = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " " + timewatchloginfo.msg + "-耗时: " + (DateTime.Now - StartTime).TotalSeconds + " s \r\n";
//            Debug.WriteLine(info);
//#else
//#endif
        }

    }
}