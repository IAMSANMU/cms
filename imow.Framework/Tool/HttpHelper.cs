using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace imow.Framework.Tool
{
   public  class HttpHelper
    {
       public static string HttpGet(string url,string paramStr)
        {
            //创建请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?" + paramStr);

            //GET请求
            request.Method = "GET";
            request.ReadWriteTimeout = 5000;
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

            //返回内容
            string retString = myStreamReader.ReadToEnd();
            return retString;
        }


    }
}
