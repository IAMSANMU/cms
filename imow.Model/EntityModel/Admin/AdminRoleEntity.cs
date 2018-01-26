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
    public class AdminRoleEntity
    {
        /// <summary>
        ///主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///是否删除
        /// </summary>
        public bool IsDel { get; set; }

        public string Remark { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public string ModuleIds { get; set; }

    }

    /// <summary>
    /// AdminRole：实体对象映射关系
    /// </summary>
    [Serializable]
    public class AdminRoleEntityOrmMapper : ClassMapper<AdminRoleEntity>
    {
        public AdminRoleEntityOrmMapper()
        {
            base.Table("ecAdminRole");
            Map(f => f.Id).Key(KeyType.Assigned);//设置主键  (如果主键名称不包含字母“ID”，请设置)      
            AutoMap();
        }
    }
}
