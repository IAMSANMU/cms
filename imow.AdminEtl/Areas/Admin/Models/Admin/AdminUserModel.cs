using System.Collections.Generic;
using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Areas.Admin.Models.Admin
{
    public class AdminUserModel
    {
        public List<AdminRoleEntity> RoleList { get; set; }

        public AdminUserEntity UserEntity { get; set; }

    }
}