using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using imow.Core;
using imow.IRepository;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;
using Imow.Framework.Cache.DistributedCache.Storage;

namespace imow.Services.BussinessService.Admin
{
    public class SchoolService : IBaseService
    {
        private readonly IBaseRepository<SchoolEntity> _schoolDao;
        private readonly CacheFactory _cache;
        public SchoolService(IBaseRepository<SchoolEntity> schoolDao,CacheFactory cache)
        {
            _cache = cache;
            _schoolDao = schoolDao;
        }

        public void RemoveCache()
        {
            _cache.Delete(new [] {CacheKey.SchoolCacheKey});
        }
        public void Add(SchoolEntity entity)
        {
            _schoolDao.Add(entity);
            RemoveCache();
        }

        public void Update(SchoolEntity entity)
        {
            _schoolDao.Update(entity);
            RemoveCache();
        }

        public IEnumerable<SchoolEntity> GetAll( )
        {
            var group = Predicates.Field<SchoolEntity>(f => f.IsDel, Operator.Eq, false);
            return _cache.GetOrSetValue(CacheKey.SchoolCacheKey,()=>_schoolDao.GetList(group));
        }

        public IEnumerable<SchoolEntity> GetList(int[] ids)
        {
            List<IPredicate> list = new List<IPredicate>();
            foreach (var id in ids)
            {
                list.Add(Predicates.Field<SchoolEntity>(f => f.Id, Operator.Eq, id));
            }
            IPredicateGroup group = Predicates.Group(GroupOperator.Or, list.ToArray());
            IEnumerable<SchoolEntity> dbList = _schoolDao.GetList(group);
            return dbList;
        }
        public void Delete(int[] ids)
        {
            List<SchoolEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = true; });
            _schoolDao.UpdateBatch(dbList);
            RemoveCache();
        }
        public void Restore(int[] ids)
        {
            List<SchoolEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = false; });
            _schoolDao.UpdateBatch(dbList);
            RemoveCache();
        }

        public SchoolEntity Get(int id)
        {
           return _schoolDao.Get(id.ToString());
        }
        public IEnumerable<SchoolEntity> GetListByPage(int pageSize, int pageIndex, string sort, string sortType,
            List<SearchModel> searchModels, out long count)
        {
            return _schoolDao.GetListByPage<SchoolEntity>(pageSize, pageIndex, sort, sortType, searchModels,
                out count);
        }

    }
}
