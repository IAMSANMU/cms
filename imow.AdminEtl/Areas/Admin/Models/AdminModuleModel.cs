using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Models.Admin
{
    public class AdminModuleModel 
    {
        public AdminModuleEntity Entity { get; set; }
        public AdminModuleEntity ParentModule { get; set; }
    }
}
