using System.Collections.Generic;
using System.Net.Http;

namespace imow.Framework.Api
{
    /// <summary>
    /// Http操作类库
    /// 包含Http的socket连接池，优化性能
    /// </summary>
    public class HttpProvider
    {
        public string Post(string url, Dictionary<string, string> param)
        {
            var content = new FormUrlEncodedContent(param) ;
            //此处未来需要添加HttpClient连接池,复用连接
            using (var client = new HttpClient())
            {
                var result = client.PostAsync(url, content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                return resultContent;
            }
        }
    }
}
