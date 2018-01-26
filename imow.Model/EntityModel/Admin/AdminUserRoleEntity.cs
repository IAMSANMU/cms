using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperExtensions.Mapper;

namespace imow.Model.EntityModel.Admin
{
    /// <summary>
    /// imow.Model.EntityModel.Admin：实体对象
    /// </summary>
    [Serializable]
    public class AdminUserRoleEntity
    {
        public int Id { get; set; }
        /// <summary>
        ///AdminID
        /// </summary>
        public long  AdminId { get; set; }

        /// <summary>
        ///RoleID
        /// </summary>
        public long RoleId { get; set; }
        public AdminRoleEntity AdminRoleEntity { get; set; }
    }

    /// <summary>
    /// AdminUserRole：实体对象映射关系
    /// </summary>
    [Serializable]
    public class AdminUserRoleEntityOrmMapper : ClassMapper<AdminUserRoleEntity>
    {
        public AdminUserRoleEntityOrmMapper()
        {
            base.Table("ecAdminUserRole");
            Map(f => f.AdminRoleEntity).Ignore();//设置忽略
            Map(f => f.RoleId).ForignKey<AdminRoleEntity>(r => r.Id);
            Map(f => f.Id).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)      
            AutoMap();
        }
    }
}
