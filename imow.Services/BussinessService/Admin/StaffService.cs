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
    public class StaffService : IBaseService
    {
        private readonly IStaffRepository _staffDao;
        private readonly CacheFactory _cache;
        public StaffService(IStaffRepository staffDao, CacheFactory cache)
        {
            _staffDao = staffDao;
            _cache = cache;
        }

        public void Add(StaffEntity entity)
        {
            _staffDao.Add(entity);
            RemoveCache();
        }

        public void Update(StaffEntity entity)
        {
            _staffDao.Update(entity);
            RemoveCache();
        }



        public IEnumerable<StaffEntity> GetList(int[] ids)
        {
            List<IPredicate> list = new List<IPredicate>();
            foreach (var id in ids)
            {
                list.Add(Predicates.Field<StaffEntity>(f => f.Id, Operator.Eq, id));
            }
            IPredicateGroup group = Predicates.Group(GroupOperator.And, list.ToArray());
            IEnumerable<StaffEntity> dbList = _staffDao.GetList(group);
            return dbList;
        }
        public void Delete(int[] ids)
        {
            List<StaffEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = true; });
            _staffDao.UpdateBatch(dbList);
            RemoveCache();
        }
        public void Restore(int[] ids)
        {
            List<StaffEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = false; });
            _staffDao.UpdateBatch(dbList);
            RemoveCache();
        }

    public StaffEntity Get(int id)
        {
            return _staffDao.Get(id.ToString());
        }
        public IEnumerable<StaffEntity> GetListByPage(int pageSize, int pageIndex, string sort, string sortType,
            List<SearchModel> searchModels, out long count)
        {
            return _staffDao.GetListByPage<StaffEntity>(pageSize, pageIndex, sort, sortType, searchModels,
                out count);
        }

        public void RemoveCache()
        {
            _cache.Delete(new[] {CacheKey.IndexTeacherCacheKey});
        }

        #region 前端方法
        public IEnumerable<StaffEntity> GetCommond()
        {
            var group = Predicates.Group(GroupOperator.And,
                Predicates.Field<StaffEntity>(f => f.IsDel, Operator.Eq, false),
                Predicates.Field<StaffEntity>(f => f.IsStop, Operator.Eq, false),
                Predicates.Field<StaffEntity>(f => f.IsCommand, Operator.Eq, true));
            return _cache.GetOrSetValue(CacheKey.IndexTeacherCacheKey, ()=> _staffDao.GetList(group));
        }
        #endregion
    }
}
