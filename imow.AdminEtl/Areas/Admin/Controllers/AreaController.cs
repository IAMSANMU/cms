using System;
using System.Collections.Generic;
using System.Web.Mvc;
using imow.Framework.Strategy.Controller;
using imow.Framework.Strategy.Filter;
using imow.Framework.Tool;
using imow.Model.EntityModel;
using imow.Services.BussinessService;

namespace imow.AdminEtl.Areas.Admin.Controllers
{
    public class AreaController : BaseController
    {
        private readonly AreaService _areaService;

        public AreaController(AreaService areaService)
        {
            _areaService = areaService;
        }
        [NoPerm]
        public ActionResult GetProvince()
        {
            return GetChild(0, "province").ToJsonResult();
        }
        [NoPerm]
        public ActionResult GetCity(int id)
        {
            return GetChild(id, "city").ToJsonResult();
        }
        [NoPerm]
        public ActionResult GetArea(int id)
        {
            return GetChild(id, "area").ToJsonResult();
        }
        [NoPerm]
        public ActionResult GetStree(int id)
        {
            return GetChild(id, "stree").ToJsonResult();
        }

        private JsonResponse GetChild(int id,string type)
        {
            JsonResponse jsonResult =new JsonResponse();
            try
            {
                IEnumerable<AreaEntity> data = null;
                if (type == "city")
                {
                    data= _areaService.GetCityByProvince(id);
                }else if (type == "area")
                {
                    data = _areaService.GetAreaByCity(id);
                }else if (type == "stree")
                {
                    data = _areaService.GetStreeByArea(id);
                }
                else if (type == "province")
                {
                    data = _areaService.GetProvince();
                }
                jsonResult.Success = true;
                jsonResult.Data = data;
            }
            catch (Exception e)
            {
                jsonResult = ErrorResponse(e.Message);
            }
            return jsonResult;
        }
    }
}