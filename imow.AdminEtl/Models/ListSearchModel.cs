using System.Collections.Generic;
using System.Web.Mvc;
using imow.Model.EntityModel;

namespace imow.AdminEtl.Models
{
  //  [ModelBinder(typeof(ListSearchModelModelBinder))]
    public class ListSearchModel
    {
        public int Draw { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string  Order { get; set; }
        public string  OrderType { get; set; }

        public List<SearchModel> SearchModels { get; set; }
    }


  
}