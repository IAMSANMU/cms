using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Imow.Framework.Db
{
    class DbSqlServer : DbHelper
    {
        public DbSqlServer(string connectionString, bool isWatchTime)
        {
            _connectionstring = connectionString;
            _iswatchtime = isWatchTime;
        }
        /// <summary>参数类型转化</summary>
        /// <param name="par">自定义参数类</param>
        /// <returns>sql参数</returns>
        private SqlParameter ParameterTransform(ProcedureParameter par)
        {
            //支持简易参数写法
            string parName = "@" + par.Name.TrimStart('@');
            /*支持无类型参数*/
            if (par.ParType == ProcParType.Default)
                return new SqlParameter(par.Name, par.Value ?? DBNull.Value);
            SqlParameter p = new SqlParameter();
            p.ParameterName = parName;
            switch (par.ParType)
            {
                case ProcParType.Int16:
                    p.SqlDbType = SqlDbType.SmallInt;
                    break;
                case ProcParType.Int32:
                    p.SqlDbType = SqlDbType.Int;
                    break;
                case ProcParType.Int64:
                    p.SqlDbType = SqlDbType.BigInt;
                    break;
                case ProcParType.Single:
                    p.SqlDbType = SqlDbType.Real;
                    break;
                case ProcParType.Double:
                    p.SqlDbType = SqlDbType.Float;
                    break;
                case ProcParType.Decimal:
                    p.SqlDbType = SqlDbType.Decimal;
                    break;
                case ProcParType.Char:
                    p.SqlDbType = SqlDbType.Char;
                    break;
                case ProcParType.VarChar:
                    p.SqlDbType = SqlDbType.VarChar;
                    break;
                case ProcParType.NVarchar:
                    p.SqlDbType = SqlDbType.NVarChar;
                    break;
                case ProcParType.Image:
                    p.SqlDbType = SqlDbType.Binary;
                    break;
                case ProcParType.DateTime:
                    p.SqlDbType = SqlDbType.DateTime;
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
            DbCommand cmd = new SqlCommand();
            DbTransaction dbTransaction = null;
            try
            {
                cmd.CommandText = sql;
                cmd.CommandType = cmdType;
                //设置参数
                procedurePar = procedurePar ?? new List<ProcedureParameter>();
                foreach (var item in procedurePar)
                {
                    SqlParameter p = ParameterTransform(item);
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
                    conn = new SqlConnection();
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
            DbCommand cmd = new SqlCommand();
            DbTransaction dbTransaction = null;
            try
            {
                cmd.CommandText = sql;
                cmd.CommandType = cmdType;
                //设置参数
                procedurePar = procedurePar ?? new List<ProcedureParameter>();
                foreach (var item in procedurePar)
                {
                    SqlParameter p = ParameterTransform(item);
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
                    conn = new SqlConnection();
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
            DbCommand cmd = new SqlCommand();
            try
            {

                cmd.CommandText = sql;
                cmd.CommandType = cmdType;
                //设置参数
                procedurePar = procedurePar ?? new List<ProcedureParameter>();
                foreach (var item in procedurePar)
                {
                    SqlParameter p = ParameterTransform(item);
                    cmd.Parameters.Add(p);
                }
                conn = new SqlConnection();
                conn.ConnectionString = _connectionstring;
                cmd.Connection = conn;
                conn.Open();
                DbDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
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
            DbCommand cmd = new SqlCommand();
            DbDataAdapter adapter = new SqlDataAdapter();
            try
            {

                cmd.CommandText = sql;
                cmd.CommandType = cmdType;
                //设置参数
                procedurePar = procedurePar ?? new List<ProcedureParameter>();
                foreach (var item in procedurePar)
                {
                    SqlParameter p = ParameterTransform(item);
                    cmd.Parameters.Add(p);
                }
                conn = new SqlConnection();
                conn.ConnectionString = _connectionstring;
                cmd.Connection = conn;
                conn.Open();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
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
            DataSet ds = SqlToDataSet("select @@identity", CommandType.Text, null);
            if (ds.Tables[0].Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return LibConvert.ObjToInt(ds.Tables[0].Rows[0][0]);
            }
        }

        /// <summary>取得数据库当前系统时间</summary>
        /// <returns></returns>
        public override DateTime GetServerDate()
        {
            DateTime result = new DateTime(0);
            string TempSql = "select GetDate() as aDate";
            SqlDataReader drSql = SqlToDbReader(TempSql, null) as SqlDataReader;
            if (drSql != null && drSql.Read())
                result = drSql.GetDateTime(0);
            drSql.Close();
            return result;
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
            DataSet Ds = SqlToDataSet("Select name from sysobjects where Name=@tablename", CommandType.Text, Par);
            if (Ds.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;

        }

        /// <summary>数据库中表是否存在</summary>
        /// <param name="aDbName">数据库名称</param>
        /// <param name="aTableName">表名</param>
        /// <returns></returns>
        public override bool TableIsExist(string aDbName, string aTableName)
        {
            DataSet Ds = SqlToDataSet("Select name from " + aDbName + "..sysobjects where Name='" + aTableName + "'", CommandType.Text, null);
            if (Ds.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>表中字段是否存在</summary>
        /// <param name="aTableName">表名</param>
        /// <param name="aFieldName">字段名</param>
        /// <returns></returns>
        public override bool FieldIsExist(string aTableName, string aFieldName)
        {
            string TempSql = "select top 0 * from " + aTableName;
            DataSet ds = SqlToDataSet(TempSql, CommandType.Text, null);
            if (ds.Tables[0].Columns.IndexOf(aFieldName) > 0)
                return true;
            else
                return false;

        }
        private static long _tick = DateTime.Now.Ticks;
        /// <summary>从多选字符串MultiChoiceStr转换成Sql条件</summary>
        /// <param name="AMultiChoiceStr">多选字符串</param>
        /// <param name="aField">字段</param>
        /// <param name="aStyle">类型 0-两边都不加  1-%左边加  2-右边加%  3-%两边加%</param>
        /// <param name="aPar">返回参数</param>
        /// <returns></returns>
        public override string GetMcsToSql(string AMultiChoiceStr, string aField, int aStyle, ref List<ProcedureParameter> aPar)
        {
            if (aPar == null) aPar = new List<ProcedureParameter>();
            if (AMultiChoiceStr == "") return "1=1";
            string[] SqlSz = AMultiChoiceStr.Split(",，".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string sRet = "";
            for (int i = 0; i < SqlSz.Length; i++)
            {
                string TmpStr = "";
                int pos = SqlSz[i].IndexOf("|");
                if (pos == -1)
                {
                    long _t = _tick;
                    _tick++;
                    aPar.Add(new Db.ProcedureParameter("@find_sql_" + _t.ToString(), Db.ProcParType.VarChar, 255, SqlSz[i]));
                    if (aStyle == 0)
                        TmpStr = aField + " Like @find_sql_" + _t.ToString();
                    else if (aStyle == 1)
                        TmpStr = aField + " Like '%'+@find_sql_" + _t.ToString();
                    else if (aStyle == 2)
                        TmpStr = aField + " Like @find_sql_" + _t.ToString() + "+'%'";
                    else if (aStyle == 3)
                        TmpStr = aField + " Like '%'+@find_sql_" + _t.ToString() + "+'%'";
                }
                else
                {
                    long _t1 = _tick;
                    _tick++;
                    long _t2 = _tick;
                    _tick++;
                    string str1 = SqlSz[i].Substring(0, pos);
                    string str2 = SqlSz[i].Substring(pos + 1, SqlSz[i].Length - pos - 1);
                    aPar.Add(new Db.ProcedureParameter("@find_sql_" + _t1.ToString(), Db.ProcParType.VarChar, 255, str1));
                    TmpStr = "(left(" + aField + "," + str1.Length + ")>=@find_sql_" + _t1.ToString();
                    aPar.Add(new Db.ProcedureParameter("@find_sql_" + _t2.ToString(), Db.ProcParType.VarChar, 255, str2));
                    TmpStr = TmpStr + " and left(" + aField + "," + str2.Length + ")<=@find_sql_" + _t2.ToString() + ")";
                }
                if (sRet == "")
                    sRet = TmpStr;
                else
                    sRet = sRet + " or " + TmpStr;
            }
            return "(" + sRet + ")";
        }
        /// <summary>批量插入</summary>
        /// <param name="dt">数据源</param>
        /// <param name="datatablename">表名</param>
        /// <param name="batchSize">条数</param>
        public override void SqlBulkCopy(DataTable dt, string datatablename,  int batchSize = 0)
        {
            SqlBulkCopy sqlBulkCopy = null;
            try
            {
                //启用事务
                if (null != Imow.Framework.Db.Transaction.Current)
                {
                    SqlConnection sqlConn = Imow.Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction.Connection as SqlConnection;
                    SqlTransaction sqlTs = Imow.Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction as SqlTransaction;

                    sqlBulkCopy = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.Default, sqlTs);
                }
                else
                {
                    sqlBulkCopy = new SqlBulkCopy(_connectionstring, SqlBulkCopyOptions.Default);
                }

                sqlBulkCopy.DestinationTableName = datatablename;
                if (batchSize > 0)
                {
                    sqlBulkCopy.BatchSize = batchSize;
                }
                sqlBulkCopy.WriteToServer(dt);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (null != sqlBulkCopy)
                {
                    sqlBulkCopy.Close();
                }
            }


        }
        /// <summary>批量插入</summary>
        /// <param name="dt">数据源</param>
        /// <param name="datatablename">表名</param>
        /// <param name="procedurePar">参数</param>
        /// <param name="mapps">列映射</param>
        /// <param name="batchSize">条数</param>
        public override void SqlBulkCopy(DataTable dt, string datatablename, Dictionary<string, string> mapps, int batchSize = 0)
        {
            SqlBulkCopy sqlBulkCopy = null;
            try
            {
                //启用事务
                if (null != Imow.Framework.Db.Transaction.Current)
                {
                    SqlConnection sqlConn = Imow.Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction.Connection as SqlConnection;
                    SqlTransaction sqlTs = Imow.Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction as SqlTransaction;
                    sqlBulkCopy = new SqlBulkCopy(sqlConn, SqlBulkCopyOptions.Default, sqlTs);
                }
                else
                {
                    sqlBulkCopy = new SqlBulkCopy(_connectionstring, SqlBulkCopyOptions.Default);
                }

                sqlBulkCopy.DestinationTableName = datatablename;
                foreach (var mapp in mapps)
                {
                    sqlBulkCopy.ColumnMappings.Add(mapp.Key, mapp.Value);
                }

                if (batchSize > 0)
                {
                    sqlBulkCopy.BatchSize = batchSize;
                }
                sqlBulkCopy.WriteToServer(dt);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (null != sqlBulkCopy)
                {
                    sqlBulkCopy.Close();
                }
            }

        }
    }
}
