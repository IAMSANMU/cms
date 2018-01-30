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
    public class LoopService : IBaseService
    {
        private readonly IBaseRepository<IndexLoopEntity> _dao;
        public LoopService(IBaseRepository<IndexLoopEntity> dao)
        {
            _dao = dao;
        }

        public void Add(IndexLoopEntity entity)
        {
            _dao.Add(entity);
        }

        public void Update(IndexLoopEntity entity)
        {
            _dao.Update(entity);
        }

        public IEnumerable<IndexLoopEntity> GetAll( )
        {
            return _dao.GetList(Predicates.Field<IndexLoopEntity>(f=>f.IsDel,Operator.Eq,false));
        }

        public IEnumerable<IndexLoopEntity> GetList(int[] ids)
        {
            List<IPredicate> list = new List<IPredicate>();
            foreach (var id in ids)
            {
                list.Add(Predicates.Field<IndexLoopEntity>(f => f.Id, Operator.Eq, id));
            }
            IPredicateGroup group = Predicates.Group(GroupOperator.And, list.ToArray());
            IEnumerable<IndexLoopEntity> dbList = _dao.GetList(group);
            return dbList;
        }
        public void Delete(int[] ids)
        {
            List<IndexLoopEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = true; });
            _dao.UpdateBatch(dbList);
        }
        public void Restore(int[] ids)
        {
            List<IndexLoopEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = false; });
            _dao.UpdateBatch(dbList);
        }

        public IndexLoopEntity Get(int id)
        {
           return _dao.Get(id.ToString());
        }
        public IEnumerable<IndexLoopEntity> GetListByPage(int pageSize, int pageIndex, string sort, string sortType,
            List<SearchModel> searchModels, out long count)
        {
            return _dao.GetListByPage<IndexLoopEntity>(pageSize, pageIndex, sort, sortType, searchModels,
                out count);
        }

    }
}
