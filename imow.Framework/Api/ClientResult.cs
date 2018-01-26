namespace imow.Framework.Api
{
    /// <summary>
    /// 与客户端通信协议
    /// </summary>
    public class ClientResult<T>
    {
        /// <summary>请求回复状态码</summary>
        public int code { get; set; }
        /// <summary>请求是存成功</summary>
        public bool success
        {
            get
            {
                return code > 0;
            }
        }

        /// <summary>
        /// 返回的对象实体
        /// </summary>
        public T data { get; set; }

        /// <summary>
        /// 请求返回的信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 返回结果的列表数
        /// </summary>
        public long total { get; set; }

        /// <summary>
        /// 服务器时间 （UTCNow - 1970-01-01）
        /// </summary>
        public long servertime { get; set; }

        /// <summary>
        /// 客户端返回的文本内容
        /// </summary>
        public string responsetext { get; set; }
    }
}