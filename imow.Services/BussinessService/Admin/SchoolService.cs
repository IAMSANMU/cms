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
    public class SchoolService : IBaseService
    {
        private readonly IBaseRepository<SchoolEntity> _schoolDao;
        public SchoolService(IBaseRepository<SchoolEntity> schoolDao)
        {
            _schoolDao = schoolDao;
        }

        public void Add(SchoolEntity entity)
        {
            _schoolDao.Add(entity);
        }

        public void Update(SchoolEntity entity)
        {
            _schoolDao.Update(entity);
        }

        public IEnumerable<SchoolEntity> GetAll( )
        {
            return _schoolDao.GetList(Predicates.Field<SchoolEntity>(f=>f.IsDel,Operator.Eq,false));
        }

        public IEnumerable<SchoolEntity> GetList(int[] ids)
        {
            List<IPredicate> list = new List<IPredicate>();
            foreach (var id in ids)
            {
                list.Add(Predicates.Field<SchoolEntity>(f => f.Id, Operator.Eq, id));
            }
            IPredicateGroup group = Predicates.Group(GroupOperator.And, list.ToArray());
            IEnumerable<SchoolEntity> dbList = _schoolDao.GetList(group);
            return dbList;
        }
        public void Delete(int[] ids)
        {
            List<SchoolEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = true; });
            _schoolDao.UpdateBatch(dbList);
        }
        public void Restore(int[] ids)
        {
            List<SchoolEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = false; });
            _schoolDao.UpdateBatch(dbList);
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
