using System;
using System.Collections;
using System.Reflection;

namespace imow.Model
{
    /// <summary>
    /// 共通枚举类型属性
    /// </summary>
    /// <example>
    /// [EcEnum("中文数字")]
    /// enum MyEnum
    /// {
    ///     [EcEnum("数字一", 1)]
    ///     One = 1,
    ///
    ///     [EcEnum("数字二", 2)]
    ///     Two,
    ///
    ///     [EcEnum("数字三", 3)]
    ///     Three
    /// }
    /// 取得枚举类名称：EcEnumAttribute.GetEnumName(typeof(MyEnum)) 返回："中文数字";
    /// 取得枚举类字段名称：EcEnumAttribute.GetEnumName(MyEnum.Two)　返回："Two";
    /// 取得枚举类字段描述：EcEnumAttribute.GetEnumText(MyEnum.Two)　返回："数字二";
    /// 获得枚举中各个字段的定义数组:EcEnumAttribute[] ems = EcEnumAttribute.GetEcEnums(typeof(MyEnum));
    /// GetEcEnums(Type enumType, SortType sortType) 重载可定义是否按排序值排序
    /// 根据value取得枚举类字段：EcEnumAttribute em = EcEnumAttribute.GetEnum(typeof(MyEnum), 3);
    ///                        string text = em.Text; //得到 "数字三"
    /// 
    /// 绑定下拉控件：
    ///     ddlControl.DataSource = EcEnumAttribute.GetEcEnums(typeof(MyEnum));
    ///     ddlControl.DataTextField = "Text";
    ///     ddlControl.DataValueField  = "Value";
    ///     ddlControl.DataBind();
    /// </example>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class EcEnumAttribute : System.Attribute
    {
        /// <summary>
        /// 排序类型
        /// </summary>
        public enum SortType
        {

            /// <summary>
            ///按枚举顺序默认排序
            /// </summary>
            Default,
            /// <summary>
            /// 按描述值排序
            /// </summary>
            Description,
            /// <summary>
            /// 按排序排序
            /// </summary>
            Rank
        }

        private static Hashtable cachedEnum = new Hashtable();

        private FieldInfo fieldInfo;

        #region 属性

        public int Value
        {
            get { return (int)fieldInfo.GetValue(null); }
        }

        public string StringValue
        {
            get { return Value.ToString(); }
        }

        public string Name
        {
            get { return fieldInfo.Name; }
        }

        public string Text { private set; get; }

        public int Rank { private set; get; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 描述枚举值，默认排序为5
        /// </summary>
        /// <param name="desc">描述内容</param>
        public EcEnumAttribute(string text)
            : this(text, 5) { }

        /// <summary>
        /// 描述枚举值
        /// </summary>
        /// <param name="desc">描述内容</param>
        /// <param name="rank">排列顺序</param>
        public EcEnumAttribute(string text, int rank)
        {
            this.Text = text;
            this.Rank = rank;
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 取得枚举类
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static EcEnumAttribute GetEnum(Type enumType, int value)
        {
            EcEnumAttribute[] descriptions = GetEcEnums(enumType, SortType.Default);
            foreach (EcEnumAttribute ed in descriptions)
            {
                if (ed.Value == value) return ed;
            }
            return null;
        }

        /// <summary>
        /// 取得枚举类
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static EcEnumAttribute GetEnum(Type enumType, string value)
        {
            EcEnumAttribute attr = null;
            int v = 0;
            if (int.TryParse(value, out v))
            {
                attr = GetEnum(enumType, v);
            }
            if (attr == null)
            {
                attr = new EcEnumAttribute("");
            }
            return attr;
        }

        /// <summary>
        /// 得到枚举类描述名称
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static string GetEnumName(Type enumType)
        {
            EcEnumAttribute[] eds = (EcEnumAttribute[])enumType.GetCustomAttributes(typeof(EcEnumAttribute), false);
            if (eds.Length == 1) return string.Empty;
            return eds[0].Text;
        }

        /// <summary>
        /// 获得指定枚举类型中，指定值的描述文本
        /// </summary>
        /// <param name="enumValue">枚举值，不要作任何类型转换</param>
        /// <returns>描述字符串</returns>
        public static string GetEnumText(object enumValue)
        {
            if (typeof(EcEnumAttribute).IsAssignableFrom(enumValue.GetType()))
            {
                return string.Empty;
            }
            EcEnumAttribute[] descriptions = GetEcEnums(enumValue.GetType(), SortType.Default);
            foreach (EcEnumAttribute ed in descriptions)
            {
                if (ed.fieldInfo.Name.Equals(enumValue.ToString())) return ed.Text;
            }
            return string.Empty;
        }

        /// <summary>
        /// 得到枚举类所有描述类型，按定义的顺序返回
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        /// <param name="enumType">枚举类型</param>
        /// <returns>所有定义的文本</returns>
        public static EcEnumAttribute[] GetEcEnums(Type enumType)
        {
            return GetEcEnums(enumType, SortType.Default);
        }

        /// <summary>
        /// 得到枚举类所有描述类型
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        /// <param name="enumType">枚举类型</param>
        /// <param name="sortType">指定排序类型</param>
        /// <returns>所有定义的文本</returns>
        public static EcEnumAttribute[] GetEcEnums(Type enumType, SortType sortType)
        {
            EcEnumAttribute[] descriptions = null;
            //缓存中没有找到，通过反射获得字段的描述信息
            lock (cachedEnum)
            {
                if (cachedEnum.Contains(enumType.FullName) == false)
                {
                    FieldInfo[] fields = enumType.GetFields();
                    ArrayList edAL = new ArrayList();
                    foreach (FieldInfo fi in fields)
                    {
                        object[] eds = fi.GetCustomAttributes(typeof(EcEnumAttribute), false);
                        if (eds.Length == 0) continue;
                        ((EcEnumAttribute)eds[0]).fieldInfo = fi;
                        edAL.Add(eds[0]);
                    }
                    cachedEnum.Add(enumType.FullName, (EcEnumAttribute[])edAL.ToArray(typeof(EcEnumAttribute)));
                }
            }
            descriptions = (EcEnumAttribute[])cachedEnum[enumType.FullName];
            if (descriptions.Length <= 0) throw new NotSupportedException("枚举类型[" + enumType.Name + "]未定义属性[EcEnum]");
            //按指定的属性冒泡排序
            for (int m = 0; m < descriptions.Length; m++)
            {
                //默认就不排序了
                if (sortType == SortType.Default) break;
                for (int n = m; n < descriptions.Length; n++)
                {
                    EcEnumAttribute temp;
                    bool swap = false;
                    switch (sortType)
                    {
                        case SortType.Default:
                            break;
                        case SortType.Description:
                            if (string.Compare(descriptions[m].Text, descriptions[n].Text) > 0) swap = true;
                            break;
                        case SortType.Rank:
                            if (descriptions[m].Rank > descriptions[n].Rank) swap = true;
                            break;
                    }
                    if (swap)
                    {
                        temp = descriptions[m];
                        descriptions[m] = descriptions[n];
                        descriptions[n] = temp;
                    }
                }
            }
            return descriptions;
        }

        #endregion
    }
}
