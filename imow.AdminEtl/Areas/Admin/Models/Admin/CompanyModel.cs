using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Areas.Admin.Models.Admin
{
    public class CompanyModel
    {
        public CompanyStrutsEntity Entity { get; set; }
        public CompanyStrutsEntity ParentModule { get; set; }
    }
}
