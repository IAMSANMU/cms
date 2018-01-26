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
using Imow.Framework.Db.Dapper;
using Oracle.ManagedDataAccess.Client;

namespace Imow.Framework.Db
{
    public abstract class DapperHelper
    {
        /// <summary>数据库连接字符串</summary>
        protected string ConnectionString;

        /// <summary>是否监控sql耗时HH:ss:mm.fff</summary>
        protected bool Iswatchtime = true;

        /// <summary>
        /// 创建数据访问对象
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns></returns>
        public static DapperHelper CreateHelper(string connectionString)
        {
            string connectionstring = connectionString;
            DbType dbType = DbConfig.GetDbType(ref connectionstring);
            switch (dbType)
            {
                case DbType.SQLSERVER:
                    return new DapperSql(connectionstring);
                case DbType.ORACLE:
                    return new DapperOracle(connectionstring);
                case DbType.MYSQL:
                    return new DapperMysql(connectionstring);
                default:
                    throw new Exception("暂无改数据库类型连接，联系开发人员添加该数据库支持！");
            }
        }

        #region 传实体查询(不支持sql语句)
        /// <summary>
        ///  根据Id获取实体
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="primaryId">主键ID</param>
        /// <returns></returns>
        public abstract T GetById<T>(dynamic primaryId) where T : class;

        /// <summary>
        /// 根据多个Id获取多个实体
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public abstract IEnumerable<T> GetByIds<T>(IList<dynamic> ids) where T : class;

        /// <summary>
        ///获取全部数据集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        public abstract IEnumerable<T> GetAll<T>() where T : class;

        /// <summary>
        /// 统计记录总数
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns></returns>
        public abstract int Count<T>(object predicate, bool buffered = false) where T : class;

        /// <summary>
        /// 查询列表数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="sort">排序</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns></returns>
        public abstract IEnumerable<T> GetList<T>(object predicate = null, IList<ISort> sort = null,
            bool buffered = false) where T : class;

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
        /// <returns>实体类</returns>
        public abstract IEnumerable<T> GetPageList<T>(int pageIndex, int pageSize, out long allRowsCount,
            object predicate = null, IList<ISort> sort = null, bool buffered = true) where T : class;

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
        /// <returns>实体类</returns>
        public abstract IEnumerable<T> GetPageList<T>(string innerSql,int pageIndex, int pageSize, out long allRowsCount,
            object predicate = null, IList<ISort> sort = null, bool buffered = true) where T : class;

        /// <summary>
        /// 多张表连接分页
        /// </summary>
        /// <typeparam name="TFirst">第一个类型（实体类名称）</typeparam>
        /// <typeparam name="TSecond">第二个类型（实体类名称）</typeparam>
        /// <typeparam name="TReturn">返回的类型（实体类名称）</typeparam>
        /// <typeparam name="TThird">第三个</typeparam>
        /// <typeparam name="TFourth">第四个</typeparam>
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
        public abstract IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TFourth, TReturn>(int pageIndex, int pageSize, out long allRowsCount,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, JoinType joinType = JoinType.Inner, string splitOn = "id",
            bool buffered = true) where TReturn : class;

        public abstract IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TFourth,TFive, TReturn>(int pageIndex, int pageSize, out long allRowsCount,
        Func<TFirst, TSecond, TThird, TFourth, TFive, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, JoinType joinType = JoinType.Inner, string splitOn = "id",
        bool buffered = true) where TReturn : class;
        public abstract IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TFourth, TFive,TSix, TReturn>(int pageIndex, int pageSize, out long allRowsCount,
       Func<TFirst, TSecond, TThird, TFourth, TFive,TSix, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, JoinType joinType = JoinType.Inner, string splitOn = "id",
       bool buffered = true) where TReturn : class;

        /// <summary>
        /// 多张表连接分页
        /// </summary>
        /// <typeparam name="TFirst">第一个类型（实体类名称）</typeparam>
        /// <typeparam name="TSecond">第二个类型（实体类名称）</typeparam>
        /// <typeparam name="TReturn">返回的类型（实体类名称）</typeparam>
        /// <typeparam name="TThird">第三个</typeparam>
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
        public abstract IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TReturn>(int pageIndex, int pageSize, out long allRowsCount,
            Func<TFirst, TSecond, TThird, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, JoinType joinType = JoinType.Inner, string splitOn = "id",
            bool buffered = true) where TReturn : class;

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
        public abstract IEnumerable<TReturn> GetPageList<TFirst, TSecond, TReturn>(int pageIndex, int pageSize, out long allRowsCount,
            Func<TFirst, TSecond, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, JoinType joinType = JoinType.Inner, string splitOn = "id",
            bool buffered = true) where TReturn : class;
        /// <summary>
        /// 插入单条记录
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public abstract dynamic Insert<T>(T entity) where T : class;

        /// <summary>
        /// 更新单条记录
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public abstract bool Update<T>(T entity) where T : class;

        /// <summary>
        ///删除单条记录
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="primaryId">主键ID</param>
        /// <returns></returns>
        public abstract int Delete<T>(dynamic primaryId) where T : class;

        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public abstract int DeleteList<T>(object predicate = null) where T : class;

        /// <summary>
        /// 批量插入功能
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entityList">实体类集合</param>
        public abstract bool InsertBatch<T>(IEnumerable<T> entityList) where T : class;
        /// <summary>
        ///批量更新（）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="entityList">实体类集合</param>
        /// <returns></returns>
        public abstract bool UpdateBatch<T>(IEnumerable<T> entityList) where T : class;
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public abstract bool DeleteBatch<T>(IEnumerable<dynamic> ids) where T : class;
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
        public abstract IEnumerable<T> Get<T>(string sql, dynamic param = null, bool buffered = true) where T : class;

        /// <summary>
        /// 根据条件筛选数据集合
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">SQL参数 匿名类型 如  new{name="张三"} name为sql参数</param>
        /// <param name="buffered">是否缓存</param>
        /// <returns></returns>
        public abstract IEnumerable<dynamic> Get(string sql, dynamic param = null, bool buffered = true);


        public abstract IEnumerable<TReturn> Get<TFirst, TSecond, TReturn>(Func<TFirst, TSecond, TReturn> map,
            JoinType joinType, IPredicate predicate = null, bool buffered = true,
            string splitOn = "Id", int? commandTimeout = null) where TReturn : class;



        public abstract IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TFourth,TReturn>(Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
        JoinType joinType, IPredicate predicate = null, bool buffered = true,
        string splitOn = "Id", int? commandTimeout = null) where TReturn : class;

        public abstract IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFive, TReturn>(
            Func<TFirst, TSecond, TThird, TFourth, TFive, TReturn> map, JoinType joinType, IPredicate predicate = null,
            bool buffered = true, string splitOn = "Id", int? commandTimeout = null) where TReturn : class;

        public abstract IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFive, TSix,TReturn>(
        Func<TFirst, TSecond, TThird, TFourth, TFive, TSix, TReturn> map, JoinType joinType, IPredicate predicate = null,
        bool buffered = true, string splitOn = "Id", int? commandTimeout = null) where TReturn : class;



        public abstract IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TReturn>(Func<TFirst, TSecond, TThird, TReturn> map,
    JoinType joinType, IPredicate predicate = null, bool buffered = true,
    string splitOn = "Id", int? commandTimeout = null) where TReturn : class;
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
        public abstract IEnumerable<TReturn> Get<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map,
            dynamic param = null, bool buffered = true, string splitOn = "Id",
            int? commandTimeout = null);
      

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
        public abstract IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TReturn>(string sql,
            Func<TFirst, TSecond, TThird, TReturn> map,
            dynamic param = null, bool buffered = true, string splitOn = "Id",
            int? commandTimeout = null);

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
        public abstract IEnumerable<TReturn> GetPageList<TFirst, TSecond, TReturn>(string sql, int pageIndex, int pageSize, out long allRowsCount,
            Func<TFirst, TSecond, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, string splitOn = "id",
            bool buffered = true) where TReturn : class;
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
        public abstract IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TReturn>(string sql, int pageIndex, int pageSize, out long allRowsCount,
            Func<TFirst, TSecond, TThird, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, string splitOn = "id",
            bool buffered = true) where TReturn : class;
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
        public abstract IEnumerable<TReturn> GetPageList<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, int pageIndex, int pageSize, out long  allRowsCount,
            Func<TFirst, TSecond, TThird, TFourth, TReturn> map, IPredicate predicate = null, IList<ISort> sort = null, string splitOn = "id",
            bool buffered = true) where TReturn : class;


        /// <summary>
        /// 获取多实体集合
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">SQL参数 匿名类型 如  new{name="张三"} name为sql参数</param>
        /// <param name="commandTimeout">command超时时间</param>
        /// <param name="commandType">command类型</param>
        /// <returns></returns>
        public abstract SqlMapper.GridReader GetMultiple(string sql, dynamic param = null,
            int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// 执行sql操作,不支持事务,事务请使用Dbhelp
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">SQL参数 匿名类型 如  new{name="张三"} name为sql参数</param>
        /// <returns></returns>
        public abstract int Execute(string sql, dynamic param = null);


        public abstract string GetPredictSql(IPredicate predicate, out SqlSimpleProcedureParameter parameters);

        #endregion

    }
}
