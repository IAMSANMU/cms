using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Models
{
    public class TeacherModel
    {
        public List<StaffEntity> ChinaList { get; set; }

        public List<StaffEntity> OtherList { get; set; }
    }
}