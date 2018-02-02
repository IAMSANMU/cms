using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Models
{
    public class SchoolModel
    {
        public List<SchoolEntity> SchoolList { get; set; }

        public List<ClassEntity> ClassList { get; set; }

        public List<PhotoImgEntity> PhotoList { get; set; }
        public SchoolEntity Entity { get; set; }
    }
}