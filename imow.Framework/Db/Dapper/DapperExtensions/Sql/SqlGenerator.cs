using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperExtensions.Mapper;
using ServiceStack;

namespace DapperExtensions.Sql
{
    public interface ISqlGenerator
    {
        IDapperExtensionsConfiguration Configuration { get; }

        string Select(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, IDictionary<string, object> parameters);
        string SelectPaged(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters);

        string GetSelectPaged(IClassMapper[] classMap, JoinType joinType, IPredicate predicate,
            IList<ISort> sort, IDictionary<string, object> parameters, ref string splitOn);

        string GetCountSelectPaged(IClassMapper[] classMap, JoinType joinType, IPredicate predicate,
          IList<ISort> sort, IDictionary<string, object> parameters);

        string GetSelectPaged(IClassMapper[] classMap, JoinType joinType, IPredicate predicate,
         IList<ISort> sort, IDictionary<string, object> parameters);

        string SelectPaged(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDictionary<string, object> parameters);

        string SelectPaged(IClassMapper[] classMap, JoinType joinType, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDictionary<string, object> parameters,ref string splitOn);
        //CX.ADD
        string SelectPaged(string innerSql, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDictionary<string, object> parameters);

        string SelectSet(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int firstResult, int maxResults, IDictionary<string, object> parameters);

        string Count(string innerSql, IPredicate predicate, IDictionary<string, object> parameters);
        string Count(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters);

        string Count(IClassMapper[] classMap, JoinType joinType, IPredicate predicate, IDictionary<string, object> parameters);
        string PageCount(string sql);
        string Insert(IClassMapper classMap);
        string Update(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters);
        string Delete(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters);

        string IdentitySql(IClassMapper classMap);
        string GetTableName(IClassMapper map);
        string GetColumnName(IClassMapper map, IPropertyMap property, bool includeAlias);
        string GetColumnName(IClassMapper map, string propertyName, bool includeAlias);
        bool SupportsMultipleStatements();
    }

    public class SqlGeneratorImpl : ISqlGenerator
    {
        public SqlGeneratorImpl(IDapperExtensionsConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IDapperExtensionsConfiguration Configuration { get; private set; }



        public virtual string Select(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, IDictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder sql = new StringBuilder(string.Format("SELECT {0} FROM {1}",
                BuildSelectColumns(classMap),
                GetTableName(classMap)));
            if (predicate != null)
            {
                sql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters, Configuration.Dialect));
            }

            if (sort != null && sort.Any())
            {
                sql.Append(" ORDER BY ")
                    .Append(sort.Select(s => GetColumnName(classMap, s.PropertyName, false) + (s.Ascending ? " ASC" : " DESC")).AppendStrings());
            }

            return sql.ToString();
        }



        public string SelectPaged(string sql, int page, int resultsPerPage, IDictionary<string, object> parameters)
        {
            string pageSql = Configuration.Dialect.GetPagingSql(sql, page, resultsPerPage, parameters);
            return pageSql;
        }




        public virtual string SelectPaged(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int page, int resultsPerPage, IDictionary<string, object> parameters)
        {
            //if (sort == null || !sort.Any())
            //{
            //    throw new ArgumentNullException("Sort", "Sort cannot be null or empty.");
            //}

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder innerSql = new StringBuilder(string.Format("SELECT {0} FROM {1}",
                BuildSelectColumns(classMap),
                GetTableName(classMap)));
            if (predicate != null)
            {
                innerSql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters, Configuration.Dialect));
            }
            if (sort != null && sort.Any())
            {
                string orderBy = sort.Select(s => GetColumnName(classMap, s.PropertyName, false) + (s.Ascending ? " ASC" : " DESC")).AppendStrings();
                innerSql.Append(" ORDER BY " + orderBy);
            }

            string sql = Configuration.Dialect.GetPagingSql(innerSql.ToString(), page, resultsPerPage, parameters);
            return sql;
        }

        public string GetSelectPaged(IClassMapper[] classMap, JoinType joinType, IPredicate predicate,
            IList<ISort> sort, IDictionary<string, object> parameters, ref string splitOn)
        {
            if (splitOn == null) splitOn = "Id";
            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }
            ArrayOfString columsNames = null;
            var tableStr = GetTableNameAndOnwhere(classMap, joinType);
            List<string> splitList = new List<string>();
            if (splitOn != null)
            {
                splitList = splitOn.Split(',').ToList();
            }
            columsNames = new ArrayOfString();
            Dictionary<string, int> columnDic = new Dictionary<string, int>();
            for (int i = 0; i < classMap.Length; i++)
            {
                var classMapper = classMap[i];
                #region filled first-model's column name.
                if (splitList.Count < classMap.Length && i == 0)
                {
                    List<string> itemSpList = new List<string> { classMapper.Properties[0].ColumnName };
                    itemSpList.AddRange(splitList);
                    splitList = itemSpList;
                }
                #endregion

                foreach (var propertyMap in classMapper.Properties)
                {
                    if (propertyMap.Ignored)
                    {
                        continue;
                    }
                    string columnName = propertyMap.ColumnName;
                    string prefix = null;
                    if (!columnDic.ContainsKey(columnName))
                    {
                        columnDic.Add(columnName, 0);
                    }
                    int count = ++columnDic[columnName];
                    if (count > 1)
                    {
                        prefix = new string('_', count - 1);
                    }
                    bool rename = prefix != null;
                    string aliName = rename ? columnName + prefix : null;
                    // rename
                    if (rename && splitList.Count > i && string.Equals(splitList[i], columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        splitList[i] = aliName;
                    }
                    columsNames.Add(Configuration.Dialect.GetColumnName(GetTableName(classMapper), columnName, rename ? aliName : null));
                }
            }
            splitOn = string.Join(",", splitList);

            StringBuilder innerSql = new StringBuilder(string.Format("SELECT {1} FROM {0}", tableStr, string.Join(",", columsNames)));
            if (predicate != null)
            {
                innerSql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters, Configuration.Dialect));
            }
            if (sort != null && sort.Any())
            {
                string orderBy = sort.Select(s => GetColumnName(classMap.First(_classMap => _classMap.Properties.Any(x => x.PropertyInfo == s.PropertyInfo)), s.PropertyName, false) + (s.Ascending ? " ASC" : " DESC")).AppendStrings();
                innerSql.Append(" ORDER BY " + orderBy);
            }


            return innerSql.ToString();
        }

        public string GetCountSelectPaged(IClassMapper[] classMap, JoinType joinType, IPredicate predicate, IList<ISort> sort,
            IDictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }
            var tableStr = GetTableNameAndOnwhere(classMap, joinType);
            StringBuilder innerSql = new StringBuilder(string.Format("SELECT {1} FROM {0}", tableStr, "'aaa' aaa"));
            if (predicate != null)
            {
                innerSql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters, Configuration.Dialect));
            }
            if (sort != null && sort.Any())
            {
                string orderBy = sort.Select(s => GetColumnName(classMap.First(_classMap => _classMap.Properties.Any(x => x.PropertyInfo == s.PropertyInfo)), s.PropertyName, false) + (s.Ascending ? " ASC" : " DESC")).AppendStrings();
                innerSql.Append(" ORDER BY " + orderBy);
            }


            return innerSql.ToString();
        }

        public string GetSelectPaged(IClassMapper[] classMap, JoinType joinType, IPredicate predicate,
       IList<ISort> sort, IDictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }
            var tableStr = GetTableNameAndOnwhere(classMap, joinType);
            StringBuilder innerSql = new StringBuilder(string.Format("SELECT {1} FROM {0}", tableStr, "*"));
            if (predicate != null)
            {
                innerSql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters, Configuration.Dialect));
            }
            if (sort != null && sort.Any())
            {
                string orderBy = sort.Select(s => GetColumnName(classMap.First(_classMap => _classMap.Properties.Any(x => x.PropertyInfo == s.PropertyInfo)), s.PropertyName, false) + (s.Ascending ? " ASC" : " DESC")).AppendStrings();
                innerSql.Append(" ORDER BY " + orderBy);
            }

            return innerSql.ToString();
        }
        //CX.ADD
        private string GetSelectPagedWithPaging(string innerTable, IPredicate predicate,
    IList<ISort> sort, IDictionary<string, object> parameters)
        {

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder innerSql = new StringBuilder(innerTable);
            if (predicate != null)
            {
                innerSql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters, Configuration.Dialect));
            }
            if (sort != null && sort.Any())
            {
                string orderBy = sort.Select(s => GetColumnName(s.TableName, s.PropertyName) + (s.Ascending ? " ASC" : " DESC")).AppendStrings();
                innerSql.Append(" ORDER BY " + orderBy);
            }


            return innerSql.ToString();
        }

        public virtual string SelectPaged(IClassMapper[] classMap, JoinType joinType, IPredicate predicate, IList<ISort> sort, int page,
            int resultsPerPage, IDictionary<string, object> parameters,ref  string splitOn)
        {
            var innerSql = GetSelectPaged(classMap, joinType, predicate, sort, parameters, ref splitOn);
            string sql = Configuration.Dialect.GetPagingSql(innerSql, page, resultsPerPage, parameters);
            return sql;
        }


        public virtual string SelectPaged(string innerTable, IPredicate predicate, IList<ISort> sort, int page,
    int resultsPerPage, IDictionary<string, object> parameters)
        {
            var innerSql = GetSelectPagedWithPaging(innerTable, predicate, sort, parameters);
            string sql = Configuration.Dialect.GetPagingSql(innerSql, page, resultsPerPage, parameters);
            return sql;
        }


        public virtual string SelectSet(IClassMapper classMap, IPredicate predicate, IList<ISort> sort, int firstResult, int maxResults, IDictionary<string, object> parameters)
        {
            if (sort == null || !sort.Any())
            {
                throw new ArgumentNullException("Sort", "Sort cannot be null or empty.");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder innerSql = new StringBuilder(string.Format("SELECT {0} FROM {1}",
                BuildSelectColumns(classMap),
                GetTableName(classMap)));
            if (predicate != null)
            {
                innerSql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters, Configuration.Dialect));
            }

            string orderBy = sort.Select(s => GetColumnName(classMap, s.PropertyName, false) + (s.Ascending ? " ASC" : " DESC")).AppendStrings();
            innerSql.Append(" ORDER BY " + orderBy);

            string sql = Configuration.Dialect.GetSetSql(innerSql.ToString(), firstResult, maxResults, parameters);
            return sql;
        }

     
        public virtual string Count(string innerSql,IPredicate predicate, IDictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder sql = new StringBuilder(string.Format("SELECT COUNT(*) {0}Total{1} FROM ({2}) item",
                                Configuration.Dialect.OpenQuote,
                                Configuration.Dialect.CloseQuote,
                                innerSql));
            if (predicate != null)
            {
                sql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters, Configuration.Dialect));
            }

            return sql.ToString();
        }

        public virtual string Count(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder sql = new StringBuilder(string.Format("SELECT COUNT(*) {0}Total{1} FROM {2}",
                                Configuration.Dialect.OpenQuote,
                                Configuration.Dialect.CloseQuote,
                                GetTableName(classMap)));
            if (predicate != null)
            {
                sql.Append(" WHERE ")
                    .Append(predicate.GetSql(this, parameters, Configuration.Dialect));
            }

            return sql.ToString();
        }

        public virtual string Count(IClassMapper[] classMap, JoinType joinType, IPredicate predicate, IDictionary<string, object> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }
            var innerSql = GetCountSelectPaged(classMap, joinType, predicate, null, parameters);
            StringBuilder sql = new StringBuilder(string.Format("SELECT COUNT(*) {0}Total{1} FROM {2} ali",
                                Configuration.Dialect.OpenQuote,
                                Configuration.Dialect.CloseQuote,
                                string.Format("({0})", innerSql)));
            return sql.ToString();
        }

        public virtual string PageCount(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder sqlCount = new StringBuilder(Configuration.Dialect.BatchSeperator);
            sqlCount.Append(string.Format("SELECT COUNT(*) AS {0}Total{1} FROM ({2}) {0}TempCountData{1}",
                                     Configuration.Dialect.OpenQuote,
                                     Configuration.Dialect.CloseQuote,
                                     sql));


            return sqlCount.ToString();
        }

        public string GetTableNameAndOnwhere(IClassMapper[] classMap, JoinType joinType)
        {
            Dictionary<string,string> tableAndJoinParaDictionary=new Dictionary<string, string>();
            List<string> resultList = new List<string>();
            List<string> tableNames = new List<string>();
            foreach (IClassMapper classMapper in classMap)
            {
                string tbName = GetTableName(classMapper);
                //获得表名称
                if (!tableNames.Contains(tbName))
                {
                    tableNames.Add(tbName);
                }
                //获得外键
                List<IPropertyMap> forignKeyPropertyMaps = classMapper.Properties.ToList().FindAll(x => x.ForignKeyPropertyInfo != null);
                if (forignKeyPropertyMaps.Any())
                {
                    foreach (IPropertyMap forignKeyPropertyMap in forignKeyPropertyMaps)
                    {
                        foreach (var forignKeyProperty in forignKeyPropertyMap.ForignKeyPropertyInfo)
                        {
                            var forignKeyclassMap = classMap.SingleOrDefault(x => x.EntityType == forignKeyProperty.DeclaringType);
                            if (forignKeyclassMap != null)
                            {
                                string fKeyTable = GetTableName(forignKeyclassMap);
                                if(tableNames.Contains(fKeyTable))continue;
                                tableNames.Add(GetTableName(forignKeyclassMap));
                                var forignPropertyInfo = forignKeyclassMap.Properties.SingleOrDefault(x => x.PropertyInfo== forignKeyProperty);
                                var pkcolumn = GetColumnName(classMapper, forignKeyPropertyMap, true);
                                var fkcolumn = GetColumnName(forignKeyclassMap, forignPropertyInfo, true);
                                resultList.Add(string.Format(" {0}={1} ", pkcolumn, fkcolumn));
                            }
                        }
                        //tableNames.Add(GetTableName(forignc));
                    }
                }
            }
            if (resultList.Count <= 0)
            {
                throw new Exception("没有设置表关系");
            }
            if (classMap.Length > 2)
            {
                return Configuration.Dialect.GetMultipleJoinSql(tableNames, resultList, GetJoinStr(joinType));
            }
            else
            {
                var tableStr = string.Join(" " + GetJoinStr(joinType) + " ", tableNames);
                return tableStr + " on " + string.Join(" and ", resultList);
            }
        }


        private string GetJoinStr(JoinType joinType)
        {
            string result = "inner join";
            switch (joinType)
            {
                case JoinType.Left:
                    result = "left join";
                    break;
                case JoinType.Right:
                    result = "right join";
                    break;
                case JoinType.Outer:
                    result = "outer join";
                    break;
            }
            return result;
        }


        public virtual string Insert(IClassMapper classMap)
        {
            var columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity));
            if (!columns.Any())
            {
                throw new ArgumentException("No columns were mapped.");
            }

            var columnNames = columns.Select(p => GetColumnName(classMap, p, false));
            var parameters = columns.Select(p => Configuration.Dialect.ParameterPrefix + p.Name);

            string sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                                       GetTableName(classMap),
                                       columnNames.AppendStrings(),
                                       parameters.AppendStrings());

            return sql;
        }

        public virtual string Update(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("Predicate");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            var columns = classMap.Properties.Where(p => !(p.Ignored || p.IsReadOnly || p.KeyType == KeyType.Identity));
            if (!columns.Any())
            {
                throw new ArgumentException("No columns were mapped.");
            }

            var setSql =
                columns.Select(
                    p =>
                    string.Format(
                        "{0} = {1}{2}", GetColumnName(classMap, p, false), Configuration.Dialect.ParameterPrefix, p.Name));


            return string.Format("UPDATE {0} SET {1} WHERE {2}",
                GetTableName(classMap),
                setSql.AppendStrings(),
                predicate.GetSql(this, parameters, Configuration.Dialect));
        }


        public virtual string Delete(IClassMapper classMap, IPredicate predicate, IDictionary<string, object> parameters)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("Predicate");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("Parameters");
            }

            StringBuilder sql = new StringBuilder(string.Format("DELETE FROM {0}", GetTableName(classMap)));
            sql.Append(" WHERE ").Append(predicate.GetSql(this, parameters, Configuration.Dialect));
            return sql.ToString();
        }

        public virtual string IdentitySql(IClassMapper classMap)
        {
            return Configuration.Dialect.GetIdentitySql(GetTableName(classMap));
        }

        public virtual string GetTableName(IClassMapper map)
        {
            return Configuration.Dialect.GetTableName(map.SchemaName, map.TableName, null);
        }



        public virtual string GetColumnName(IClassMapper map, IPropertyMap property, bool includeAlias)
        {
            string alias = null;
            if (property.ColumnName != property.Name && includeAlias)
            {
                alias = property.Name;
            }

            return Configuration.Dialect.GetColumnName(GetTableName(map), property.ColumnName, alias);
        }

        public virtual string GetColumnName(IClassMapper map, string propertyName, bool includeAlias)
        {
            IPropertyMap propertyMap = map.Properties.SingleOrDefault(p => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
            if (propertyMap == null)
            {
                throw new ArgumentException(string.Format("Could not find '{0}' in Mapping.", propertyName));
            }

            return GetColumnName(map, propertyMap, includeAlias);
        }
        public virtual string GetColumnName(string prefix, string propertyName)
        {
            return Configuration.Dialect.GetColumnName(prefix, propertyName, null);
        }


        public virtual bool SupportsMultipleStatements()
        {
            return Configuration.Dialect.SupportsMultipleStatements;
        }

        public virtual string BuildSelectColumns(IClassMapper classMap)
        {
            var columns = classMap.Properties
                .Where(p => !p.Ignored)
                .Select(p => GetColumnName(classMap, p, true));
            return columns.AppendStrings();
        }

        public virtual IList<IPropertyMap> GetProperties(IClassMapper classMap)
        {
            return classMap.Properties
                .Where(p => !p.Ignored).ToList();
        }


    }
}