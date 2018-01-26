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
    public class AdminRoleModuleEntity
    {
        public int Id { get; set; }

        /// <summary>
        ///RoleID
        /// </summary>
        public long RoleId { get; set; }

        /// <summary>
        ///ModuleID
        /// </summary>
        public int ModuleId { get; set; }

    }

    /// <summary>
    /// AdminRoleModule：实体对象映射关系
    /// </summary>
    [Serializable]
    public class AdminRoleModuleEntityOrmMapper : ClassMapper<AdminRoleModuleEntity>
    {
        public AdminRoleModuleEntityOrmMapper()
        {
            base.Table("ecAdminRoleModule");
            //Map(f => f.UserID).Ignore();//设置忽略
            Map(f => f.Id).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)      
            Map(f => f.ModuleId).ForignKey<AdminModuleEntity>(m => m.Id);
            Map(f => f.RoleId).ForignKey<AdminRoleEntity>(r => r.Id);
            AutoMap();
        }
    }
}
