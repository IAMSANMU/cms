using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EC.Common.Extensions;
using imow.AdminEtl.Models;
using imow.Framework.Strategy.Controller;
using imow.Framework.Tool;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;
using imow.Services.BussinessService.Admin;
using Imow.Framework.Tool;

namespace imow.AdminEtl.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private readonly OrderService _service;
        public OrderController(OrderService service)
        {
            _service = service;
        }
        public ActionResult Index(string tab)
        {
            tab = string.IsNullOrEmpty(tab) ? "list" : tab;
            ViewBag.tab = tab;
            return View();
        }

        [HttpPost]
        [PermissionFilter("/Order/Index")]
        public ActionResult List(ListSearchModel searchModel)
        {
            SearchModel model = searchModel.SearchModels.FirstOrDefault(f => f.Column.Equals("EndTime"));

            if (model != null)
            {
                model.Column = "CreateTime";
                model.Value = model.Value + " 23:59:59";
            }


            long count = 0L;
            var list = _service.GetListByPage(searchModel.PageSize, searchModel.PageIndex, searchModel.Order, searchModel.OrderType, searchModel.SearchModels, out count);
            DatatablesJson<OrderEntity> json = new DatatablesJson<OrderEntity>
            {
                Data = list,
                RecordsFiltered = count,
                RecordsTotal = count
            };
            return Content(json.ToJsonString());
        }


        [PermissionFilter("/Order/Modify")]
        public ActionResult View(int id)
        {
            OrderEntity entity = _service.Get(id);
            return View(entity);
        }

        [HttpPost]
        [PermissionFilter("/Order/Modify")]
        public ActionResult Choose(string ids)
        {
            JsonResponse jsonResult = new JsonResponse();
            try
            {
                int[] idArr = ids.SplitRemoveEmptyToInt32(',');
                _service.Choose(idArr);
                jsonResult.Success = true;
            }
            catch (Exception e)
            {
                jsonResult = ErrorResponse(e.Message);
            }
            return jsonResult.ToJsonResult();
        }

    }
}