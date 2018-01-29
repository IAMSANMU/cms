using System;
using System.Collections.Generic;
using DapperExtensions;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;

namespace imow.IRepository
{
    public interface IBaseRepository<TEntityType> where TEntityType : class
    {
        bool Add(TEntityType entity);

        bool AddBatch(IEnumerable<TEntityType> entitys);

        bool Update(TEntityType entity);

        bool UpdateBatch(IEnumerable<TEntityType> entitys);

        bool Delete(TEntityType entity);

        bool DeleteList(IPredicate predGroup);

        TEntityType Get(string id);

        IEnumerable<TEntityType> GetList();

        bool IsRecordExist(IPredicate predGroup);


        int Count(IPredicate predGroup);

        IEnumerable<TEntityType> GetList(IPredicate predGroup);

        IEnumerable<TEntityType> GetList(IPredicate predGroup, IList<ISort> sort);

        IEnumerable<TEntityType> GetListByPage(int pageIndex, int pageSize, out long allRowsCount, IPredicate predGroup, IList<ISort> sort);
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
        IEnumerable<T> GetListByPage<T>( int pageSize, int pageIndex, string sort, string sortType, List<SearchModel> searchModels, out long count) where T : class;

    }
}