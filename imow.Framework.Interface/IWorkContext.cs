using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;

namespace imow.Framework.Interface
{
    public interface IWorkContext
    {

        /// <summary>
        /// 登陆页地址
        /// </summary>
        string LoginUrl { get; }

        AdminUserEntity AdminEntity { get;  }

        bool IsAdminLogined { get; }
        /// <summary>
        /// 当前访问路径的权限URL
        /// </summary>
        IEnumerable<AdminModuleEntity> ModuleList { get; set; }

        string PermStr { get; }
    }
}