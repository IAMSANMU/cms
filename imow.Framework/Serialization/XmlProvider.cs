using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Imow.Framework.Serialization
{
    public class XmlProvider<T>
    {
        public System.Text.Encoding encode = Encoding.GetEncoding("utf-8");
        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public string Serializer(T obj)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            //ns.Add("", "");
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter vStreamWriter = new StreamWriter(stream, encode))
                {
                    xs.Serialize(vStreamWriter, obj, ns);
                    var r = encode.GetString(stream.ToArray());
                    return r;
                }
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <returns></returns>
        public string Serializer(T obj, bool ifNameSpace)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter vStreamWriter = new StreamWriter(stream, encode))
                {
                    if (ifNameSpace == false)
                    {
                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                        ns.Add("", "");
                        xs.Serialize(vStreamWriter, obj, ns);
                    }
                    else
                    {
                        xs.Serialize(vStreamWriter, obj);
                    }
                    var r = encode.GetString(stream.ToArray());
                    return r;
                }
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <returns></returns>
        public T Deserialize(string xml)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream(encode.GetBytes(xml)))
            {
                var r = (T)xs.Deserialize(stream);
                return r;
            }
        }
    }
}
