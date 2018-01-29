using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using imow.IRepository;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;

namespace imow.Services.BussinessService.Admin
{
    public class ContextService : IBaseService
    {
        private readonly IBaseRepository<ContextEntity> _dao;

        public ContextService(IBaseRepository<ContextEntity> dao)
        {
            _dao = dao;
        }


        public void Add(ContextEntity entity)
        {
            _dao.Add(entity);
        }

        public void Update(ContextEntity entity)
        {
            _dao.Update(entity);
        }


        public IEnumerable<ContextEntity> GetList(int[] ids)
        {
            List<IPredicate> list = new List<IPredicate>();
            foreach (var id in ids)
            {
                list.Add(Predicates.Field<ContextEntity>(f => f.Id, Operator.Eq, id));
            }
            IPredicateGroup group = Predicates.Group(GroupOperator.And, list.ToArray());
            IEnumerable<ContextEntity> dbList = _dao.GetList(group);
            return dbList;
        }
        public void Delete(int[] ids)
        {
            List<ContextEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = true; });
            _dao.UpdateBatch(dbList);
        }
        public void Restore(int[] ids)
        {
            List<ContextEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = false; });
            _dao.UpdateBatch(dbList);
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

    }
}
