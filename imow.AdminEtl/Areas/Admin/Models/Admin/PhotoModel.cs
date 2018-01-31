using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Areas.Admin.Models.Admin
{
    public class PhotoModel
    {

        public IEnumerable<PhotoEntity> PhotoList { get; set; }
        public IEnumerable<PhotoImgEntity> ImgList { get; set; }

        public PhotoEntity PhotoEntity { get; set; }

        public PhotoImgEntity PhotoImgEntity { get; set; }

    }
}