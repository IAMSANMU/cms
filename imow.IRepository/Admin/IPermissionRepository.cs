using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;

namespace imow.IRepository.Admin
{
    public interface IPermissionRepository : IBaseRepository<AdminModuleEntity>
    {
        void DeleteModule(int[] ids);

        void RestoreModule(int[] ids);
        /// <summary>
        /// 模块列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<AdminModuleEntity> GetAdminModuleList();
        /// <summary>
        /// 根据权限获取模块
        /// </summary>
        /// <param name="moduleIds"></param>
        /// <returns></returns>
        IEnumerable<AdminModuleEntity> GetModuleByIds(int[] moduleIds);

        /// <summary>
        /// 获取用户的权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<AdminUserRoleEntity> GetUserRoleList(long userId);
        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<AdminRoleEntity> GetRoleList(bool isDel);


        IEnumerable<AdminRoleEntity> GetRoleList(IPredicate group);


        IEnumerable<AdminRoleModuleEntity> GetRoleModuleList(long roleId);
        T Get<T>(string id) where T : class;

        void Update<T>(T t) where T : class;

        void Add<T>(T t) where T : class;

        void Delete<T>(T t) where T : class;

        void DeleteBatch<T>(IPredicate predGroup) where T : class;

        void LogicDeleteRole(long[] ids);

        void RestoreRole(long[] ids);

    }
}
