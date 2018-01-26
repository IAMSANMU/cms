namespace imow.Framework.Api
{
    /// <summary>
    /// 服务器端返回通信协议
    /// </summary>
    public class ServiceResult
    {
        /// <summary>
        /// 返回值 1成功 -1失败 <0表示错误码 >0表示成功码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 消息返回
        /// </summary>
        private string _msg = string.Empty;
        public string msg { get { return _msg; } set { _msg = value; } }
        /// <summary>
        /// 接受对象
        /// </summary>
        public object data { get; set; }
        /// <summary>
        /// 如列表，列表总数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 服务器时间 （UTCNow - 1970-01-01）
        /// </summary>
        public long servertime { get { return new TimeProvider().GetTimeStamp(); } }
    }
}
