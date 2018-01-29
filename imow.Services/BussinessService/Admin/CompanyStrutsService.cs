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
            using (TransactionScope scope=new TransactionScope())
            {
                if (entity.Levels == 1)
                {
                    //增加公司 增加一个超级管理员权限
                    //_permService.AddAdminRole(entity.Id);
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
    }
}
