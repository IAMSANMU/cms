using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Imow.Framework.Tool
{
    /// <summary>
    /// 枚举操作工具类 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumHelper<T> where T : struct
    {
        /// <summary>
        /// 将指定枚举转为列表控件的数据源
        /// </summary>
        /// <returns>Text:枚举的描述，value：枚举的值</returns>
        public static IEnumerable<SelectListItem> GetAllEnumsOfList()
        {
            Type t = typeof(T);
            FieldInfo[] fieldInfoList = t.GetFields();
            IList<SelectListItem> dt = new List<SelectListItem>();

            var query = from q in fieldInfoList
                        where q.GetCustomAttributes(typeof(DescriptionAttribute), false).Length > 0
                        select new SelectListItem()
                        {
                            Value = q.Name,
                            Text = (q.GetCustomAttributes(typeof(DescriptionAttribute), false)[0] as DescriptionAttribute).Description ?? q.Name
                        };
            return query;
        }

        /// <summary>
        /// 将指定枚举转为列表控件的数据源
        /// </summary>
        /// <returns>Text:枚举的描述，value：枚举的值</returns>
        public static IEnumerable<SelectListItem> GetAllEnumsOfList2()
        {
            Type t = typeof(T);
            FieldInfo[] fieldInfoList = t.GetFields();
            IList<SelectListItem> dt = new List<SelectListItem>();

            var query = from q in fieldInfoList
                        where q.GetCustomAttributes(typeof(DescriptionAttribute), false).Length > 0
                        select new SelectListItem()
                        {
                            Value = ((int)q.GetValue(null)).ToString(),
                            Text = (q.GetCustomAttributes(typeof(DescriptionAttribute), false)[0] as DescriptionAttribute).Description ?? q.Name
                        };
            return query;
        }

        /// <summary>
        /// 将制定枚举转化为dataTable  
        /// </summary>
        /// <returns>Text:枚举的描述，Value：枚举的值</returns>
        public static DataTable GetDataTableFromEnums()
        {
            Type t = typeof(T);
            FieldInfo[] fieldInfoList = t.GetFields();
            DataTable dt = new DataTable();
            dt.Columns.Add("Text");
            dt.Columns.Add("Value");
            foreach (var m in fieldInfoList)
            {
                if (m.GetCustomAttributes(typeof(DescriptionAttribute), false).Length > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["Text"] = (m.GetCustomAttributes(typeof(DescriptionAttribute), false)[0] as DescriptionAttribute).Description ?? m.Name;
                    dr["Value"] = ((int)m.GetValue(null)).ToString();
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        /// <summary>
        /// 获得指定枚举的描述信息
        /// </summary>
        /// <param name="value "></param>
        /// <returns></returns>
        public static string GetText(int value)
        {
            try
            {
                return GetText((T)Enum.ToObject(typeof(T), value));
            }
            catch(Exception ee)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获得指定枚举的描述信息
        /// </summary>
        /// <param name="enumInstance"></param>
        /// <returns></returns>
        public static string GetText(T enumInstance)
        {
            Type t = typeof(T);
            FieldInfo[] fieldInfoList = t.GetFields();

            var query = from q in fieldInfoList
                        where q.GetCustomAttributes(typeof(DescriptionAttribute), false).Length > 0
                        && string.Equals(q.Name, enumInstance.ToString(), StringComparison.CurrentCultureIgnoreCase)
                        select ((DescriptionAttribute)q.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description;
            if (query != null && query.Count() > 0)
            {
                return query.First<string>();
            }
            return null;
            /*
            string strReturn = string.Empty;


            foreach (FieldInfo tField in fieldInfoList)
            {
                if (!tField.IsSpecialName && tField.Name.ToLower() == enumInstance.ToString().ToLower())
                {
                    DescribtionAtrtribute[] enumAttributelist = (DescribtionAtrtribute[])tField.GetCustomAttributes(typeof(DescribtionAtrtribute), false);
                    if (enumAttributelist != null && enumAttributelist.Length > 0)
                    {
                        strReturn = enumAttributelist[0].Description;
                        break;
                    }
                }
            }
            return strReturn;
             * */
        }

        /// <summary>
        /// 通过枚举，获得其枚举值
        /// </summary>
        /// <param name="enumInstance"></param>
        /// <returns></returns>
        public static string GetValue(T enumInstance)
        {
            return enumInstance.ToString();
            /*
            Type t = typeof(T);
            FieldInfo[] fieldInfoList = t.GetFields();
            string strReturn = string.Empty;
            foreach (FieldInfo tField in fieldInfoList)
            {
                if (!tField.IsSpecialName && tField.Name.ToLower() == enumInstance.ToString().ToLower())
                {
                    strReturn = tField.Name;
                    break;
                }
            }
            return strReturn;*/
        }

        /// <summary>
        /// 将字符串转换为枚举
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static T GetEnum(string Value)
        {
            Type t = typeof(T);
            FieldInfo field = t.GetField(Value);
            T e = default(T);
            System.Enum.TryParse<T>(Value, out e);
            return e;
        }


        /*public static void BindListControl(ListControl listControl)
        {
            listControl.DisplayMember = "Text";
            listControl.ValueMember = "Value";
            listControl.DataSource = GetAllEnums();
        }*/
    }
}
