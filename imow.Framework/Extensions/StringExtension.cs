using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC.Common.Extensions
{
	public static class StringExtension
	{
		public static string[] SplitRemoveEmpty(this string obj, char split)
		{
			if (obj == null)
			{
				return new string[0];
			}
			return obj.Split(new char[1] { split }, StringSplitOptions.RemoveEmptyEntries);
		}

		public static long[] SplitRemoveEmptyToInt64(this string obj, char split)
		{
			if (obj == null)
			{
				return new long[0];
			}
			return obj.Split(new char[1] { split }, StringSplitOptions.RemoveEmptyEntries)
						.Select(i => Int64.Parse(i))
						.ToArray(); ;
		}

		public static int[] SplitRemoveEmptyToInt32(this string obj, char split)
		{
			if (obj == null)
			{
				return new int[0];
			}
			return obj.Split(new char[1] { split }, StringSplitOptions.RemoveEmptyEntries)
						.Select(i => Int32.Parse(i))
						.ToArray();
		}

		public static bool EqualOrdinalIgnoreCase(this string obj, string value)
		{
			if (obj == null)
			{
				return false;
			}
			return obj.Equals(value, StringComparison.OrdinalIgnoreCase);
		}
		/// <summary>
		/// 后缀加入指定的值
		/// </summary>
		public static string EndSuffix(this string obj, string value)
		{
			if (obj == null)
			{
				return null;
			}
			return obj.EndsWith(value) ? obj : string.Concat(obj, value);
		}

		public static string Contact(this string obj, string value)
		{
			var v = obj;
			if (obj == null)
			{
				v = "";
			}
			return string.Concat(obj, value??"");
		}

		public static int ToInt(this string obj)
		{
			return int.Parse(obj);
		}

		public static long ToInt64(this string obj)
		{
			return long.Parse(obj);
		}

		public static decimal ToDecimal(this string obj)
		{
			return decimal.Parse(obj);
		}

		public static T? ToNullable<T>(this string obj) where T : struct
		{
			if (string.IsNullOrWhiteSpace(obj))
			{
				return null;
			}
			return JsonConvert.DeserializeObject<T>(obj);
		}
		/// <summary>
		/// 截断字符串
		/// 如果指定的长度大于了字符串本身的长度，则自动截断
		/// <example>
		/// SubStr(0,12);
		/// </example>
		/// </summary>
		public static string Substr(this string obj, int start, int length)
		{
			if (string.IsNullOrWhiteSpace(obj))
			{
				return obj;
			}
			if ((start + length) > obj.Length)
			{
				return obj.Substring(0, obj.Length);
			}
			return obj.Substring(0, length);
		}
	}
}
