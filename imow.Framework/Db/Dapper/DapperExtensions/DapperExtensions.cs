using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Dapper;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;
namespace DapperExtensions
{
    public static class DapperExtensions
    {
        private readonly static object _lock = new object();

        private static Func<IDapperExtensionsConfiguration, IDapperImplementor> _instanceFactory;
        private static IDapperImplementor _instance;
        private static Dictionary<Imow.Framework.Db.DbType, IDapperImplementor> _instanceList = new Dictionary<Imow.Framework.Db.DbType, IDapperImplementor>();
        private static IDapperExtensionsConfiguration _configuration;


        /// <summary>
        /// Gets or sets the default class mapper to use when generating class maps. If not specified, AutoClassMapper<T> is used.
        /// DapperExtensions.Configure(Type, IList<Assembly>, ISqlDialect) can be used instead to set all values at once
        /// </summary>
        public static Type DefaultMapper
        {
            get
            {
                return _configuration.DefaultMapper;
            }
            set
            {
                Configure(value, _configuration.MappingAssemblies, _configuration.Dialect);
            }
        }

        /// <summary>
        /// Gets or sets the type of sql to be generated.
        /// DapperExtensions.Configure(Type, IList<Assembly>, ISqlDialect) can be used instead to set all values at once
        /// </summary>
        public static ISqlDialect SqlDialect
        {
            get
            {
                return _configuration.Dialect;
            }

            set
            {
                Configure(_configuration.DefaultMapper, _configuration.MappingAssemblies, value);
            }
        }

        /// <summary>
        /// Get or sets the Dapper Extensions Implementation Factory.
        /// </summary>
        public static Func<IDapperExtensionsConfiguration, IDapperImplementor> InstanceFactory
        {
            get
            {
                if (_instanceFactory == null)
                {
                    _instanceFactory = config => new DapperImplementor(new SqlGeneratorImpl(config));
                }
                return _instanceFactory;
            }
            //set
            //{
            //    _instanceFactory = value;
            //    Configure(_configuration.DefaultMapper, _configuration.MappingAssemblies, _configuration.Dialect);
            //}
        }


        /// <summary>
        /// Gets the Dapper Extensions Implementation
        /// </summary>
        private static IDapperImplementor Instance(Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER)
        {
            if (!_instanceList.ContainsKey(dbType))
            {
                lock (_instanceList)
                {
                    if (!_instanceList.ContainsKey(dbType))
                    {
                        switch (dbType)
                        {
                            case Imow.Framework.Db.DbType.SQLSERVER:
                                _instance = InstanceFactory(new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new SqlServerDialect()));
                                _instanceList.Add(dbType, _instance);
                                break;
                            case Imow.Framework.Db.DbType.ORACLE:
                                _instance = InstanceFactory(new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new OracleDialect()));
                                _instanceList.Add(dbType, _instance);
                                break;
                            case Imow.Framework.Db.DbType.MYSQL:
                                _instance = InstanceFactory(new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new MySqlDialect()));
                                _instanceList.Add(dbType, _instance);
                                break;
                            default:
                                _instance = InstanceFactory(new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new SqlServerDialect()));
                                _instanceList.Add(dbType, _instance);
                                break;
                        }
                    }
                }
            }
            return _instanceList[dbType];
        }

        static DapperExtensions()
        {
            Configure(typeof(AutoClassMapper<>), new List<Assembly>(), new SqlServerDialect());
        }

        /// <summary>
        /// Add other assemblies that Dapper Extensions will search if a mapping is not found in the same assembly of the POCO.
        /// </summary>
        /// <param name="assemblies"></param>
        public static void SetMappingAssemblies(IList<Assembly> assemblies)
        {
            Configure(_configuration.DefaultMapper, assemblies, _configuration.Dialect);
        }

        /// <summary>
        /// Configure DapperExtensions extension methods.
        /// </summary>
        /// <param name="defaultMapper"></param>
        /// <param name="mappingAssemblies"></param>
        /// <param name="sqlDialect"></param>
        public static void Configure(IDapperExtensionsConfiguration configuration)
        {
            _instance = null;
            _configuration = configuration;
        }

        /// <summary>
        /// Configure DapperExtensions extension methods.
        /// </summary>
        /// <param name="defaultMapper"></param>
        /// <param name="mappingAssemblies"></param>
        /// <param name="sqlDialect"></param>
        public static void Configure(Type defaultMapper, IList<Assembly> mappingAssemblies, ISqlDialect sqlDialect)
        {
            Configure(new DapperExtensionsConfiguration(defaultMapper, mappingAssemblies, sqlDialect));
        }

        /// <summary>
        /// Executes a query for the specified id, returning the data typed as per T
        /// </summary>
        public static T Get<T>(this IDbConnection connection, dynamic id, IDbTransaction transaction = null, int? commandTimeout = null, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where T : class
        {
            var result = Instance(dbType).Get<T>(connection, id, transaction, commandTimeout);
            return (T)result;
        }

        public static IEnumerable<TReturn> Get<TFirst, TSecond, TReturn>(this IDbConnection connection, Func<TFirst, TSecond, TReturn> map, JoinType joinType, string splitOn, IPredicate predicate, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where TReturn : class
        {
            return Instance(dbType).Get(connection, map, joinType, predicate,  transaction,commandTimeout, buffered, splitOn);
        }
        public static IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TReturn>(this IDbConnection connection, Func<TFirst, TSecond, TThird, TReturn> map, JoinType joinType, string splitOn, IPredicate predicate, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where TReturn : class
        {
            return Instance(dbType).Get(connection, map, joinType, predicate, transaction, commandTimeout, buffered, splitOn);
        }

        public static IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TFourth,TReturn>(this IDbConnection connection, Func<TFirst, TSecond, TThird, TFourth,TReturn> map, JoinType joinType, string splitOn, IPredicate predicate, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where TReturn : class
        {
            return Instance(dbType).Get(connection, map, joinType, predicate, transaction, commandTimeout, buffered, splitOn);
        }

        public static IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TFourth,TFive, TReturn>(this IDbConnection connection, Func<TFirst, TSecond, TThird, TFourth, TFive, TReturn> map, JoinType joinType, string splitOn, IPredicate predicate, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where TReturn : class
        {
            return Instance(dbType).Get(connection, map, joinType, predicate, transaction, commandTimeout, buffered, splitOn);
        }

        public static IEnumerable<TReturn> Get<TFirst, TSecond, TThird, TFourth, TFive, TSix,TReturn>(this IDbConnection connection, Func<TFirst, TSecond, TThird, TFourth, TFive, TSix, TReturn> map, JoinType joinType, string splitOn, IPredicate predicate, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where TReturn : class
        {
            return Instance(dbType).Get(connection, map, joinType, predicate, transaction, commandTimeout, buffered, splitOn);
        }

        /// <summary>
        /// Executes an insert query for the specified entity.
        /// </summary>
        public static void Insert<T>(this IDbConnection connection, IEnumerable<T> entities, IDbTransaction transaction = null, int? commandTimeout = null, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where T : class
        {
            Instance(dbType).Insert<T>(connection, entities, transaction, commandTimeout);
        }

        /// <summary>
        /// Executes an insert query for the specified entity, returning the primary key.  
        /// If the entity has a single key, just the value is returned.  
        /// If the entity has a composite key, an IDictionary&lt;string, object&gt; is returned with the key values.
        /// The key value for the entity will also be updated if the KeyType is a Guid or Identity.
        /// </summary>
        public static dynamic Insert<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = null, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where T : class
        {
            return Instance(dbType).Insert<T>(connection, entity, transaction, commandTimeout);
        }

        /// <summary>
        /// Executes an update query for the specified entity.
        /// </summary>
        public static bool Update<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = null, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where T : class
        {
            return Instance(dbType).Update<T>(connection, entity, transaction, commandTimeout);
        }

        /// <summary>
        /// Executes a delete query for the specified entity.
        /// </summary>
        public static int Delete<T>(this IDbConnection connection, T entity, IDbTransaction transaction = null, int? commandTimeout = null, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where T : class
        {
            return Instance(dbType).Delete<T>(connection, entity, transaction, commandTimeout);
        }

        /// <summary>
        /// Executes a delete query using the specified predicate.
        /// </summary>
        public static int Delete<T>(this IDbConnection connection, object predicate, IDbTransaction transaction = null, int? commandTimeout = null, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where T : class
        {
            return Instance(dbType).Delete<T>(connection, predicate, transaction, commandTimeout);
        }

        /// <summary>
        /// Executes a select query using the specified predicate, returning an IEnumerable data typed as per T.
        /// </summary>
        public static IEnumerable<T> GetList<T>(this IDbConnection connection, object predicate = null, IList<ISort> sort = null, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where T : class
        {
            return Instance(dbType).GetList<T>(connection, predicate, sort, transaction, commandTimeout, buffered);
        }


        public static IEnumerable<T> GetPage<T>(this IDbConnection connection, int page, int resultsPerPage, out long allRowsCount, string sql, dynamic param = null, string allRowsCountSql = null, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where T : class
        {
            return Instance(dbType).GetPage<T>(connection, page, resultsPerPage, out allRowsCount, sql, param, allRowsCountSql, transaction, commandTimeout, buffered);
        }


        /// <summary>
        /// Executes a select query using the specified predicate, returning an IEnumerable data typed as per T.
        /// Data returned is dependent upon the specified page and resultsPerPage.
        /// </summary>
        public static IEnumerable<T> GetPage<T>(this IDbConnection connection, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where T : class
        {
            return Instance(dbType).GetPage<T>(connection, predicate, sort, page, resultsPerPage, transaction, commandTimeout, buffered);
        }

       

        /// <summary>
        /// Executes a select query using the specified predicate, returning an IEnumerable data typed as per T.
        /// Data returned is dependent upon the specified page and resultsPerPage.
        /// </summary>
        public static IEnumerable<T> GetPage<T>(this IDbConnection connection,string innerSql, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where T : class
        {
            return Instance(dbType).GetPage<T>(innerSql,connection, predicate, sort, page, resultsPerPage, transaction, commandTimeout, buffered);
        }


        public static IEnumerable<TReturn> GetPage<TFirst, TSecond, TReturn>(this IDbConnection connection, Func<TFirst, TSecond, TReturn> map, JoinType joinType, string splitOn, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where TReturn : class
        {
            return Instance(dbType)
                .GetPage(connection, map, joinType, predicate, sort, page, resultsPerPage, transaction,
                    commandTimeout, buffered, splitOn);
        }
        public static IEnumerable<TReturn> GetPage<TFirst, TSecond, TThird, TFourth, TReturn>(this IDbConnection connection, Func<TFirst, TSecond, TThird, TFourth,TReturn> map, JoinType joinType, string splitOn, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where TReturn : class
        {
            return Instance(dbType)
                .GetPage(connection, map, joinType, predicate, sort, page, resultsPerPage, transaction,
                    commandTimeout, buffered, splitOn);
        }

        public static IEnumerable<TReturn> GetPage<TFirst, TSecond, TThird, TFourth,TFive, TReturn>(this IDbConnection connection, Func<TFirst, TSecond, TThird, TFourth, TFive, TReturn> map, JoinType joinType, string splitOn, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where TReturn : class
        {
            return Instance(dbType)
                .GetPage(connection, map, joinType, predicate, sort, page, resultsPerPage, transaction,
                    commandTimeout, buffered, splitOn);
        }
        public static IEnumerable<TReturn> GetPage<TFirst, TSecond, TThird, TFourth, TFive,TSix, TReturn>(this IDbConnection connection, Func<TFirst, TSecond, TThird, TFourth, TFive,TSix, TReturn> map, JoinType joinType, string splitOn, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where TReturn : class
        {
            return Instance(dbType)
                .GetPage(connection, map, joinType, predicate, sort, page, resultsPerPage, transaction,
                    commandTimeout, buffered, splitOn);
        }
        public static IEnumerable<TReturn> GetPage<TFirst, TSecond, TThird,  TReturn>(this IDbConnection connection, Func<TFirst, TSecond, TThird, TReturn> map, JoinType joinType, string splitOn, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where TReturn : class
        {
            return Instance(dbType)
                .GetPage(connection, map, joinType, predicate, sort, page, resultsPerPage, transaction,
                    commandTimeout, buffered, splitOn);
        }
        public static IEnumerable<TReturn> GetPage<TFirst, TSecond, TReturn>(this IDbConnection connection, string innerSql, Func<TFirst, TSecond, TReturn> map, string splitOn, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, out long allRowsCount,IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where TReturn : class
        {
            return Instance(dbType)
                .GetPage(connection, innerSql, map, predicate, sort, page, resultsPerPage, out allRowsCount, transaction,
                    commandTimeout, buffered, splitOn);
        }

     
        public static IEnumerable<TReturn> GetPage<TFirst, TSecond, TThird,TReturn>(this IDbConnection connection,string innerSql, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, out long allRowsCount, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where TReturn : class
        {
            return Instance(dbType)
                .GetPage(connection, innerSql, map, predicate, sort, page, resultsPerPage, out allRowsCount, transaction,
                    commandTimeout, buffered, splitOn);
        }
        public static IEnumerable<TReturn> GetPage<TFirst, TSecond, TThird,TFourth, TReturn>(this IDbConnection connection, string innerSql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, string splitOn, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, out long allRowsCount, IDbTransaction transaction = null, int? commandTimeout = null, bool buffered = false, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where TReturn : class
        {
            return Instance(dbType)
                .GetPage(connection, innerSql, map, predicate, sort, page, resultsPerPage, out allRowsCount, transaction,
                    commandTimeout, buffered, splitOn);
        }


        /// <summary>
        /// Executes a query using the specified predicate, returning an integer that represents the number of rows that match the query.
        /// </summary>
        public static int Count<T>(this IDbConnection connection,string innerSql, object predicate, IDbTransaction transaction = null, int? commandTimeout = null, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where T : class
        {
            return Instance(dbType).Count<T>(innerSql,connection, predicate, transaction, commandTimeout);
        }
        /// <summary>
        /// Executes a query using the specified predicate, returning an integer that represents the number of rows that match the query.
        /// </summary>
        public static int Count<T>(this IDbConnection connection, object predicate, IDbTransaction transaction = null, int? commandTimeout = null, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where T : class
        {
            return Instance(dbType).Count<T>(connection, predicate, transaction, commandTimeout);
        }

        /// <summary>
        /// Executes a query using the specified predicate, returning an integer that represents the number of rows that match the query.
        /// </summary>
        public static int Count<TFirst, TSecond>(this IDbConnection connection, JoinType joinType, IPredicate predicate, IDbTransaction transaction = null, int? commandTimeout = null, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER)
        {
            return Instance(dbType).Count<TFirst, TSecond>(connection, joinType, predicate, transaction, commandTimeout);
        }

        /// <summary>
        /// Executes a query using the specified predicate, returning an integer that represents the number of rows that match the query.
        /// </summary>
        public static int Count<TFirst, TSecond, TThird>(this IDbConnection connection, JoinType joinType, IPredicate predicate, IDbTransaction transaction = null, int? commandTimeout = null, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER)
        {
            return Instance(dbType).Count<TFirst, TSecond, TThird>(connection, joinType, predicate, transaction, commandTimeout);
        }


        /// <summary>
        /// Executes a query using the specified predicate, returning an integer that represents the number of rows that match the query.
        /// </summary>
        public static int Count<TFirst, TSecond, TThird, TFourth>(this IDbConnection connection, JoinType joinType, IPredicate predicate, IDbTransaction transaction = null, int? commandTimeout = null, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER)
        {
            return Instance(dbType).Count<TFirst, TSecond, TThird, TFourth>(connection, joinType, predicate, transaction, commandTimeout);
        }


        /// <summary>
        /// Executes a query using the specified predicate, returning an integer that represents the number of rows that match the query.
        /// </summary>
        public static int Count<TFirst, TSecond, TThird, TFourth, TFive>(this IDbConnection connection, JoinType joinType, IPredicate predicate, IDbTransaction transaction = null, int? commandTimeout = null, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER)
        {
            return Instance(dbType).Count<TFirst, TSecond, TThird, TFourth, TFive>(connection, joinType, predicate, transaction, commandTimeout);
        }

        /// <summary>
        /// Executes a select query for multiple objects, returning IMultipleResultReader for each predicate.
        /// </summary>
        public static IMultipleResultReader GetMultiple(this IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction = null, int? commandTimeout = null, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER)
        {
            return Instance(dbType).GetMultiple(connection, predicate, transaction, commandTimeout);
        }

        /// <summary>
        /// Gets the appropriate mapper for the specified type T. 
        /// If the mapper for the type is not yet created, a new mapper is generated from the mapper type specifed by DefaultMapper.
        /// </summary>
        public static IClassMapper GetMap<T>(Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER) where T : class
        {
            return Instance(dbType).SqlGenerator.Configuration.GetMap<T>();
        }

        /// <summary>
        /// Clears the ClassMappers for each type.
        /// </summary>
        public static void ClearCache(Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER)
        {
            Instance(dbType).SqlGenerator.Configuration.ClearCache();
        }

        /// <summary>
        /// Generates a COMB Guid which solves the fragmented index issue.
        /// See: http://davybrion.com/blog/2009/05/using-the-guidcomb-identifier-strategy
        /// </summary>
        public static Guid GetNextGuid(Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER)
        {
            return Instance(dbType).SqlGenerator.Configuration.GetNextGuid();
        }


        public static string GetPredictSql(IPredicate predicate, IDictionary<string, object> parameters, Imow.Framework.Db.DbType dbType = Imow.Framework.Db.DbType.SQLSERVER)
        {
            return predicate.GetSql(Instance(dbType).SqlGenerator, parameters, SqlDialect);
        }
    }
}
