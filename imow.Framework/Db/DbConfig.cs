using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Imow.Framework.Engine;
using Oracle.ManagedDataAccess.Client;

namespace Imow.Framework.Db
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public static class DbConfig
    {

        /// <summary>数据库类型</summary>
        public static Db.DbType DbType
        {
            get
            {
                return DbType.SQLSERVER;
            }
        }

        /// <summary>连接字符串</summary>
        public static string ConnectionString
        {
            get { return ImowEngineContext.Current.ResolveConfig<imow.Core.config.DBConfig>().ConnectionString; }
        }

        /// <summary>创建数据库连接</summary>
        /// <returns></returns>
        public static Db.DbHelper CreateHelper()
        {
            return DbHelper.CreateHelper(ConnectionString);
        }
        public static Db.DbHelper CreateHelper(DbType dbtype, string connectionString)
        {
            return Db.DbHelper.CreateHelper(dbtype, connectionString);
        }

        /// <summary>创建数据库连接</summary>
        /// <returns></returns>
        public static Db.DbHelper CreateHelper(string connectionString)
        {
            if (connectionString.StartsWith("oracle;", StringComparison.CurrentCultureIgnoreCase))
            {
                return Db.DbHelper.CreateHelper(DbType.ORACLE, connectionString.Remove(0, "oracle;".Length));
            }
            return Db.DbHelper.CreateHelper(DbType.SQLSERVER, connectionString);
        }

        /// <summary>
        /// 获得数据库提供程序工厂和连接字符串
        /// </summary>
        public static DbProviderFactory GetDbProviderFactory(ref string connectionString)
        {
            if (connectionString.StartsWith("oracle;", StringComparison.CurrentCultureIgnoreCase))
            {
                connectionString = connectionString.Remove(0, "oracle;".Length);
                return OracleClientFactory.Instance;
            }
            return SqlClientFactory.Instance;
        }
        /// <summary>
        ///获得数据库类型提供程序工厂/数据库类型
        /// </summary>
        public static DbType GetDbType(ref string connectionString)
        {
            if (connectionString.StartsWith("oracle;",StringComparison.CurrentCultureIgnoreCase))
            {
                connectionString = connectionString.Remove(0, "oracle;".Length);
                return DbType.ORACLE;
            }
            if (connectionString.StartsWith("mysql;", StringComparison.CurrentCultureIgnoreCase))
            {
                connectionString = connectionString.Remove(0, "mysql;".Length);
                return DbType.MYSQL;
            }
            return DbType.SQLSERVER;
        }



    }
}
