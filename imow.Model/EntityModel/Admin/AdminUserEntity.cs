using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperExtensions.Mapper;
using EC.Common.Extensions;
using Newtonsoft.Json;

namespace imow.Model.EntityModel.Admin
{
    /// <summary>
    /// imow.Model.EntityModel.Admin：实体对象
    /// </summary>
    [Serializable]
    public class AdminUserEntity
    {
        /// <summary>
        ///主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///角色ID
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        ///登录账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///密码
        /// </summary>
        [JsonIgnore]
        public string Pwd { get; set; }

        /// <summary>
        ///最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        ///真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        ///性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        ///手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        ///邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///QQ号码
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///停用
        /// </summary>
        public bool IsStop { get; set; }

        /// <summary>
        ///删除
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        ///最后更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public List<AdminUserRoleEntity> RoleList { get; set; }
        public bool HasRole(long roleId)
        {
            var roles = this.RoleId.SplitRemoveEmpty(',').Select(i => long.Parse(i));
            return roles.Contains(roleId);
        }
    }

    /// <summary>
    /// AdminUser：实体对象映射关系
    /// </summary>
    [Serializable]
    public class AdminUserEntityOrmMapper : ClassMapper<AdminUserEntity>
    {
        public AdminUserEntityOrmMapper()
        {
            base.Table("ecAdminUser");
            Map(f => f.RoleList).Ignore();//设置忽略
            Map(f => f.Id).Key(KeyType.Assigned);//设置主键  (如果主键名称不包含字母“ID”，请设置)      
            AutoMap();
        }
    }

    public class AdminUserSearch
    {
        public bool ShowLogicDelData { get; set; }
        public string UserName { get; set; }
    }
}
