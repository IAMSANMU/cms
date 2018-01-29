using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using imow.IRepository;
using imow.IRepository.Admin;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;

namespace imow.Services.BussinessService.Admin
{
    public class ClassService : IBaseService
    {
        private readonly IClassRepository _dao;
        public ClassService(IClassRepository dao)
        {
            _dao = dao;
        }

        public void Add(ClassEntity entity)
        {
            _dao.Add(entity);
        }

        public void Update(ClassEntity entity)
        {
            _dao.Update(entity);
        }

        public IEnumerable<ClassEntity> GetList(int[] ids)
        {
            List<IPredicate> list = new List<IPredicate>();
            foreach (var id in ids)
            {
                list.Add(Predicates.Field<ClassEntity>(f => f.Id, Operator.Eq, id));
            }
            IPredicateGroup group = Predicates.Group(GroupOperator.And, list.ToArray());
            IEnumerable<ClassEntity> dbList = _dao.GetList(group);
            return dbList;
        }
        public void Delete(int[] ids)
        {
            List<ClassEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = true; });
            _dao.UpdateBatch(dbList);
        }
        public void Restore(int[] ids)
        {
            List<ClassEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = false; });
            _dao.UpdateBatch(dbList);
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

    }
}
