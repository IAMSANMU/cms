using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using DapperExtensions;
using imow.Core;
using imow.IRepository;
using imow.IRepository.Admin;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;
using Imow.Framework.Cache.DistributedCache.Storage;
using Imow.Framework.Db;
using Lucene.Net.Search;
using EC.Common.Extensions;
using imow.Framework.Tool;

namespace imow.Services.BussinessService.Admin
{
    public class PermissionService : IBaseService
    {
        private readonly IPermissionRepository _permDao;
        private readonly CacheFactory _cache;
        public PermissionService(IPermissionRepository permDao, CacheFactory cache)
        {
            _cache = cache;
            _permDao = permDao;
        }

        #region Module 相关操作

        /// <summary>
        /// 更新模块,删除缓存
        /// </summary>
        private void DeleteAllModuleCache()
        {
            string[] keys =
            {
                CacheKey.AllModuleKey
            };
            _cache.Delete(keys);
        }

        /// <summary>
        /// 新增模块
        /// </summary>
        /// <param name="entity"></param>
        public void AddModule(AdminModuleEntity entity)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                //depth,code 由trig_insert_code触发
                entity.CreateTime = DateTime.Now;
                entity.IsDel = false;
                _permDao.Add(entity);
                scope.Complete();
                DeleteAllModuleCache();
            }
        }
        /// <summary>
        /// 禁用模块
        /// </summary>
        /// <param name="ids"></param>
        public void DeleteModule(int[] ids)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                _permDao.DeleteModule(ids);
                DeleteAllModuleCache();
                scope.Complete();
            }
        }
        /// <summary>
        /// 还原删除模块
        /// </summary>
        /// <param name="ids"></param>
        public void RestoreModule(int[] ids)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                _permDao.RestoreModule(ids);
                DeleteAllModuleCache();
                scope.Complete();
            }
        }

        /// <summary>
        /// 获取模块, 优先取缓存
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IEnumerable<AdminModuleEntity> GetModuleByUser(AdminUserEntity user)
        {
            var cacheModule = _cache.GetValue<IEnumerable<AdminModuleEntity>>(CacheKey.AllModuleKey);
            bool reload = cacheModule == null;

            IEnumerable<AdminModuleEntity> models = null;
            var cacheUserModule =
                _cache.GetValue<IEnumerable<AdminModuleEntity>>(string.Format(CacheKey.AdminUserModuleKey, user.Id));
            if (reload || cacheUserModule == null)
            {
                GetsAllModule(user);//缓存所有module
                HashSet<int> ids = new HashSet<int>();
                user.RoleList.ForEach(f =>
                    {
                        ids.UnionWith(new HashSet<int>(f.AdminRoleEntity.ModuleIds.SplitRemoveEmptyToInt32(',')));
                    }
                );
                models = GetModuleByIds(user.Id, ids.ToArray());
            }
            else
            {
                models = cacheUserModule;
            }
            return models;
        }

        public IEnumerable<AdminModuleEntity> GetModuleList(int pageSize, int pageIndex, string sort, string sortType,
            List<SearchModel> searchModels, out long count)
        {
            return _permDao.GetListByPage<AdminModuleEntity>(pageSize, pageIndex, sort, sortType, searchModels,
                out count);
        }

        public IEnumerable<AdminModuleEntity> GetModuleByIds(long userId, int[] moduleIds)
        {
            var list = _permDao.GetModuleByIds(moduleIds);
            _cache.SetValue(string.Format(CacheKey.AdminUserModuleKey, userId), list);
            return list;
        }

        public IEnumerable<AdminModuleEntity> GetsAllModule(AdminUserEntity user)
        {
            IEnumerable<AdminModuleEntity> models = _permDao.GetAdminModuleList().Where(f => f.IsDel == false);
            _cache.SetValue(CacheKey.AllModuleKey, models);
            return models;
        }

        #endregion


        #region Role 相关操作
        public IEnumerable<AdminRoleEntity> GetRoleList(bool isDel)
        {
            return _permDao.GetRoleList(isDel);
        }
        public List<AdminRoleEntity> GetRoleList(int pageSize, int pageIndex, string sort, string sortType, List<SearchModel> searchModels, out long count)
        {
            return _permDao.GetListByPage<AdminRoleEntity>(pageSize, pageIndex, sort, sortType, searchModels, out count).ToList();
        }

        public bool CheckRole(string name, long? id)
        {
            List<IPredicate> list = new List<IPredicate>();
            list.Add(Predicates.Field<AdminRoleEntity>(f => f.Name, Operator.Eq, name));
            var data = _permDao.GetRoleList(Predicates.Group(GroupOperator.And, list.ToArray())).FirstOrDefault();
            var flag = false;
            if (id.HasValue)
            {
                if (data == null)
                {
                    flag = true;
                }
                else
                {
                    flag = data.Id == id;
                }
            }
            else
            {
                flag = data == null;
            }
            return flag;
        }

        public void AddRole(AdminRoleEntity entity, string[] modules)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                entity.CreateTime = DateTime.Now;
                entity.Id = ID.GetNextId();
                var addModules = modules.Where(f => !f.Equals("#")).ToArray();
                foreach (var item in addModules)
                {
                    AdminRoleModuleEntity role = new AdminRoleModuleEntity
                    {
                        RoleId = entity.Id,
                        ModuleId = int.Parse(item),
                    };
                    Add(role);

                }
                entity.ModuleIds = string.Join(",", addModules);
                Add(entity);
                scope.Complete();

            }
        }

        public void UpdateRole(AdminRoleEntity entity, string[] modules)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                //删除旧权限模块
                var predicate = Predicates.Field<AdminRoleModuleEntity>(f => f.RoleId, Operator.Eq, entity.Id);
                _permDao.DeleteBatch<AdminRoleModuleEntity>(predicate);

                var addModules = modules.Where(f => !f.Equals("#")).ToArray();
                //保存新权限模块
                foreach (var item in addModules)
                {
                    AdminRoleModuleEntity role = new AdminRoleModuleEntity
                    {
                        RoleId = entity.Id,
                        ModuleId = int.Parse(item),
                    };
                    Add(role);
                }
                entity.ModuleIds = string.Join(",", addModules);
                Update(entity);
                scope.Complete();
            }
        }

        public void DeleteRole(string ids)
        {
            _permDao.LogicDeleteRole(ids.SplitRemoveEmptyToInt64(','));
        }
        public void RestoreRole(string ids)
        {
            _permDao.RestoreRole(ids.SplitRemoveEmptyToInt64(','));
        }


        public IEnumerable<AdminRoleModuleEntity> GetRoleModuleList(long roleId)
        {
            return _permDao.GetRoleModuleList(roleId);
        }


        #endregion

        public T Get<T>(string id) where T : class
        {
            return _permDao.Get<T>(id);
        }
        public void Update<T>(T t) where T : class
        {
            _permDao.Update(t);
            DeleteAllModuleCache();
        }

        public void Add<T>(T t) where T : class
        {
            _permDao.Add(t);
            DeleteAllModuleCache();
        }






    }
}

