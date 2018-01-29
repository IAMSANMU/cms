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
    public class ArticleService : IBaseService
    {
        private readonly IBaseRepository<ArticleEntity> _dao;

        public ArticleService(IBaseRepository<ArticleEntity> dao)
        {
            _dao = dao;
        }


        public void Add(ArticleEntity entity)
        {
            _dao.Add(entity);
        }

        public void Update(ArticleEntity entity)
        {
            _dao.Update(entity);
        }

        public IEnumerable<ArticleEntity> GetAll()
        {
            return _dao.GetList(Predicates.Field<ArticleEntity>(f => f.IsDel, Operator.Eq, false));
        }

        public IEnumerable<ArticleEntity> GetList(int[] ids)
        {
            List<IPredicate> list = new List<IPredicate>();
            foreach (var id in ids)
            {
                list.Add(Predicates.Field<ArticleEntity>(f => f.Id, Operator.Eq, id));
            }
            IPredicateGroup group = Predicates.Group(GroupOperator.And, list.ToArray());
            IEnumerable<ArticleEntity> dbList = _dao.GetList(group);
            return dbList;
        }
        public void Delete(int[] ids)
        {
            List<ArticleEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = true; });
            _dao.UpdateBatch(dbList);
        }
        public void Restore(int[] ids)
        {
            List<ArticleEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.IsDel = false; });
            _dao.UpdateBatch(dbList);
        }

        public ArticleEntity Get(int id)
        {
            return _dao.Get(id.ToString());
        }
        public IEnumerable<ArticleEntity> GetListByPage(int pageSize, int pageIndex, string sort, string sortType,
            List<SearchModel> searchModels, out long count)
        {
            return _dao.GetListByPage<ArticleEntity>(pageSize, pageIndex, sort, sortType, searchModels,
                out count);
        }

    }
}
