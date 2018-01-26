using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using DapperExtensions;
using imow.IRepository;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;
using Imow.Framework.Db;
using Imow.Framework.Engine;

namespace imow.Repository
{
    public class BaseRepository<TEntityType> : IBaseRepository<TEntityType> where TEntityType : class
    {

        protected readonly string DbconnectString;

        protected DapperHelper DapperHelper
        {
            get { return DapperHelper.CreateHelper(DbconnectString); }
        }


        protected DbHelper DbHelper
        {
            get { return DbHelper.CreateHelper(DbconnectString); }
        }

        public BaseRepository()
        {
            DbconnectString = ImowEngineContext.Current.ResolveConfig<imow.Core.config.DBConfig>().ConnectionString;
        }


        /// <summary>
        ///     添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Add(TEntityType entity)
        {
            DapperHelper.Insert(entity);
            return true;
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public virtual bool AddBatch(IEnumerable<TEntityType> entitys)
        {
            return DapperHelper.InsertBatch(entitys);
        }


        /// <summary>
        /// 获得数量
        /// </summary>
        /// <param name="predGroup"></param>
        /// <returns></returns>
        public virtual int Count(IPredicate predGroup)
        {
            return DapperHelper.Count<TEntityType>(predGroup);
        }

        /// <summary>
        ///     更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Update(TEntityType entity)
        {
            return DapperHelper.Update(entity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public virtual bool UpdateBatch(IEnumerable<TEntityType> entitys)
        {
            return DapperHelper.UpdateBatch(entitys);
        }

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Delete(TEntityType entity)
        {
            return DapperHelper.Delete<TEntityType>(entity) > 0;
        }



        public virtual bool DeleteList(IPredicate predGroup)
        {
            return DapperHelper.DeleteList<TEntityType>(predGroup) > 0;
        }


        /// <summary>
        ///     根据主键查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntityType Get(string id)
        {
            return DapperHelper.GetById<TEntityType>(id);
        }

        /// <summary>
        ///     查找所有
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntityType> GetList()
        {
            return DapperHelper.GetAll<TEntityType>();
        }

        /// <summary>
        ///     根据条件查找
        /// </summary>
        /// <param name="predGroup"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntityType> GetList(IPredicate predGroup)
        {
            return DapperHelper.GetList<TEntityType>(predGroup);
        }

        /// <summary>
        ///     根据条件查找
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntityType> GetList(IPredicate predGroup, IList<ISort> sort)
        {
            return DapperHelper.GetList<TEntityType>(predGroup, sort);
        }

        /// <summary>
        ///     记录是否存在
        /// </summary>
        /// <param name="predGroup"></param>
        /// <returns></returns>
        public virtual bool IsRecordExist(IPredicate predGroup)
        {
            return DapperHelper.Count<TEntityType>(predGroup) > 0;
        }

        /// <summary>
        ///     分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="allRowsCount"></param>
        /// <param name="predGroup">查询条件</param>
        /// <param name="sort">排序字段</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntityType> GetListByPage(int pageIndex, int pageSize, out long allRowsCount,
            IPredicate predGroup, IList<ISort> sort)
        {
            return DapperHelper
                .GetPageList<TEntityType>(pageIndex, pageSize, out allRowsCount, predGroup, sort);
        }
        /// <summary>
        /// 单表搜索 分页查询
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="sort"></param>
        /// <param name="sortType"></param>
        /// <param name="searchModels"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetListByPage<T>(int pageSize, int pageIndex, string sort, string sortType, List<SearchModel> searchModels, out long count) where T : class
        {
            PredicateGroup group = BuildPredGroup<T>(searchModels);

            IList<ISort> sortList = BuildSort<T>(sort, sortType);

            return DapperHelper.GetPageList<T>(pageIndex, pageSize, out count, group, sortList);
        }

        protected IPredicate BuildFieldPredicate<T>(SearchModel item) where T : class
        {
            return new FieldPredicate<T>
            {
                PropertyName = item.Column,
                Operator = item.Op,
                Value = item.DisposeValue,
                Not = item.Not
            };
        }

        protected PredicateGroup BuildPredGroup<T>(List<SearchModel> searchModels) where T : class
        {
            PredicateGroup group = null;
            if (searchModels != null && searchModels.Any())
            {
                group = new PredicateGroup
                {
                    Operator = GroupOperator.And,
                    Predicates = new List<IPredicate>()
                };
                foreach (var item in searchModels)
                {
                    if (item.NotAuto)
                    {
                        continue;
                    }
                    IPredicate predicate = new FieldPredicate<T>
                    {
                        PropertyName = item.Column,
                        Operator = item.Op,
                        Value = item.DisposeValue,
                        Not = item.Not
                    };
                    group.Predicates.Add(predicate);
                }
            }
            return group;
        }

        protected IList<ISort> BuildSort<T>(string sort, string sortType)
        {
            IList<ISort> sortList = null;
            if (!string.IsNullOrEmpty(sort) && !string.IsNullOrEmpty(sortType))
            {
                var sorts = sort.Split('.');
                var sortLen = sorts.Length;
                string propertyName = sortLen > 1 ? sorts[1] : sort;
                sortList = new List<ISort>();
                var sortItem = new Sort(typeof(T))
                {
                    PropertyName = propertyName,
                    Ascending = sortType == "asc"
                };
                sortList.Add(sortItem);
            }
            return sortList;
        }
    }
}