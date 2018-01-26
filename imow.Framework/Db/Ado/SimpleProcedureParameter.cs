using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imow.Framework.Db
{
    /// <summary>
    /// SqlServer简易参数写法
    /// 简化并扩展原先的参数书写方式
    /// </summary>
    public class SqlSimpleProcedureParameter : Dictionary<string, object>
    {
        /// <summary>
        /// 转化成框架支持的参数数组
        /// </summary>
        /// <returns></returns>
        public List<ProcedureParameter> ToParameters()
        {
            List<Db.ProcedureParameter> Par = new List<Db.ProcedureParameter>();
            foreach (var d in this.Keys)
            {
                Par.Add(new ProcedureParameter("@" + d.TrimStart('@'), this[d]));
            }
            return Par;
        }
    }

    /// <summary>
    /// Oracle简易参数写法
    /// 简化并扩展原先的参数书写方式
    /// </summary>
    public class OracleSimpleProcedureParameter : Dictionary<string, object>
    {
        /// <summary>
        /// 转化成框架支持的参数数组
        /// </summary>
        /// <returns></returns>
        public List<ProcedureParameter> ToParameters()
        {
            List<Db.ProcedureParameter> Par = new List<Db.ProcedureParameter>();
            foreach (var d in this.Keys)
            {
                Par.Add(new ProcedureParameter(":" + d.TrimStart(':'), this[d]));
            }
            return Par;
        }
    }
}
