using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Models
{
    public class OrderModel
    {
        public List<SchoolEntity> SchoolList { get; set; }
        public List<ClassEntity> ClassList { get; set; }

    }
}