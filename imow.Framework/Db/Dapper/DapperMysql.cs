using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using DapperExtensions;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using ServiceStack;

namespace Imow.Framework.Db.Dapper
{
     class DapperMysql: DapperHelper
    {
        public DapperMysql(string connectionstring)
        {
            ConnectionString = connectionstring;
        }

        #region DapperExtensions的扩展方法(不支持sql语句)
        /// <summary>
        ///  根据Id获取实体
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="primaryId">主键ID</param>
        /// <returns></returns>
        public override T GetById<T>(dynamic primaryId)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Get<T>(primaryId as object, dbType: DbType.MYSQL);
            }
        }
        /// <summary>
        /// 根据多个Id获取多个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public override IEnumerable<T> GetByIds<T>(IList<dynamic> ids)
        {
            string tblName = string.Format("{0}", typeof(T).Name);
            string idsin = string.Join(",", ids.ToArray());
            string sql = "SELECT * FROM :table WHERE Id in (:ids)";
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                IEnumerable<T> dataList = conn.Query<T>(sql, new { table = tblName, ids = idsin });
                return dataList;
            }
        }
        /// <summary>
        ///获取全部数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override IEnumerable<T> GetAll<T>()
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.GetList<T>(dbType: DbType.MYSQL);
            }

        }
        /// <summary>
        /// 统计记录总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public override int Count<T>(object predicate, bool buffered = false)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Count<T>(predicate, dbType: DbType.MYSQL);
            }

        }
        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="sort">排序</param>
        /// <param name="buffered">缓存</param>
        /// <returns></returns>
        public override IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null, bool buffered = false)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.GetList<T>(predicate, sort, null, null, buffered, dbType: DbType.MYSQL);
            }
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="allRowsCount"></param>
        /// <param name="predicate"></param>
        /// <param name="sort"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public override IEnumerable<T> GetPageList<T>(int pageIndex, int pageSize, out long allRowsCount, object predicate = null,
            IList<ISort> sort = null, bool buffered = true)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                if (sort == null)
                {
                    sort = new List<ISort>();
                }
                IEnumerable<T> entityList = conn.GetPage<T>(predicate, sort, pageIndex, pageSize, null, null,
                    buffered, dbType: DbType.MYSQL);
                allRowsCount = conn.Count<T>(predicate, dbType: DbType.MYSQL);
                return entityList;
            }
        }

         public override IEnumerable<T> GetPageList<T>(string innerSql, int pageIndex, int pageSize, out long allRowsCount, object predicate = null,
             IList<ISort> sort = null, bool buffered = true)
         {
             throw new NotImplementedException();
         }

         public override IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TFourth, TReturn>(int pageIndex, int pageSize, out long allRowsCount,
             Func<TFirst, TSecond, TThird, TFourth, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, JoinType joinType = JoinType.Inner, string splitOn = "id",
             bool buffered = true)
         {
             throw new NotImplementedException();
         }

         public override IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TFourth, TFive, TReturn>(int pageIndex, int pageSize,
             out long allRowsCount, Func<TFirst, TSecond, TThird, TFourth, TFive, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, JoinType joinType = JoinType.Inner,
             string splitOn = "id", bool buffered = true)
         {
             throw new NotImplementedException();
         }
        public override IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TFourth, TFive,TSix, TReturn>(int pageIndex, int pageSize,
          out long allRowsCount, Func<TFirst, TSecond, TThird, TFourth, TFive, TSix, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, JoinType joinType = JoinType.Inner,
          string splitOn = "id", bool buffered = true)
        {
            throw new NotImplementedException();
        }


        public override IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TReturn>(int pageIndex, int pageSize, out long allRowsCount, Func<TFirst, TSecond, TThird, TReturn> map,
             IPredicate predicate = null, IList<ISort> sort = null, JoinType joinType = JoinType.Inner, string splitOn = "id",
             bool buffered = true)
         {
             throw new NotImplementedException();
         }

         public override IEnumerable<TReturn> GetPageList<TFirst, TSecond, TReturn>(int pageIndex, int pageSize, out long allRowsCount, Func<TFirst, TSecond, TReturn> map,
             IPredicate predicate = null, IList<ISort> sort = null, JoinType joinType = JoinType.Inner, string splitOn = "id",
             bool buffered = true)
         {
             throw new NotImplementedException();
         }

         /// <summary>
        /// 插入单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override dynamic Insert<T>(T entity)
        {
            dynamic result = null;
            //启用事务
            if (null != Imow.Framework.Db.Transaction.Current)
            {
                result = Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction.Connection.Insert(entity, transaction: Imow.Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction, dbType: DbType.MYSQL);
            }
            else
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    result = conn.Insert(entity, transaction: null, dbType: DbType.MYSQL);
                }
            }
            return result;
        }
        /// <summary>
        /// 更新单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Update<T>(T entity)
        {

            bool isOk;
            //启用事务
            if (null != Imow.Framework.Db.Transaction.Current)
            {
                isOk = Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction.Connection.Update(entity, Imow.Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction, dbType: DbType.MYSQL);
            }
            else
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    isOk = conn.Update(entity, null, dbType: DbType.MYSQL);
                }
            }
            return isOk;
        }
        /// <summary>
        ///删除单条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="primaryId"></param>
        /// <returns></returns>
        public override int Delete<T>(dynamic primaryId)
        {
            var entity = GetById<T>(primaryId);
            var obj = entity as T;
            int isOk;
            //启用事务
            if (null != Imow.Framework.Db.Transaction.Current)
            {
                isOk = Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction.Connection.Delete(obj, Imow.Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction, dbType: DbType.MYSQL);
            }
            else
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    isOk = conn.Delete(obj, null, dbType: DbType.MYSQL);
                }
            }
            return isOk;
        }
        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override int DeleteList<T>(object predicate = null)
        {
            int delNum = 0;
            //启用事务
            if (null != Imow.Framework.Db.Transaction.Current)
            {
                delNum = Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction.Connection.Delete<T>(predicate, Imow.Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction, dbType: DbType.MYSQL);
            }
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    delNum = conn.Delete<T>(predicate, null, dbType: DbType.MYSQL);
                }
            }
            return delNum;
        }

        /// <summary>
        /// 批量插入功能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        public override bool InsertBatch<T>(IEnumerable<T> entityList)
        {
            bool isOk = false;
            foreach (T item in entityList)
            {
                Insert(item);
            }
            isOk = true;
            return isOk;
        }
        /// <summary>
        ///批量更新（）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        /// <returns></returns>
        public override bool UpdateBatch<T>(IEnumerable<T> entityList)
        {
            bool isOk = false;
            foreach (T item in entityList)
            {
                Update(item);
            }
            isOk = true;
            return isOk;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public override bool DeleteBatch<T>(IEnumerable<dynamic> ids)
        {
            bool isOk = false;
            foreach (dynamic id in ids)
            {
                Delete<T>(id);
            }
            isOk = true;
            return isOk;
        }


        #endregion

        #region Dapper自己的方法（sql语句查询）
        /// <summary>
        /// 根据条件筛选出数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public override IEnumerable<T> Get<T>(string sql, dynamic param = null, bool buffered = true)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query<T>(sql, param as object, null, buffered);
            }
        }
        /// <summary>
        /// 根据条件筛选数据集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public override IEnumerable<dynamic> Get(string sql, dynamic param = null, bool buffered = true)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query(sql, param as object, null, buffered);
            }
        }

         public override IEnumerable<TReturn> Get<TFirst, TSecond, TReturn>(Func<TFirst, TSecond, TReturn> map, JoinType joinType, IPredicate predicate = null, bool buffered = true,
             string splitOn = "Id", int? commandTimeout = null)
         {
             throw new NotImplementedException();
         }

         public override IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TFourth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TReturn> map, JoinType joinType, IPredicate predicate = null,
             bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
         {
             throw new NotImplementedException();
         }

         public override IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFive, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFive, TReturn> map, JoinType joinType,
             IPredicate predicate = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
         {
             throw new NotImplementedException();
         }

        public override IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFive, TSix, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFive, TSix, TReturn> map, JoinType joinType,
            IPredicate predicate = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> map, JoinType joinType, IPredicate predicate = null,
             bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
         {
             throw new NotImplementedException();
         }

         /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="allRowsCount"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="allRowsCountSql"></param>
        /// <param name="allRowsCountParam"></param>
        /// <param name="buffered"></param>
        /// <returns></returns>
        public  IEnumerable<T> GetPage<T>(int pageIndex, int pageSize, out long allRowsCount, string sql, dynamic param = null,
            string allRowsCountSql = null, dynamic allRowsCountParam = null, bool buffered = true)
        {
            throw new NotImplementedException();

        }

        /// <summary>
        /// 根据表达式筛选
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public override IEnumerable<TReturn> Get<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, dynamic param = null, bool buffered = true,
            string splitOn = "Id", int? commandTimeout = null)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query(sql, map, param as object, null, buffered, splitOn);
            }
        }
        /// <summary>
        /// 根据表达式筛选
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="sql"></param>
        /// <param name="map"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public override IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, dynamic param = null, bool buffered = true,
            string splitOn = "Id", int? commandTimeout = null)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Query(sql, map, param as object, null, buffered, splitOn);
            }
        }

         public override IEnumerable<TReturn> GetPageList<TFirst, TSecond, TReturn>(string sql, int pageIndex, int pageSize, out long allRowsCount,
             Func<TFirst, TSecond, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, string splitOn = "id", bool buffered = true)
         {
             throw new NotImplementedException();
         }

         public override IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TReturn>(string sql, int pageIndex, int pageSize, out long allRowsCount,
             Func<TFirst, TSecond, TThird, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, string splitOn = "id", bool buffered = true)
         {
             throw new NotImplementedException();
         }

         public override IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, int pageIndex, int pageSize,
             out long allRowsCount, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, string splitOn = "id",
             bool buffered = true)
         {
             throw new NotImplementedException();
         }

         /// <summary>
        /// 获取多实体集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public override SqlMapper.GridReader GetMultiple(string sql, dynamic param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.QueryMultiple(sql, param as object, null, commandTimeout, commandType);
            }

        }
        /// <summary>
        /// 执行sql操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public override int Execute(string sql, dynamic param = null)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                return conn.Execute(sql, param as object, null);
            }

        }

         public override string GetPredictSql(IPredicate predicate, out SqlSimpleProcedureParameter parameters)
         {
             throw new NotImplementedException();
         }

         #endregion
    }
}