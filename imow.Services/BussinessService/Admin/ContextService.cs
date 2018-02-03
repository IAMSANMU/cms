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
    public class ContextService : IBaseService
    {
        private readonly IContextRepository _dao;
        private readonly CacheFactory _cache;
        public ContextService(IContextRepository dao, CacheFactory cache)
        {
            _dao = dao;
            _cache = cache;
        }

        public void RemoveCache()
        {
            _cache.Delete(new [] {CacheKey.TopNewsCacheKey });
        }

        public void Add(ContextEntity entity)
        {
            _dao.Add(entity);
            BuildHtml(entity);
        }

        public void Update(ContextEntity entity)
        {
            _dao.Update(entity);
            BuildHtml(entity);
        }

        private void BuildHtml(ContextEntity entity)
        {
            //TODO 生成静态页
            RemoveCache();
        }

        public IEnumerable<ContextEntity> GetList(int[] ids)
        {
            List<IPredicate> list = new List<IPredicate>();
            foreach (var id in ids)
            {
                list.Add(Predicates.Field<ContextEntity>(f => f.Id, Operator.Eq, id));
            }
            IPredicateGroup group = Predicates.Group(GroupOperator.Or, list.ToArray());
            IEnumerable<ContextEntity> dbList = _dao.GetList(group);
            return dbList;
        }
        public void Delete(int[] ids)
        {
            List<ContextEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = true; });
            _dao.UpdateBatch(dbList);
            RemoveCache();
        }
        public void Restore(int[] ids)
        {
            List<ContextEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = false; });
            _dao.UpdateBatch(dbList);
            RemoveCache();
        }

        public ContextEntity Get(int id)
        {
            return _dao.Get(id.ToString());
        }
        public IEnumerable<ContextEntity> GetListByPage(int pageSize, int pageIndex, string sort, string sortType,
            List<SearchModel> searchModels, out long count)
        {
            return _dao.GetListByPage<ContextEntity>(pageSize, pageIndex, sort, sortType, searchModels,
                out count);
        }

        #region 前端方法

        public IEnumerable<ContextEntity> GetListByPage(int pageSize,int pageIndex,out long count)
        {
            return _dao.GetListByPage(pageSize, pageIndex, out count);
        }

        public IEnumerable<ContextEntity> GetTop(int pageSize)
        {
            return _cache.GetOrSetValue(CacheKey.TopNewsCacheKey,()=> _dao.GetTop(pageSize));
        }


        #endregion
    }
}
