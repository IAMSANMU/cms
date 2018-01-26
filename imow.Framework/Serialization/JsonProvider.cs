using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Imow.Framework.Serialization
{
    /// <summary>
    /// jason 序列化方式
    /// </summary>
    public class JsonProvider
    {
        /// <summary>
        /// jason序列化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string Serializer(object o)
        {
            return Serializer(o,false);
        }
        /// <summary>
        /// jason序列化，过滤字段
        /// </summary>
        /// <param name="o">生成json的对象</param>
        /// <param name="filed"> 忽略的字段数组</param>
        /// <returns></returns>
        public static string Serializer(object o, List<string> filed)
        {
            JsonSerializerSettings Ignore = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new PropertiesResolver(filed),
                //忽略默认值 如引用类型的null
                DefaultValueHandling = DefaultValueHandling.Ignore

            };
            return JsonConvert.SerializeObject(o, Ignore);
        }

        /// <summary>
        /// jason序列化
        /// </summary>
        /// <param name="o">生成json的对象</param>
        /// <param name="formatjson">是否格式化</param>
        /// <returns></returns>
        public static string Serializer(object o, bool formatjson)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            IsoDateTimeConverter idtc = new IsoDateTimeConverter();
            idtc.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(idtc);
            JsonWriter jw = new JsonTextWriter(sw);

            if (formatjson)
            {
                jw.Formatting = Formatting.Indented;
            }
            serializer.Serialize(jw, o);
            return sb.ToString();
        }

        /// <summary>
        /// jason反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            var jsetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            return JsonConvert.DeserializeObject<T>(json, jsetting);
        }
        /// <summary>
        /// jason反序列化
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="filed">忽略字段</param>
        /// <returns></returns>
        public static T Deserialize<T>(string json, List<string> filed)
        {
            var jsetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            jsetting.ContractResolver = new PropertiesResolver(filed);
            return JsonConvert.DeserializeObject<T>(json, jsetting);
        }


    }
    /// <summary>
    /// 忽略字段转换json
    /// </summary>
    public class PropertiesResolver : DefaultContractResolver
    {
        IEnumerable<string> lstExclude;
        public PropertiesResolver(IEnumerable<string> excludedProperties)
        {
            lstExclude = excludedProperties;
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).ToList().FindAll(p => !lstExclude.Contains(p.PropertyName));
        }
    }

}
