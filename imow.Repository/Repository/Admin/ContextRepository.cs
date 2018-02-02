using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using imow.IRepository.Admin;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;

namespace imow.Repository.Repository.Admin
{
    public class ContextRepository:BaseRepository<ContextEntity>, IContextRepository
    {

        public override IEnumerable<T> GetListByPage<T>(int pageSize, int pageIndex, string sort, string sortType, List<SearchModel> searchModels, out long count)
        {
            Func<ContextEntity, SectionEntity, ContextEntity> map = (context, section) =>
            {
                context.SectionEntity = section;
                return context;
            };

            PredicateGroup group = BuildPredGroup<T>(searchModels);

            IList<ISort> sortList = BuildSort<T>(sort, sortType);

            return (IEnumerable<T>) DapperHelper.GetPageList(pageIndex, pageSize, out count,map, group, sortList);
        }

        public IEnumerable<ContextEntity> GetListByPage(int pageSize, int pageIndex, out long count)
        {
            List<IPredicate> list=new List<IPredicate>();
            list.Add(Predicates.Field<ContextEntity>(f=>f.IsDel,Operator.Eq,false));
            list.Add(Predicates.Field<ContextEntity>(f => f.IsShow, Operator.Eq, true));
            list.Add(Predicates.Field<ContextEntity>(f => f.Status, Operator.Eq, 2));
            var group = Predicates.Group(GroupOperator.And, list.ToArray());
            List<ISort> sortList = new List<ISort>();
            sortList.Add(Predicates.Sort<ContextEntity>(f => f.CreateTime, false));
            return DapperHelper.GetPageList<ContextEntity>(pageIndex, pageSize, out count, group, sortList);
        }

        public IEnumerable<ContextEntity> GetTop(int pageSize)
        {
            List<IPredicate> list = new List<IPredicate>();
            list.Add(Predicates.Field<ContextEntity>(f => f.IsDel, Operator.Eq, false));
            list.Add(Predicates.Field<ContextEntity>(f => f.IsShow, Operator.Eq, true));
            list.Add(Predicates.Field<ContextEntity>(f => f.Status, Operator.Eq, 2));
            var group = Predicates.Group(GroupOperator.And, list.ToArray());
            List<ISort> sortList = new List<ISort>();
            sortList.Add(Predicates.Sort<ContextEntity>(f => f.IsTop, false));
            sortList.Add(Predicates.Sort<ContextEntity>(f => f.CreateTime, false));
            long count;
            return DapperHelper.GetPageList<ContextEntity>(1, pageSize, out count, group, sortList);
        }
    }
}
