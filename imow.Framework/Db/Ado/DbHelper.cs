using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Imow.Framework.Db
{
    /// <summary>
    /// 数据库连接类
    /// </summary>
    public abstract class DbHelper
    {
        /// <summary>连接字符串</summary>
        protected string _connectionstring;
        /// <summary>是否监控sql耗时HH:ss:mm.fff</summary>
        protected bool _iswatchtime = true;
        ///// <summary>
        ///// 是否监控sql耗时HH:ss:mm.fff
        ///// </summary>
        //public bool IsWatchTime { get { return _iswatchtime; } set { _iswatchtime = value; } }
        /// <summary>
        /// 创建数据访问对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns></returns>
        public static DbHelper CreateHelper(string connectionString)
        {
            string connectionstring = connectionString;
            DbType dbType = DbConfig.GetDbType(ref connectionstring);
            switch (dbType)
            {
                case DbType.SQLSERVER:
                    return new DbSqlServer(connectionstring, true);
                case DbType.ORACLE:
                    return new DbOracle(connectionstring, true);
                default:
                    throw new Exception("该数据库类型不适合使用CreateHelper，请用new创建！");
            }
        }
        public static DbHelper CreateHelper(DbType dbtype, string connectionString)
        {
            string connectionstring = connectionString;
            switch (dbtype)
            {
                case DbType.SQLSERVER:
                    return new DbSqlServer(connectionstring, true);
                case DbType.ORACLE:
                    return new DbOracle(connectionstring, true);
                default:
                    throw new Exception("该数据库类型不适合使用CreateHelper，请用new创建！");
            }
        }
        /// <summary>
        ///  创建数据访问对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="isWatchTime">是否开启耗时监控</param>
        /// <returns></returns>
        public static DbHelper CreateHelper(string connectionString, bool isWatchTime)
        {
            string connectionstring = connectionString;

            DbType dbType = DbConfig.GetDbType(ref connectionstring);
            switch (dbType)
            {
                case DbType.SQLSERVER:
                    return new DbSqlServer(connectionstring, isWatchTime);
                case DbType.ORACLE:
                    return new DbOracle(connectionstring, isWatchTime);
                default:
                    throw new Exception("该数据库类型不适合使用CreateHelper，请用new创建！");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static DbHelper CreateHelper(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.SQLSERVER:
                    return new DbSqlServer("", true);
                case DbType.ORACLE:
                    return new DbOracle("", true);
                default:
                    throw new Exception("该数据库类型不适合使用CreateHelper，请用new创建！");
            }
        }
        /// <summary>执行SQL语句 返回影响数据行数</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="procedurePar">SQL参数</param>
        /// <returns>影响行数</returns>
        public int ExecuteSql(string sql, List<ProcedureParameter> procedurePar)
        {
            return ExecuteSql(sql, CommandType.Text, procedurePar);
        }
        /// <summary>执行SQL语句 返回影响数据行数</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="procedurePar">SQL参数</param>
        /// <returns>返回影响行数</returns>
        protected abstract int ExecuteSql(string sql, CommandType cmdType, List<ProcedureParameter> procedurePar);
        /// <summary>执行存储过程 返回影响行数</summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="procedurePar">参数</param>
        /// <returns>影响行数</returns>
        public int ExecuteProcedure(string procedureName, List<ProcedureParameter> procedurePar)
        {
            return ExecuteSql(procedureName, CommandType.StoredProcedure, procedurePar);
        }

        /// <summary>执行SQL语句 返回第一行第一列</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="procedurePar">SQL参数</param>
        /// <returns>Object</returns>
        public object ExecuteScalar(string sql, List<ProcedureParameter> procedurePar)
        {
            return ExecuteScalar(sql, CommandType.Text, procedurePar);
        }
        /// <summary>执行SQL语句 返回第一行第一列</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="procedurePar">SQL参数</param>
        /// <returns>object</returns>
        protected abstract object ExecuteScalar(string sql, CommandType cmdType, List<ProcedureParameter> procedurePar);
        /// <summary>执行存储过程 返回第一行第一列</summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="procedurePar">参数</param>
        /// <returns></returns>
        public object ExecuteSpScalar(string procedureName, List<ProcedureParameter> procedurePar)
        {
            return ExecuteScalar(procedureName, CommandType.StoredProcedure, procedurePar);
        }
        /// <summary>执行SQL语句返回DataReader</summary>
        /// <param name="sql"></param>
        /// <param name="procedurePar">SQL参数</param>
        /// <returns>DataReader</returns>
        public DbDataReader SqlToDbReader(string sql, List<ProcedureParameter> procedurePar)
        {
            return SqlToDbReader(sql, CommandType.Text, procedurePar);
        }
        /// <summary>执行SQL语句返回DataReader</summary>
        /// <param name="sql"></param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="procedurePar">SQL参数</param>
        /// <returns>DataReader</returns>
        protected abstract DbDataReader SqlToDbReader(string sql, CommandType cmdType, List<ProcedureParameter> procedurePar);
        /// <summary>执行存储过程返回DataReader</summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="procedurePar">SQL参数</param>
        /// <returns>DataReader</returns>
        public DbDataReader SpToDbReader(string procedureName, List<ProcedureParameter> procedurePar)
        {
            return SqlToDbReader(procedureName, CommandType.StoredProcedure, procedurePar);
        }


        /// <summary>执行SQL语句返回DataTable</summary>
        /// <param name="sql">sql语句</param>
        /// <param name="procedurePar">参数</param>
        /// <returns>DataTable</returns>
        public DataTable SqlToDataTable(string sql, List<ProcedureParameter> procedurePar)
        {
            DataSet ds = SqlToDataSet(sql, CommandType.Text, procedurePar);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>执行存储过程返回DataTable</summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="procedurePar">参数</param>
        /// <returns>DataTable</returns>
        public DataTable SqToDataTable(string procedureName, List<ProcedureParameter> procedurePar)
        {
            DataSet ds = SqlToDataSet(procedureName, CommandType.StoredProcedure, procedurePar);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>执行SQL语句返回记录集</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="procedurePar">参数</param>
        /// <returns>DataSet</returns>
        public DataSet SqlToDataSet(string sql, List<ProcedureParameter> procedurePar)
        {
            return SqlToDataSet(sql, CommandType.Text, procedurePar);
        }

        /// <summary>执行SQL语句返回记录集</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="procedurePar">参数</param>
        /// <returns></returns>
        protected abstract DataSet SqlToDataSet(string sql, CommandType cmdType, List<ProcedureParameter> procedurePar);

        /// <summary>执行存储过程返回DataSet</summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="procedurePar">参数</param>
        /// <returns>DataSet</returns>
        public DataSet SpToDataSet(string procedureName, List<ProcedureParameter> procedurePar)
        {
            return SqlToDataSet(procedureName, CommandType.StoredProcedure, procedurePar);
        }

        /// <summary>取得刚刚插入数据库的种子序号</summary>
        /// <returns></returns>
        public abstract int GetIdentity();
        /// <summary>取得数据库当前系统时间</summary>
        /// <returns></returns>
        public abstract DateTime GetServerDate();
        /// <summary>数据库中表是否存在</summary>
        /// <param name="aTableName">表名</param>
        /// <returns></returns>
        public abstract bool TableIsExist(string aTableName);
        /// <summary>数据库中表是否存在</summary>
        /// <param name="aDbName">数据库名</param>
        /// <param name="aTableName">表名</param>
        /// <returns></returns>
        public abstract bool TableIsExist(string aDbName, string aTableName);
        /// <summary>表中字段是否存在</summary>
        /// <param name="aTableName">表名</param>
        /// <param name="aFieldName">字段名</param>
        /// <returns></returns>
        public abstract bool FieldIsExist(string aTableName, string aFieldName);
        /// <summary>从多选字符串MultiChoiceStr转换成Sql条件</summary>
        /// <param name="AMultiChoiceStr">多选字符串</param>
        /// <param name="aField">字段</param>
        /// <param name="aStyle">类型 0-两边都不加  1-%左边加  2-右边加%  3-%两边加%</param>
        /// <param name="aPar">返回参数</param>
        /// <returns></returns>
        public abstract string GetMcsToSql(string AMultiChoiceStr, string aField, int aStyle, ref List<Db.ProcedureParameter> aPar);

        /// <summary>批量插入</summary>
        /// <param name="dt">数据源</param>
        /// <param name="datatablename">表名</param>
        /// <param name="batchSize">条数</param>
        public abstract void SqlBulkCopy(DataTable dt, string datatablename,int batchSize = 0);
        /// <summary>批量插入</summary>
        /// <param name="dt">数据源</param>
        /// <param name="datatablename">表名</param>
        /// <param name="mapps">列映射</param>
        /// <param name="batchSize">条数</param>
        public abstract void SqlBulkCopy(DataTable dt, string datatablename,  Dictionary<string, string> mapps, int batchSize = 0);


    }
}
