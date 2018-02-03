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
    public class SectionService : IBaseService
    {
        private readonly IBaseRepository<SectionEntity> _dao;

        public SectionService(IBaseRepository<SectionEntity> dao)
        {
            _dao = dao;
        }


        public void Add(SectionEntity entity)
        {
            _dao.Add(entity);
        }

        public void Update(SectionEntity entity)
        {
            _dao.Update(entity);
        }

        public IEnumerable<SectionEntity> GetAll()
        {
            return _dao.GetList(Predicates.Field<SectionEntity>(f => f.IsDel, Operator.Eq, false));
        }

        public IEnumerable<SectionEntity> GetList(int[] ids)
        {
            List<IPredicate> list = new List<IPredicate>();
            foreach (var id in ids)
            {
                list.Add(Predicates.Field<SectionEntity>(f => f.Id, Operator.Eq, id));
            }
            IPredicateGroup group = Predicates.Group(GroupOperator.Or, list.ToArray());
            IEnumerable<SectionEntity> dbList = _dao.GetList(group);
            return dbList;
        }
        public void Delete(int[] ids)
        {
            List<SectionEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = true; });
            _dao.UpdateBatch(dbList);
        }
        public void Restore(int[] ids)
        {
            List<SectionEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = false; });
            _dao.UpdateBatch(dbList);
        }

        public SectionEntity Get(int id)
        {
            return _dao.Get(id.ToString());
        }
        public IEnumerable<SectionEntity> GetListByPage(int pageSize, int pageIndex, string sort, string sortType,
            List<SearchModel> searchModels, out long count)
        {
            return _dao.GetListByPage<SectionEntity>(pageSize, pageIndex, sort, sortType, searchModels,
                out count);
        }

    }
}
