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
    public class OrderRepository :BaseRepository<OrderEntity>,IOrderRepository
    {
        public override OrderEntity Get(string id)
        {
            Func<OrderEntity, ClassEntity, SchoolEntity, OrderEntity> map = (order, classEntity, schoool) =>
            {
                order.ClassEntity = classEntity;
                order.SchoolEntity = schoool;
                return order;
            };
            var group = Predicates.Field<OrderEntity>(f => f.Id, Operator.Eq, id);
            return DapperHelper.Get(map, JoinType.Inner, group).FirstOrDefault();
        }


        public override IEnumerable<T> GetListByPage<T>(int pageSize, int pageIndex, string sort, string sortType, List<SearchModel> searchModels, out long count)
        {
            Func<OrderEntity,ClassEntity,SchoolEntity,OrderEntity> map = (order,classEntity,schoool) =>
            {
                order.ClassEntity = classEntity;
                order.SchoolEntity = schoool;
                return order;
            };

            PredicateGroup group = BuildPredGroup<T>(searchModels);

            IList<ISort> sortList = BuildSort<T>(sort, sortType);

            return (IEnumerable<T>)DapperHelper.GetPageList(pageIndex, pageSize, out count, map, group, sortList,JoinType.Inner,"Id,Id,Id");
        }
    }
}
