using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using imow.Model.EntityModel.Admin;

namespace imow.IRepository.Admin
{
    public interface IAdminRepository : IBaseRepository<AdminUserEntity>
    {
        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="ids"></param>
        void ActiveUser(long[] ids);
        /// <summary>
        /// 停用用户
        /// </summary>
        /// <param name="ids"></param>
        void UnActiveUser(long[] ids);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids"></param>
        void LogicDelete(long[] ids);
        /// <summary>
        /// 还原用户
        /// </summary>
        /// <param name="ids"></param>
        void Restore(long[] ids);


    }
}
