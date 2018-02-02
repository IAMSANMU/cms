using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Models
{
    public class NewsModel
    {
        public List<ContextEntity> NewsList { get; set; }
        public ContextEntity News { get; set; }

        public Paging Page { get; set; }
    }
}