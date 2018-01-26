using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Oracle.ManagedDataAccess.Client;

namespace Imow.Framework.Db
{
    class DbOracle : DbHelper
    {
        public DbOracle(string connectionString, bool isWatchTime)
        {
            _connectionstring = connectionString;
            _iswatchtime = isWatchTime;
        }
        /// <summary>参数类型转化</summary>
        /// <param name="par">参数类</param>
        /// <returns>oralce参数</returns>
        private OracleParameter ParameterTransform(ProcedureParameter par)
        {
            //支持简易参数写法
            string parName = ":" + par.Name.TrimStart(':');
            /*支持无类型参数*/
            if (par.ParType == ProcParType.Default)
                return new OracleParameter(parName, par.Value ?? DBNull.Value);
            OracleParameter p = new OracleParameter();
            p.ParameterName = parName;
            switch (par.ParType)
            {
                case ProcParType.Int16:
                    p.OracleDbType = OracleDbType.Int16;
                    break;
                case ProcParType.Int32:
                    p.OracleDbType = OracleDbType.Int32;
                    break;
                case ProcParType.Int64:
                    p.OracleDbType = OracleDbType.Int64;
                    break;
                case ProcParType.Single:
                    p.OracleDbType = OracleDbType.Single;
                    break;
                case ProcParType.Double:
                    p.OracleDbType = OracleDbType.Double;
                    break;
                case ProcParType.Decimal:
                    p.OracleDbType = OracleDbType.Decimal;
                    break;
                case ProcParType.Char:
                    p.OracleDbType = OracleDbType.Char;
                    break;
                case ProcParType.VarChar:
                    p.OracleDbType = OracleDbType.Varchar2;
                    break;
                case ProcParType.NVarchar:
                    p.OracleDbType = OracleDbType.NVarchar2;
                    break;
                case ProcParType.Image:
                    p.OracleDbType = OracleDbType.Blob;
                    break;
                case ProcParType.DateTime:
                    p.OracleDbType = OracleDbType.Date;
                    break;
                default:
                    throw new Exception("未知类型ProcParType：" + par.ParType.ToString());
            }
            p.Size = par.Size;
            p.Direction = par.Direction;
            switch (par.Direction)
            {
                case ParameterDirection.Input:
                case ParameterDirection.InputOutput:
                    if (par.Value == null)
                    {
                        p.Value = DBNull.Value;
                    }
                    else
                    {
                        p.Value = par.Value;
                    }
                    break;
            }
            return p;
        }

        /// <summary>执行SQL语句 返回影响数据行数</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="procedurePar">SQL参数</param>
        /// <returns>返回影响行数</returns>
        protected override int ExecuteSql(string sql, CommandType cmdType, List<ProcedureParameter> procedurePar)
        {

            DbConnection conn = null;
            DbCommand cmd = new OracleCommand();
            DbTransaction dbTransaction = null;
            try
            {
                cmd.CommandText = sql;
                cmd.CommandType = cmdType;
                //设置参数
                procedurePar = procedurePar ?? new List<ProcedureParameter>();
                foreach (var item in procedurePar)
                {
                    OracleParameter p = ParameterTransform(item);
                    cmd.Parameters.Add(p);
                }
                //启用事务
                if (null != Imow.Framework.Db.Transaction.Current)
                {
                    cmd.Connection = Imow.Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction.Connection;
                    cmd.Transaction = Imow.Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction;
                }
                else
                {
                    conn = new OracleConnection();
                    conn.ConnectionString = _connectionstring;
                    cmd.Connection = conn;
                    conn.Open();

                    if (System.Transactions.Transaction.Current == null)
                    {
                        dbTransaction = conn.BeginTransaction();
                        cmd.Transaction = dbTransaction;
                    }
                }
                int result = cmd.ExecuteNonQuery();
                if (null != dbTransaction)
                {
                    dbTransaction.Commit();
                }
                cmd.Parameters.Clear();
                return result;
            }
            catch
            {
                if (null != dbTransaction)
                {
                    dbTransaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (null != conn)
                {
                    conn.Dispose();
                }
                if (null != dbTransaction)
                {
                    dbTransaction.Dispose();
                }
                cmd.Dispose();
            }
        }

        /// <summary>执行SQL语句 返回第一行第一列</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="procedurePar">SQL参数</param>
        /// <returns>object</returns>
        protected override object ExecuteScalar(string sql, CommandType cmdType, List<ProcedureParameter> procedurePar)
        {
            DbConnection conn = null;
            DbCommand cmd = new OracleCommand();
            DbTransaction dbTransaction = null;
            try
            {
                cmd.CommandText = sql;
                cmd.CommandType = cmdType;
                //设置参数
                procedurePar = procedurePar ?? new List<ProcedureParameter>();
                foreach (var item in procedurePar)
                {
                    OracleParameter p = ParameterTransform(item);
                    cmd.Parameters.Add(p);
                }
                //启用事务
                if (null != Imow.Framework.Db.Transaction.Current)
                {
                    cmd.Connection = Imow.Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction.Connection;
                    cmd.Transaction = Imow.Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction;
                }
                else
                {
                    conn = new OracleConnection();
                    conn.ConnectionString = _connectionstring;
                    cmd.Connection = conn;
                    conn.Open();
                    if (System.Transactions.Transaction.Current == null)
                    {
                        dbTransaction = conn.BeginTransaction();
                        cmd.Transaction = dbTransaction;
                    }

                }
                object val = cmd.ExecuteScalar();
                if (null != dbTransaction)
                {
                    dbTransaction.Commit();
                }
                cmd.Parameters.Clear();
                return val;
            }
            catch
            {
                if (null != dbTransaction)
                {
                    dbTransaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (null != conn)
                {
                    conn.Dispose();
                }
                if (null != dbTransaction)
                {
                    dbTransaction.Dispose();
                }
                cmd.Dispose();
            }
        }

        /// <summary>执行SQL语句返回DataReader</summary>
        /// <param name="sql"></param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="procedurePar">SQL参数</param>
        /// <returns>DataReader</returns>
        protected override DbDataReader SqlToDbReader(string sql, CommandType cmdType, List<ProcedureParameter> procedurePar)
        {
            DbConnection conn = null;
            try
            {
                conn = new OracleConnection();
                conn.ConnectionString = _connectionstring;
                conn.Open();
                //耗时错误拦截
                return DbCatch.Catch<DbDataReader>(_iswatchtime, conn.DataSource, sql, procedurePar, () =>
                {
                    DbCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = cmdType;
                    //设置参数
                    procedurePar = procedurePar ?? new List<ProcedureParameter>();
                    foreach (var item in procedurePar)
                    {
                        OracleParameter p = ParameterTransform(item);
                        cmd.Parameters.Add(p);
                    }
                    DbDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return rdr;
                });
            }
            catch
            {
                conn.Dispose();
                throw;
            }
        }

        /// <summary>执行SQL语句返回记录集</summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="procedurePar">参数</param>
        /// <returns></returns>
        protected override DataSet SqlToDataSet(string sql, CommandType cmdType, List<ProcedureParameter> procedurePar)
        {
            DbConnection conn = null;
            DbCommand cmd = new OracleCommand();
            DbDataAdapter adapter = new OracleDataAdapter();
            try
            {
                //耗时错误拦截
                return DbCatch.Catch<DataSet>(_iswatchtime, null, sql, procedurePar, () =>
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = cmdType;
                    //设置参数
                    procedurePar = procedurePar ?? new List<ProcedureParameter>();
                    foreach (var item in procedurePar)
                    {
                        OracleParameter p = ParameterTransform(item);
                        cmd.Parameters.Add(p);
                    }
                    conn = new OracleConnection();
                    conn.ConnectionString = _connectionstring;
                    cmd.Connection = conn;
                    conn.Open();
                    adapter.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    cmd.Parameters.Clear();
                    return ds;
                });
            }
            catch
            {
                throw;
            }
            finally
            {
                if (null != conn)
                {
                    conn.Dispose();
                }
                cmd.Dispose();
            }
        }

        /// <summary>取得刚刚插入数据库的种子序号</summary>
        /// <returns></returns>
        public override int GetIdentity()
        {
            throw new NotImplementedException();
        }

        /// <summary>取得数据库当前系统时间</summary>
        /// <returns></returns>
        public override DateTime GetServerDate()
        {
            string TempSql = "Select sysdate as aDate from DUAL";
            return Convert.ToDateTime(ExecuteScalar(TempSql, CommandType.Text, null));
        }


        /// <summary>数据库中表是否存在</summary>
        /// <param name="aTableName">表名</param>
        /// <returns></returns>
        public override bool TableIsExist(string aTableName)
        {
            List<ProcedureParameter> Par = new List<ProcedureParameter>()
            {
                new ProcedureParameter(){ Name="tablename", ParType= ProcParType.VarChar, Value=aTableName.ToUpper()}
            };
            DataSet Ds = SqlToDataSet("select table_name from user_tables where table_name=@tablename", CommandType.Text, Par);
            if (Ds.Tables[0].Rows.Count > 0)
                return true;

            return false;
        }

        /// <summary>数据库中表是否存在</summary>
        /// <param name="aDbName">数据库名称</param>
        /// <param name="aTableName">表名</param>
        /// <returns></returns>
        public override bool TableIsExist(string aDbName, string aTableName)
        {
            throw new NotImplementedException();
        }

        /// <summary>表中字段是否存在</summary>
        /// <param name="aTableName">表名</param>
        /// <param name="aFieldName">字段名</param>
        /// <returns></returns>
        public override bool FieldIsExist(string aTableName, string aFieldName)
        {
            string TempSql = "select count(*) from all_tab_columns where table_name='" + aTableName + "' and column_name='" + aFieldName + "'";
            object objCount = ExecuteScalar(TempSql, CommandType.Text, null);
            if (objCount != null && objCount.ToString() != "0")
                return true;
            return false;
        }

        /// <summary>从多选字符串MultiChoiceStr转换成Sql条件</summary>
        /// <param name="AMultiChoiceStr">多选字符串</param>
        /// <param name="aField">字段</param>
        /// <param name="aStyle">类型 0-两边都不加  1-%左边加  2-右边加%  3-%两边加%</param>
        /// <param name="aPar">返回参数</param>
        /// <returns></returns>
        public override string GetMcsToSql(string AMultiChoiceStr, string aField, int aStyle, ref List<ProcedureParameter> aPar)
        {
            throw new NotImplementedException();
        }
        /// <summary>批量插入</summary>
        /// <param name="dt">数据源</param>
        /// <param name="datatablename">表名</param>
        /// <param name="batchSize">条数</param>
        public override void SqlBulkCopy(DataTable dt, string datatablename, int batchSize = 0)
        {
            throw new NotImplementedException();
        }
        /// <summary>批量插入</summary>
        /// <param name="dt">数据源</param>
        /// <param name="datatablename">表名</param>
        /// <param name="mapps">列映射</param>
        /// <param name="batchSize">条数</param>
        public override void SqlBulkCopy(DataTable dt, string datatablename, Dictionary<string, string> mapps, int batchSize = 0)
        {
            throw new NotImplementedException();
        }
    }
}
