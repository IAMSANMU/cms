using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using imow.Core.config;
using Imow.Framework.Engine;
using StackExchange.Profiling;

namespace Imow.Framework.Db
{
    class DapperSql : DapperHelper
    {
        private bool _profiler;

        public DapperSql(string connectionstring)
        {
            _profiler = ImowEngineContext.Current.ResolveConfig<ImowConfig>().Profiler;
            ConnectionString = connectionstring;
        }

        public DbConnection GetSqlConnection()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            if (_profiler) return new StackExchange.Profiling.Data.ProfiledDbConnection(conn, MiniProfiler.Current);
            return conn;
        }


        #region 传sql语句查询
        /// <summary>
        ///  根据Id获取实体
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="primaryId">主键ID</param>
        /// <returns></returns>
        public override T GetById<T>(dynamic primaryId)
        {

            using (DbConnection conn = GetSqlConnection())
            {
                conn.Open();
                return conn.Get<T>(primaryId as object, dbType: DbType.SQLSERVER);
            }
        }

        /// <summary>
        /// 根据多个Id获取多个实体
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public override IEnumerable<T> GetByIds<T>(IList<dynamic> ids)
        {
            string tblName = string.Format("dbo.{0}", typeof(T).Name);
            string idsin = string.Join(",", ids.ToArray());
            string sql = "SELECT * FROM @table WHERE Id in (@ids)";
            using (DbConnection conn = GetSqlConnection())
            {
                conn.Open();
                IEnumerable<T> dataList = conn.Query<T>(sql, new { table = tblName, ids = idsin });
                return dataList;
            }
        }
        /// <summary>
        ///获取全部数据集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public override IEnumerable<T> GetAll<T>()
        {
            using (DbConnection conn = GetSqlConnection())
            {
                conn.Open();
                return conn.GetList<T>(dbType: DbType.SQLSERVER);
            }

        }
        /// <summary>
        /// 统计记录总数
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns></returns>
        public override int Count<T>(object predicate, bool buffered = false)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                conn.Open();
                return conn.Count<T>(predicate, dbType: DbType.SQLSERVER);
            }

        }
        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="sort">排序</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns></returns>
        public override IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null, bool buffered = false)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                conn.Open();
                return conn.GetList<T>(predicate, sort, null, null, buffered, dbType: DbType.SQLSERVER);
            }
        }
        /// <summary>
        /// 单表分页
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="pageIndex">pageIndex</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="allRowsCount">out 返回总记录数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns></returns>
        public override IEnumerable<T> GetPageList<T>(int pageIndex, int pageSize, out long allRowsCount, object predicate = null,
            IList<ISort> sort = null, bool buffered = true)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                conn.Open();
                if (sort == null)
                {
                    sort = new List<ISort>();
                }
                IEnumerable<T> entityList = conn.GetPage<T>(predicate, sort, pageIndex, pageSize, null, null,
                    buffered, dbType: DbType.SQLSERVER);
                allRowsCount = conn.Count<T>(predicate, dbType: DbType.SQLSERVER);
                return entityList;
            }
        }

        /// <summary>
        /// 单表分页
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="innerSql"></param>
        /// <param name="pageIndex">pageIndex</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="allRowsCount">out 返回总记录数</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns></returns>
        public override IEnumerable<T> GetPageList<T>(string innerSql, int pageIndex, int pageSize, out long allRowsCount, object predicate = null,
            IList<ISort> sort = null, bool buffered = true)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                conn.Open();
                if (sort == null)
                {
                    sort = new List<ISort>();
                }
                IEnumerable<T> entityList = conn.GetPage<T>(innerSql, predicate, sort, pageIndex, pageSize, null, null,
                    buffered, dbType: DbType.SQLSERVER);
                allRowsCount = conn.Count<T>(innerSql,predicate, dbType: DbType.SQLSERVER);
                return entityList;
            }
        }

        public override IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TFourth, TReturn>(int pageIndex, int pageSize, out long allRowsCount,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, JoinType joinType = JoinType.Inner, string splitOn = "id",
            bool buffered = true)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                if (sort == null)
                {
                    sort = new List<ISort>();
                }
                IEnumerable<TReturn> entityList = conn.GetPage(map, joinType, splitOn, predicate, sort, pageIndex, pageSize, null, null,
                    buffered, dbType: DbType.SQLSERVER);
                allRowsCount = conn.Count<TFirst, TSecond, TThird, TFourth>(joinType, predicate, dbType: DbType.SQLSERVER);
                return entityList;
            }
        }

        public override IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TFourth, TFive, TReturn>(int pageIndex, int pageSize, out long allRowsCount,
        Func<TFirst, TSecond, TThird, TFourth, TFive, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, JoinType joinType = JoinType.Inner, string splitOn = "id",
        bool buffered = true)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                if (sort == null)
                {
                    sort = new List<ISort>();
                }
                IEnumerable<TReturn> entityList = conn.GetPage(map, joinType, splitOn, predicate, sort, pageIndex, pageSize, null, null,
                    buffered, dbType: DbType.SQLSERVER);
                allRowsCount = conn.Count<TFirst, TSecond, TThird, TFourth, TFive>(joinType, predicate, dbType: DbType.SQLSERVER);
                return entityList;
            }
        }
        public override IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TFourth, TFive, TSix, TReturn>(int pageIndex, int pageSize, out long allRowsCount,
      Func<TFirst, TSecond, TThird, TFourth, TFive, TSix, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, JoinType joinType = JoinType.Inner, string splitOn = "id",
      bool buffered = true)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                if (sort == null)
                {
                    sort = new List<ISort>();
                }
                IEnumerable<TReturn> entityList = conn.GetPage(map, joinType, splitOn, predicate, sort, pageIndex, pageSize, null, null,
                    buffered, dbType: DbType.SQLSERVER);
                allRowsCount = conn.Count<TFirst, TSecond, TThird, TFourth, TFive>(joinType, predicate, dbType: DbType.SQLSERVER);
                return entityList;
            }
        }

        public override IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TReturn>(int pageIndex, int pageSize, out long allRowsCount, Func<TFirst, TSecond, TThird, TReturn> map,
            IPredicate predicate = null, IList<ISort> sort = null, JoinType joinType = JoinType.Inner, string splitOn = "id",
            bool buffered = true)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                if (sort == null)
                {
                    sort = new List<ISort>();
                }
                IEnumerable<TReturn> entityList = conn.GetPage(map, joinType, splitOn, predicate, sort, pageIndex, pageSize, null, null,
                    buffered, dbType: DbType.SQLSERVER);
                allRowsCount = conn.Count<TFirst, TSecond, TThird>(joinType, predicate, dbType: DbType.SQLSERVER);
                return entityList;
            }
        }

        /// <summary>
        /// 两张表连接分页
        /// </summary>
        /// <typeparam name="TFirst">第一个类型（实体类名称）</typeparam>
        /// <typeparam name="TSecond">第二个类型（实体类名称）</typeparam>
        /// <typeparam name="TReturn">返回的类型（实体类名称）</typeparam>
        /// <param name="pageIndex">pageIndex</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="allRowsCount">out 返回总记录数</param>
        /// <param name="map">映射表达式</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <param name="joinType">表连接枚举类型  （Inner、Left、Right）等</param>
        /// <param name="splitOn">读取第二个对象的的分割列，从哪个列起开始读取第二个对象，如果表里的自增长列都为Id,可以设置这个参数为”Id”</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns>实体类</returns>
        public override IEnumerable<TReturn> GetPageList<TFirst, TSecond, TReturn>(int pageIndex, int pageSize, out long allRowsCount, Func<TFirst, TSecond, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, JoinType joinType = JoinType.Inner, string splitOn = "id", bool buffered = true)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                if (sort == null)
                {
                    sort = new List<ISort>();
                }
                IEnumerable<TReturn> entityList = conn.GetPage(map, joinType, splitOn, predicate, sort, pageIndex, pageSize, null, null,
                    buffered, dbType: DbType.SQLSERVER);
                allRowsCount = conn.Count<TFirst, TSecond>(joinType, predicate, dbType: DbType.SQLSERVER);
                return entityList;
            }
        }


        /// <summary>
        /// 插入单条记录
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public override dynamic Insert<T>(T entity)
        {
            dynamic result = null;
            //启用事务
            if (null != Imow.Framework.Db.Transaction.Current)
            {
                result = Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction.Connection.Insert(entity, transaction: Imow.Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction, dbType: DbType.SQLSERVER);
            }
            else
            {
                using (DbConnection conn = GetSqlConnection())
                {
                    conn.Open();
                    result = conn.Insert(entity, transaction: null, dbType: DbType.SQLSERVER);
                }
            }
            return result;
        }
        /// <summary>
        /// 更新单条记录
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public override bool Update<T>(T entity)
        {

            bool isOk;
            //启用事务
            if (null != Imow.Framework.Db.Transaction.Current)
            {
                isOk = Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction.Connection.Update(entity, Imow.Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction, dbType: DbType.SQLSERVER);
            }
            else
            {
                using (DbConnection conn = GetSqlConnection())
                {
                    conn.Open();
                    isOk = conn.Update(entity, null, dbType: DbType.SQLSERVER);
                }
            }
            return isOk;
        }
        /// <summary>
        ///删除单条记录
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="primaryId">主键ID</param>
        /// <returns></returns>
        public override int Delete<T>(dynamic primaryId)
        {
            var entity = GetById<T>(primaryId);
            var obj = entity as T;
            int isOk;
            //启用事务
            if (null != Imow.Framework.Db.Transaction.Current)
            {
                isOk = Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction.Connection.Delete(obj, Imow.Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction, dbType: DbType.SQLSERVER);
            }
            else
            {
                using (DbConnection conn = GetSqlConnection())
                {
                    conn.Open();
                    isOk = conn.Delete(obj, null, dbType: DbType.SQLSERVER);
                }
            }
            return isOk;
        }
        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public override int DeleteList<T>(object predicate = null)
        {
            int delNum = 0;
            //启用事务
            if (null != Imow.Framework.Db.Transaction.Current)
            {
                delNum = Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction.Connection.Delete<T>(predicate, Imow.Framework.Db.Transaction.Current.DbTransactionWrapper.DbTransaction, dbType: DbType.SQLSERVER);
            }
            else
            {
                using (DbConnection conn = GetSqlConnection())
                {
                    conn.Open();
                    delNum = conn.Delete<T>(predicate, null, dbType: DbType.SQLSERVER);
                }
            }
            return delNum;
        }
        /// <summary>
        /// 批量插入功能
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entityList">实体类集合</param>
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
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entityList">实体类集合</param>
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
        /// <typeparam name="T">类型</typeparam>
        /// <param name="ids">主键ID集合</param>
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

        #region 传sql语句查询
        /// <summary>
        /// 根据条件筛选出数据集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="param">SQL参数 匿名类型 如  new{name="张三"} name为sql参数</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns></returns>
        public override IEnumerable<T> Get<T>(string sql, dynamic param = null, bool buffered = true)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                conn.Open();
                return conn.Query<T>(sql, param as object, null, buffered);
            }
        }
        /// <summary>
        /// 根据条件筛选数据集合
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">SQL参数 匿名类型 如  new{name="张三"} name为sql参数</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns></returns>
        public override IEnumerable<dynamic> Get(string sql, dynamic param = null, bool buffered = true)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                conn.Open();
                return conn.Query(sql, param as object, null, buffered);
            }
        }

        public override IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TFourth, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TReturn> map, JoinType joinType, IPredicate predicate = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                IEnumerable<TReturn> entityList = conn.Get(map, joinType, splitOn, predicate, null, null,
                   buffered, dbType: DbType.SQLSERVER);
                return entityList;
            }
        }

        public override IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFive, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFive, TReturn> map, JoinType joinType, IPredicate predicate = null,
          bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                IEnumerable<TReturn> entityList = conn.Get(map, joinType, splitOn, predicate, null, null,
                   buffered, dbType: DbType.SQLSERVER);
                return entityList;
            }
        }

        public override IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFive, TSix, TReturn>(Func<TFirst, TSecond, TThird, TFourth, TFive, TSix, TReturn> map, JoinType joinType,
            IPredicate predicate = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                IEnumerable<TReturn> entityList = conn.Get(map, joinType, splitOn, predicate, null, null,
                   buffered, dbType: DbType.SQLSERVER);
                return entityList;
            }
        }

        public override IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> map, JoinType joinType, IPredicate predicate = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                IEnumerable<TReturn> entityList = conn.Get(map, joinType, splitOn, predicate, null, null,
                   buffered, dbType: DbType.SQLSERVER);
                return entityList;
            }
        }

        /// <summary>
        /// 两张表连接查询
        /// </summary>
        /// <typeparam name="TFirst">第一个类型（实体类名称）</typeparam>
        /// <typeparam name="TSecond">第二个类型（实体类名称）</typeparam>
        /// <typeparam name="TReturn">返回的类型（实体类名称）</typeparam>
        /// <param name="sql">连接查询SQL语句</param>
        /// <param name="map">映射表达式</param>
        /// <param name="param">SQL参数 匿名类型 如  new{name="张三"} name为sql参数</param>
        /// <param name="buffered">是否缓存</param>
        /// <param name="splitOn">读取第二个对象的的分割列，从哪个列起开始读取第二个对象，如果表里的自增长列都为Id,可以设置这个参数为”Id”</param>
        /// <param name="commandTimeout">command超时时间</param>
        /// <returns>实体类</returns>
        public override IEnumerable<TReturn> Get<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, dynamic param = null, bool buffered = true,
            string splitOn = "Id", int? commandTimeout = null)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                conn.Open();
                return conn.Query(sql, map, param as object, null, buffered, splitOn);
            }
        }
        /// <summary>
        /// 三张表连接查询
        /// </summary>
        /// <typeparam name="TFirst">第一个类型（实体类名称）</typeparam>
        /// <typeparam name="TSecond">第二个类型（实体类名称）</typeparam>
        /// <typeparam name="TThird">第三个（实体类名称）</typeparam>
        /// <typeparam name="TReturn">返回的类型（实体类名称）</typeparam>
        /// <param name="sql">连接查询SQL语句</param>
        /// <param name="map">映射表达式</param>
        /// <param name="param">SQL参数 匿名类型 如  new{name="张三"} name为sql参数</param>
        /// <param name="buffered">是否缓存</param>
        /// <param name="splitOn">读取第二个对象的的分割列，从哪个列起开始读取第二个对象，如果表里的自增长列都为Id,可以设置这个参数为”Id”</param>
        /// <param name="commandTimeout">command超时时间</param>
        /// <returns>实体类</returns>
        public override IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, dynamic param = null, bool buffered = true,
            string splitOn = "Id", int? commandTimeout = null)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                conn.Open();
                return conn.Query(sql, map, param as object, null, buffered, splitOn);
            }
        }

        /// <summary>
        /// 两张表连接分页查询
        /// </summary>
        /// <param name="sql">sql语句(请使用.*,join连写的sql语句)</param>
        /// <param name="pageIndex">pageIndex</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="allRowsCount">out 返回总记录数</param>
        /// <param name="map">映射表达式</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <param name="splitOn">读取第二个对象的的分割列，从哪个列起开始读取第二个对象，如果表里的自增长列都为Id,可以设置这个参数为”Id”</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns></returns>
        public override IEnumerable<TReturn> GetPageList<TFirst, TSecond, TReturn>(string sql, int pageIndex, int pageSize, out long allRowsCount, Func<TFirst, TSecond, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, string splitOn = "id", bool buffered = true)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                var entityList = conn.GetPage(sql, map, splitOn, predicate, sort, pageIndex, pageSize, out allRowsCount, null, null, buffered, dbType: DbType.SQLSERVER);
                return entityList;
            }
        }
        /// <summary>
        /// 三张表连接分页查询
        /// </summary>
        /// <param name="sql">sql语句(请使用.*,join连写的sql语句)</param>
        /// <param name="pageIndex">pageIndex</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="allRowsCount">out 返回总记录数</param>
        /// <param name="map">映射表达式</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <param name="splitOn">读取第二个对象的的分割列，从哪个列起开始读取第二个对象，如果表里的自增长列都为Id,可以设置这个参数为”Id”</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns></returns>
        public override IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TReturn>(string sql, int pageIndex, int pageSize, out long allRowsCount, Func<TFirst, TSecond, TThird, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, string splitOn = "id", bool buffered = true)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                var entityList = conn.GetPage(sql, map, splitOn, predicate, sort, pageIndex, pageSize, out allRowsCount, null, null, buffered, dbType: DbType.SQLSERVER);
                return entityList;
            }
        }
        /// <summary>
        /// 四张表连接分页查询
        /// </summary>
        /// <param name="sql">sql语句(请使用.*,join连写的sql语句)</param>
        /// <param name="pageIndex">pageIndex</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="allRowsCount">out 返回总记录数</param>
        /// <param name="map">映射表达式</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="sort">排序条件</param>
        /// <param name="splitOn">读取第二个对象的的分割列，从哪个列起开始读取第二个对象，如果表里的自增长列都为Id,可以设置这个参数为”Id”</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns></returns>
        public override IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, int pageIndex, int pageSize, out long allRowsCount, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, string splitOn = "id", bool buffered = true)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                var entityList = conn.GetPage(sql, map, splitOn, predicate, sort, pageIndex, pageSize, out allRowsCount, null, null, buffered, dbType: DbType.SQLSERVER);
                return entityList;
            }
        }

        public override IEnumerable<TReturn> Get<TFirst, TSecond, TReturn>(Func<TFirst, TSecond, TReturn> map, JoinType joinType, IPredicate predicate = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = default(int?))
        {
            using (DbConnection conn = GetSqlConnection())
            {
                IEnumerable<TReturn> entityList = conn.Get(map, joinType, splitOn, predicate, null, null,
                   buffered, dbType: DbType.SQLSERVER);
                return entityList;
            }
        }


        /// <summary>
        /// 获取多实体集合
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">SQL参数 匿名类型 如  new{name="张三"} name为sql参数</param>
        /// <param name="commandTimeout">command超时时间</param>
        /// <param name="commandType">command类型</param>
        /// <returns></returns>
        public override SqlMapper.GridReader GetMultiple(string sql, dynamic param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                conn.Open();
                return conn.QueryMultiple(sql, param as object, null, commandTimeout, commandType);
            }

        }
        /// <summary>
        /// 执行sql操作
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">SQL参数 匿名类型 如  new{name="张三"} name为sql参数</param>
        /// <returns></returns>
        public override int Execute(string sql, dynamic param = null)
        {
            using (DbConnection conn = GetSqlConnection())
            {
                conn.Open();
                return conn.Execute(sql, param as object, null);
            }

        }


        public override string GetPredictSql(IPredicate predicate,out SqlSimpleProcedureParameter parameters)
        {
            SqlSimpleProcedureParameter item = new SqlSimpleProcedureParameter();
            var result= DapperExtensions.DapperExtensions.GetPredictSql(predicate, item);
            parameters = item;
            return result;
        }

        #endregion

    }
}
