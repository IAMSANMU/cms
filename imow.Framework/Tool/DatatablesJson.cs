using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace imow.Framework.Tool
{
    /// <summary>
    /// 返回datagrid json 格式类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DatatablesJson<T>
    {
        [JsonProperty("recordsTotal")]
        public long RecordsTotal { get; set; }
        [JsonProperty("recordsFiltered")]
        public long RecordsFiltered { get; set; }

        [JsonProperty("data")]
        public IEnumerable<T> Data { get; set; }

        [JsonProperty("info")]
        public dynamic Info { get; set; }

        public string ToJsonString()
        {
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(this,Formatting.Indented,timeFormat);
        }
    }
}
