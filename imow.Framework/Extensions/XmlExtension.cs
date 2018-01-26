using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EC.Common.Extensions
{
	public static class XmlAttributeExtension
	{

		public static bool IsNullOrEmpty(this XmlAttribute attr)
		{
			return attr == null || string.IsNullOrWhiteSpace(attr.Value);
		}

		public static Int16 ToShort(this XmlAttribute attr)
		{
			if (!attr.IsNullOrEmpty())
			{
				short result;
				if (short.TryParse(attr.Value, out result))
				{
					return result;
				}
			}

			throw new ArgumentException("参数不符合规范", attr.Name);
		}

		public static Int32 ToInt(this XmlAttribute attr)
		{
			if (!attr.IsNullOrEmpty())
			{
				int result;
				if (int.TryParse(attr.Value, out result))
				{
					return result;
				}
			}

			throw new ArgumentException("参数不符合规范", attr.Name);
		}

		public static Int64 ToInt64(this XmlAttribute attr)
		{
			if (!attr.IsNullOrEmpty())
			{
				Int64 result;
				if (Int64.TryParse(attr.Value, out result))
				{
					return result;
				}
			}

			throw new ArgumentException("参数不符合规范", attr.Name);
		}

		public static decimal ToDecimal(this XmlAttribute attr)
		{
			if (!attr.IsNullOrEmpty())
			{
				decimal result;
				if (decimal.TryParse(attr.Value, out result))
				{
					return result;
				}
			}

			throw new ArgumentException("参数不符合规范", attr.Name);
		}

		public static string ToStr(this XmlAttribute attr)
		{
			if (!attr.IsNullOrEmpty())
			{
				return attr.Value;
			}
			return string.Empty;
		}

		public static bool ToBoolean(this XmlAttribute attr)
		{
			bool result;
			if (!attr.IsNullOrEmpty())
			{
				if (bool.TryParse(attr.Value, out result))
				{
					return result;
				}
				if ("1".Equals(attr.Value))
				{
					return true;
				}
				else if ("0".Equals(attr.Value))
				{
					return false;
				}
			}
			throw new ArgumentException("参数不符合规范", attr.Name);
		}

		public static T? ToNullable<T>(this XmlAttribute attr) where T : struct
		{
			if (attr.IsNullOrEmpty())
			{
				return null;
			}
			return JsonConvert.DeserializeObject<T>(attr.Value);
		}

        public static string GetWXValueByTagName(this XmlDocument xml, string tagName)
        {
            var tagList = xml.GetElementsByTagName(tagName);
            foreach (XmlNode node in tagList)
            {
                if (node.FirstChild.NodeType == XmlNodeType.CDATA)
                {
                    return node.FirstChild.Value;
                }
            }
            if (tagList.Count > 0)
            {
                return tagList[0].InnerText;
            }
            return "";
        }
	}
}
