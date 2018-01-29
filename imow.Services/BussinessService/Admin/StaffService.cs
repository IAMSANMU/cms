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
    public class StaffService : IBaseService
    {
        private readonly IBaseRepository<StaffEntity> _staffDao;
        public StaffService(IBaseRepository<StaffEntity> staffDao)
        {
            _staffDao = staffDao;
        }

        public void Add(StaffEntity entity)
        {
            _staffDao.Add(entity);
        }

        public void Update(StaffEntity entity)
        {
            _staffDao.Update(entity);
        }

        public IEnumerable<StaffEntity> GetAll( )
        {
            return _staffDao.GetList(Predicates.Field<StaffEntity>(f=>f.IsDel,Operator.Eq,false));
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
        }
        public void Restore(int[] ids)
        {
            List<StaffEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = false; });
            _staffDao.UpdateBatch(dbList);
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

    }
}
