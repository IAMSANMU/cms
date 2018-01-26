using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using imow.Framework.Tool;
using imow.IRepository;
using imow.IRepository.Admin;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;
using Imow.Framework.Db;
using Imow.Framework.Cache.DistributedCache.Storage;
using imow.Core;

namespace imow.Services.BussinessService.Admin
{
    public class CompanyStrutsService : IBaseService
    {
        private readonly ICompanyStrutsRepository _companyDao;
        private readonly PermissionService _permService;
        private readonly CacheFactory _cache;


        public CompanyStrutsService(ICompanyStrutsRepository companyDao, CacheFactory cache, PermissionService permService)
        {
            _companyDao = companyDao;
            _permService = permService;
            _cache = cache;
        }

        public IEnumerable<CompanyStrutsEntity> GetAll()
        {
            IList<IPredicate> list = new List<IPredicate>();
            list.Add(Predicates.Field<CompanyStrutsEntity>(f => f.IsDel, Operator.Eq, false));
            var group = Predicates.Group(GroupOperator.And, list.ToArray());
            return _companyDao.GetList(group);
        }
        /// <summary>
        /// 获取父级(包含当前)
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public IEnumerable<CompanyStrutsEntity> GetParentByAdminId(long adminId)
        {
            return _companyDao.GetParentByAdminId(adminId);
        }
        /// <summary>
        /// 获取父级(包含当前)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<CompanyStrutsEntity> GetParentsById(long id)
        {
            return _companyDao.GetParentById(id);
        }
        public IEnumerable<CompanyStrutsEntity> GetChildrenById(long id)
        {
            return _companyDao.GetChildrenById(id);
        }

        public IEnumerable<CompanyStrutsEntity> GetCompanyList()
        {
            IList<IPredicate> list = new List<IPredicate>();
            list.Add(Predicates.Field<CompanyStrutsEntity>(f => f.IsDel, Operator.Eq, false));
            list.Add(Predicates.Field<CompanyStrutsEntity>(f => f.Pid, Operator.Eq, 0));
            list.Add(Predicates.Field<CompanyStrutsEntity>(f => f.IsLast, Operator.Eq,1));
            var group = Predicates.Group(GroupOperator.And, list.ToArray());
            return _companyDao.GetList(group);
        }


        public IEnumerable<CompanyStrutsEntity> GetListByPage(int pageSize, int pageIndex, string sort, string sortType,
            List<SearchModel> searchModels, out long count)
        {
            return _companyDao.GetListByPage<CompanyStrutsEntity>(pageSize, pageIndex, sort, sortType, searchModels,
                out count);
        }

        public CompanyStrutsEntity Get(string id)
        {
            return _companyDao.Get(id);
        }

        public void Update(CompanyStrutsEntity entity)
        {
            _companyDao.Update(entity);
        }

        public void Add(CompanyStrutsEntity entity)
        {
            entity.CreateTime = DateTime.Now;
            entity.Id = ID.GetNextId();
            using (TransactionScope scope=new TransactionScope())
            {
                if (entity.Levels == 1)
                {
                    //增加公司 增加一个超级管理员权限
                    _permService.AddAdminRole(entity.Id);
                }
                _companyDao.Add(entity);
                scope.Complete();
            }
        }
        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="ids"></param>
        public void Delete(long[] ids)
        {
            _companyDao.LogicDelete(ids);
        }
        /// <summary>
        /// 还原删除
        /// </summary>
        /// <param name="ids"></param>
        public void Restore(long[] ids)
        {
            _companyDao.Restore(ids);
        }

        /// <summary>
        /// 获取当前公司部门
        /// </summary>
        /// <param name="code">公司code</param>
        /// <returns></returns>
        public IEnumerable<CompanyStrutsEntity> GetDepartList(string code)
        {
            return _companyDao.GetDepartList(code);
        }


        /// <summary>
        /// 获得当前人当前公司的部门
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="comapnyId"></param>
        /// <returns></returns>
        public IEnumerable<CompanyStrutsEntity> GetDepartByAdminId(long adminId, long comapnyId)
        {
            return _companyDao.GetDepartByAdminId(adminId, comapnyId);
        }

        /// <summary>
        /// 根据编码获取部门信息
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public IEnumerable<CompanyStrutsEntity> GetDepartByCode(string code)
        {
            return _companyDao.GetDepartByCode(code);
        }

        /// <summary>
        /// 获取用户所分配的公司列表
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public IEnumerable<CompanyStrutsEntity> GetOwnerCompany(long adminId)
        {
            string cacheKey = string.Format(CacheKey.CompanyKey, adminId);
            IEnumerable<CompanyStrutsEntity> data = _cache.GetValue<IEnumerable<CompanyStrutsEntity>>(cacheKey);
            if (data == null) {
               data=_companyDao.GetOwnerCompany(adminId);
                _cache.SetValue(cacheKey, data);
            }
            return data;
        }
    }
}
