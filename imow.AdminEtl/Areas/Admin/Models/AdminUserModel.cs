using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Models.Admin
{
    public class AdminUserModel
    {
        public List<AdminRoleEntity> RoleList { get; set; }

        public AdminUserEntity UserEntity { get; set; }

    }
}