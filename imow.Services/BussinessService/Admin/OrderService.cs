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
    public class OrderService : IBaseService
    {
        private readonly IOrderRepository _dao;
        public OrderService(IOrderRepository dao)
        {
            _dao = dao;
        }

        public void Add(OrderEntity model,out string msg)
        {
            msg = "";
            if (string.IsNullOrEmpty(model.Tel))
            {
                msg = "请输入您的联系电话";
            }
            else if (string.IsNullOrEmpty(model.Name))
            {
                msg = "请输入您的姓名";
            }
            else
            {
                model.CreateTime = DateTime.Now;
                model.Status = 0;
                _dao.Add(model);
            }
        }



        public IEnumerable<OrderEntity> GetList(int[] ids)
        {
            List<IPredicate> list = new List<IPredicate>();
            foreach (var id in ids)
            {
                list.Add(Predicates.Field<OrderEntity>(f => f.Id, Operator.Eq, id));
            }
            IPredicateGroup group = Predicates.Group(GroupOperator.Or, list.ToArray());
            IEnumerable<OrderEntity> dbList = _dao.GetList(group);
            return dbList;
        }
        public void Choose(int[] ids)
        {
            List<OrderEntity> dbList = GetList(ids).ToList();
            dbList.ForEach(f => { f.Status =1;
                f.UpdateTime = DateTime.Now;
            });
            _dao.UpdateBatch(dbList);
        }

        public OrderEntity Get(int id)
        {
           return _dao.Get(id.ToString());
        }
        public IEnumerable<OrderEntity> GetListByPage(int pageSize, int pageIndex, string sort, string sortType,
            List<SearchModel> searchModels, out long count)
        {
            return _dao.GetListByPage<OrderEntity>(pageSize, pageIndex, sort, sortType, searchModels,
                out count);
        }

    }
}
