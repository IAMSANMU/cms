using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Areas.Admin.Models.Admin
{
    public class ContextModel
    {
        public IEnumerable<SectionEntity> SectionList { get; set; }

        public ContextEntity ContextEntity { get; set; }

        public IEnumerable<TemplateEntity> TemplateList { get; set; }
    }
}