using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Models
{
    public class IndexModel
    {
        public List<IndexLoopEntity> LoopList { get; set; }
        public List<ClassEntity> ClassList { get; set; }

        public List<StaffEntity> StaffList { get; set; }
        public List<PhotoImgEntity> ImgList { get; set; }

    }
}