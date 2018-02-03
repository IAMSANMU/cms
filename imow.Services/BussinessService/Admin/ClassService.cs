using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using imow.Core;
using imow.IRepository;
using imow.IRepository.Admin;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;
using Imow.Framework.Cache.DistributedCache.Storage;

namespace imow.Services.BussinessService.Admin
{
    public class ClassService : IBaseService
    {
        private readonly IClassRepository _dao;
        private readonly CacheFactory _cache;
        public ClassService(IClassRepository dao,CacheFactory cache)
        {
            _cache = cache;
            _dao = dao;
        }

        public void RemoveCache()
        {
            _cache.Delete(new [] {CacheKey.IndexClassCacheKey});
        }

        public void Add(ClassEntity entity)
        {
            _dao.Add(entity);
            RemoveCache();
        }

        public void Update(ClassEntity entity)
        {
            _dao.Update(entity);
            RemoveCache();
        }

        public IEnumerable<ClassEntity> GetList(int[] ids)
        {
            List<IPredicate> list = new List<IPredicate>();
            foreach (var id in ids)
            {
                list.Add(Predicates.Field<ClassEntity>(f => f.Id, Operator.Eq, id));
            }
            IPredicateGroup group = Predicates.Group(GroupOperator.Or, list.ToArray());
            IEnumerable<ClassEntity> dbList = _dao.GetList(group);
            return dbList;
        }
        public void Delete(int[] ids)
        {
            List<ClassEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = true; });
            _dao.UpdateBatch(dbList);
            RemoveCache();
        }
        public void Restore(int[] ids)
        {
            List<ClassEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = false; });
            _dao.UpdateBatch(dbList);
            RemoveCache();
        }

        public ClassEntity Get(int id)
        {
           return _dao.Get(id.ToString());
        }
        public IEnumerable<ClassEntity> GetListByPage(int pageSize, int pageIndex, string sort, string sortType,
            List<SearchModel> searchModels, out long count)
        {
            return _dao.GetListByPage<ClassEntity>(pageSize, pageIndex, sort, sortType, searchModels,
                out count);
        }

        #region 前台方法

        public IEnumerable<ClassEntity> GetAll()
        {
            List<ISort> sortList = new List<ISort> { Predicates.Sort<ClassEntity>(f => f.OrderNum) };
            var group = Predicates.Field<ClassEntity>(f => f.IsDel, Operator.Eq, false);
            return _cache.GetOrSetValue(CacheKey.IndexClassCacheKey,
                () => _dao.GetList(group, sortList));
        }

        #endregion

    }
}
