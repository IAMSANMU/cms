﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Areas.Admin.Models.Admin
{
    public class StaffModel
    {
        public IEnumerable<SchoolEntity> SchoolList { get; set; }
        public StaffEntity Entity { get; set; }
    }
}