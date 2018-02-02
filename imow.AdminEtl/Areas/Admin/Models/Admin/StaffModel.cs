using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Areas.Admin.Models.Admin
{
    public class ClassModel
    {
        public IEnumerable<SchoolEntity> SchoolList { get; set; }
        public ClassEntity Entity { get; set; }
    }
}