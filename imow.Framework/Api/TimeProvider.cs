using System;

namespace imow.Framework.Api
{
    /// <summary>
    /// 用于获取标准时间
    /// 未来需要解决重放攻击，需要加入网络标准时间
    /// </summary>
    public class TimeProvider
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public long GetTimeStamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}
