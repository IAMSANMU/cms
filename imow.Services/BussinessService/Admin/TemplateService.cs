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
    public class TemplateService:IBaseService
    {
        private readonly IBaseRepository<TemplateEntity> _dao;

        public TemplateService(IBaseRepository<TemplateEntity> dao)
        {
            _dao = dao;
        }


        public void Add(TemplateEntity entity)
        {
            _dao.Add(entity);
        }

        public void Update(TemplateEntity entity)
        {
            _dao.Update(entity);
        }

        public IEnumerable<TemplateEntity> GetAll()
        {
            return _dao.GetList(Predicates.Field<TemplateEntity>(f => f.IsDel, Operator.Eq, false));
        }

        public IEnumerable<TemplateEntity> GetList(int[] ids)
        {
            List<IPredicate> list = new List<IPredicate>();
            foreach (var id in ids)
            {
                list.Add(Predicates.Field<TemplateEntity>(f => f.Id, Operator.Eq, id));
            }
            IPredicateGroup group = Predicates.Group(GroupOperator.And, list.ToArray());
            IEnumerable<TemplateEntity> dbList = _dao.GetList(group);
            return dbList;
        }
        public void Delete(int[] ids)
        {
            List<TemplateEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = true; });
            _dao.UpdateBatch(dbList);
        }
        public void Restore(int[] ids)
        {
            List<TemplateEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = false; });
            _dao.UpdateBatch(dbList);
        }

        public TemplateEntity Get(int id)
        {
            return _dao.Get(id.ToString());
        }
        public IEnumerable<TemplateEntity> GetListByPage(int pageSize, int pageIndex, string sort, string sortType,
            List<SearchModel> searchModels, out long count)
        {
            return _dao.GetListByPage<TemplateEntity>(pageSize, pageIndex, sort, sortType, searchModels,
                out count);
        }

    }
}
