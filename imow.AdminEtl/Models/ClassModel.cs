using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Models
{
    public class ClassModel
    {
        public List<ClassEntity> ClassList { get; set; }

        public List<ClassEntity> TopList { get; set; }

        public ClassEntity Entity { get; set; }
    }
}