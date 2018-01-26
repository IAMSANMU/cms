using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
namespace Imow.Framework.Tool
{
    /// <summary>
    /// 类型映射帮助
    /// </summary>
    public static class MapperHelper
    {
        /// <summary>
        /// 拷贝到另一个实体
        /// </summary>
        /// <param name="from">源</param>
        /// <param name="to"></param>
        /// <param name="convertNull">是否转换NULL</param>
        /// <param name="ignore">忽略的字段,全小写</param>
        public static void Copy(object from, object to, bool convertNull=false,string[] ignore=null)
        {
            //利用反射获得类成员
            var fieldFroms = from.GetType().GetProperties();
            var fieldTos = to.GetType().GetProperties();
            int lenTo = fieldTos.Length;
            for (int i = 0, l = fieldFroms.Length; i < l; i++)
            {
                for (int j = 0; j < lenTo; j++)
                {
                    if (fieldTos[j].Name != fieldFroms[i].Name) continue;
                    if (ignore != null && ignore.Contains(fieldTos[j].Name.ToLower())) continue;
                    if (fieldFroms[i].CanRead)
                    {
                        var tmpValue = fieldFroms[i].GetValue(from, null);
                        if (tmpValue == null)
                        {
                            if (convertNull)
                            {
                                fieldTos[j].SetValue(to, tmpValue, null);
                            }
                        }
                        else
                        {
                            if (fieldTos[j].CanWrite)
                            {
                                fieldTos[j].SetValue(to, tmpValue, null);
                            }
                        }

                        break;
                    }
                }
            }
        }


        /// <summary>
        /// 实体转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">目标实体</param>
        /// <param name="source">源实体</param>
        /// <returns></returns>
        public static T InEntityFrom<T>(this T target, T source)
        {
            Type t = typeof(T);
            PropertyInfo[] pros = t.GetProperties();
            foreach (PropertyInfo p in pros)
            {
                var o = p.GetValue(source, null);
                if (o != null)
                {
                    p.SetValue(target, o, null);
                }
            }
            return target;
        }
        /// <summary>
        /// 实体转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">目标实体</param>
        /// <param name="source">源实体</param>
        /// <returns></returns>
        public static T InEntityFrom<T, K>(this T target, K source) where T : class, new()
        {
            Type t = typeof(T);
            PropertyInfo[] pros = t.GetProperties();
            PropertyInfo[] spros = typeof(K).GetProperties();
            HybridDictionary dic = new HybridDictionary();
            foreach (var p in spros)
                dic.Add(p.Name, p.GetValue(source, null));
            target = target ?? new T();
            foreach (PropertyInfo p in pros)
            {
                var o = dic[p.Name];
                if (o != null)
                {
                    p.SetValue(target, o, null);
                }
            }
            return target;
        }

        /// <summary>
        /// DataTable转实体
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dt">DataTable数据源</param>
        /// <returns></returns>
        public static T DataTableToModel<T>(this DataTable dt) where T : new()
        {
            DataRow dr = dt.Rows[0];
            T t = new T();
            // 获得此模型的公共属性
            PropertyInfo[] propertys = t.GetType().GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                var tempName = pi.Name;
                // 检查DataTable是否包含此列
                if (dr.Table.Columns.Contains(tempName))
                {
                    // 判断此属性是否有可写
                    if (!pi.CanWrite) continue;
                    object value = dr[tempName];
                    if (value != DBNull.Value)
                    {
                        pi.SetValue(t, CheckType(value, pi.PropertyType), null);
                    }
                }
            }
            return t;
        }



        /// <summary>
        /// DataTable转泛型
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dt">DataTable数据源</param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(this DataTable dt) where T : new()
        {

            List<T> ts = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();

                // 获得此模型的公共属性
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    var tempName = pi.Name;
                    // 检查DataTable是否包含此列
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有可写
                        if (!pi.CanWrite) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(t, CheckType(value, pi.PropertyType), null);
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }
        /// <summary>
        /// DataRow转Model
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dr">DataRow数据源</param>
        /// <returns></returns>
        public static T DataRowToModel<T>(this DataRow dr) where T : new()
        {
            T t = new T();
            // 获得此模型的公共属性
            PropertyInfo[] propertys = t.GetType().GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                var tempName = pi.Name;
                // 检查DataTable是否包含此列
                if (dr.Table.Columns.Contains(tempName))
                {
                    // 判断此属性是否有可写
                    if (!pi.CanWrite) continue;
                    object value = dr[tempName];
                    if (value != DBNull.Value)
                    {
                        pi.SetValue(t, CheckType(value, pi.PropertyType), null);
                    }
                }
            }
            return t;
        }
        /// <summary>
        /// DataReader转实体
        /// </summary>
        /// <typeparam name="T">传入的实体类</typeparam>
        /// <param name="dr">DataReader数据源</param>
        /// <returns>实体类</returns>
        public static T DataReaderMapToModel<T>(this IDataReader dr)
        {
            using (dr)
            {
                if (dr.Read())
                {
                    Type modelType = typeof(T);
                    int count = dr.FieldCount;
                    T model = Activator.CreateInstance<T>();
                    for (int i = 0; i < count; i++)
                    {
                        object value = dr[i];
                        if (value != DBNull.Value)
                        {
                            PropertyInfo pi = modelType.GetProperty(dr.GetName(i), BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                            if (pi != null)
                            {
                                // 判断此属性是否有可写
                                if (!pi.CanWrite) continue;
                                pi.SetValue(model, CheckType(value, pi.PropertyType), null);
                            }
                        }
                    }
                    return model;
                }
            }
            return default(T);
        }
        /// <summary>
        /// DataReader转泛型
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dr">DataReader数据源</param>
        /// <returns></returns>
        public static List<T> DataReaderToList<T>(this IDataReader dr)
        {
            using (dr)
            {
                List<T> list = new List<T>();
                //获取传入的数据类型
                Type modelType = typeof(T);
                //遍历DataReader对象
                while (dr.Read())
                {
                    //使用与指定参数匹配最高的构造函数，来创建指定类型的实例
                    T model = Activator.CreateInstance<T>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        object value = dr[i];
                        if (value != DBNull.Value)
                        {
                            //匹配字段名
                            PropertyInfo pi = modelType.GetProperty(dr.GetName(i), BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                            if (pi != null)
                            {     // 判断此属性是否有可写
                                if (!pi.CanWrite) continue;
                                //绑定实体对象中同名的字段  
                                pi.SetValue(model, CheckType(value, pi.PropertyType), null);
                            }
                        }
                    }
                    list.Add(model);
                }
                return list;
            }
        }
        /// <summary>
        /// 对可空类型进行判断转换(*要不然会报错)
        /// </summary>
        /// <param name="value">DataReader字段的值</param>
        /// <param name="conversionType">该字段的类型</param>
        /// <returns></returns>
        private static object CheckType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null || value.ToString() == string.Empty)
                    return null;
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }
    }
}
