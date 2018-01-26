using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Areas.Admin.Models.Admin
{
    public class AdminModuleModel 
    {
        public AdminModuleEntity Entity { get; set; }
        public AdminModuleEntity ParentModule { get; set; }
    }
}
