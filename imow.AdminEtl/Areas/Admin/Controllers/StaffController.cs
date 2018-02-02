using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EC.Common.Extensions;
using imow.AdminEtl.Areas.Admin.Models.Admin;
using imow.AdminEtl.Models;
using imow.Framework.Strategy.Controller;
using imow.Framework.Tool;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;
using imow.Services.BussinessService.Admin;
using Imow.Framework.Tool;

namespace imow.AdminEtl.Areas.Admin.Controllers
{
    public class StaffController : BaseController
    {
        private readonly StaffService _service;
        private readonly SchoolService _schoolService;
        public StaffController(SchoolService schoolService, StaffService service)
        {
            _schoolService = schoolService;
            _service = service;
        }
        public ActionResult Index(string tab)
        {
            tab = string.IsNullOrEmpty(tab) ? "list" : tab;
            ViewBag.tab = tab;
            IEnumerable<SchoolEntity> schoolList = _schoolService.GetAll();
            StaffModel model = new StaffModel();
            model.SchoolList = schoolList;
            return View(model);
        }

        [HttpPost]
        [PermissionFilter("/Staff/Index")]
        public ActionResult List(ListSearchModel searchModel)
        {
            SearchModel tmpModel = searchModel.SearchModels.FirstOrDefault(f => f.Column == "CName");
            if (tmpModel!=null)
            {
                SearchModel newModel = new SearchModel
                {
                    Column = "EName",
                    Operator = tmpModel.Operator,
                    Value = tmpModel.Value
                };
                searchModel.SearchModels.Add(newModel);
            }

            long count = 0L;
            var list = _service.GetListByPage(searchModel.PageSize, searchModel.PageIndex, searchModel.Order, searchModel.OrderType, searchModel.SearchModels, out count);
            DatatablesJson<StaffEntity> json = new DatatablesJson<StaffEntity>
            {
                Data = list,
                RecordsFiltered = count,
                RecordsTotal = count
            };
            return Content(json.ToJsonString());
        }

        [HttpGet]
        [PermissionFilter("/Staff/Modify")]
        public ActionResult Add()
        {
            //获取校区列表
            IEnumerable<SchoolEntity> schoolList = _schoolService.GetAll();
            StaffModel model = new StaffModel
            {
                SchoolList = schoolList,
                Entity = new StaffEntity()
            };
            return View("Edit", model);
        }

        private ActionResult Add(StaffEntity model)
        {
            JsonResponse jsonResult = new JsonResponse();
            try
            {
                model.CreateTime = DateTime.Now;
                model.IsDel = false;
                model.IsStop = false;
                _service.Add(model);
                jsonResult.Success = true;
            }
            catch (Exception e)
            {
                jsonResult = ErrorResponse(e.Message);
            }
            return jsonResult.ToJsonResult();
        }

        [HttpPost]
        public ActionResult Modify(StaffEntity model)
        {
            if (model.Id == 0)
            {
                return Add(model);
            }
            return Edit(model);
        }

        [HttpGet]
        [PermissionFilter("/Staff/Modify")]
        public ActionResult Edit(int id)
        {
            StaffEntity entity = _service.Get(id);
            //获取校区列表

            if (entity == null)
            {
                return InvokeHttp404();
            }
            IEnumerable<SchoolEntity> schoolList = _schoolService.GetAll();
            StaffModel model = new StaffModel();
            model.SchoolList = schoolList;
            model.Entity = entity;
            return View(model);
        }

        private ActionResult Edit(StaffEntity model)
        {
            JsonResponse jsonResult = new JsonResponse();
            try
            {
                StaffEntity db = _service.Get(model.Id);
                if (db == null)
                {
                    jsonResult.Success = false;
                    jsonResult.Message = "数据不存在";
                }
                else
                {
                    MapperHelper.Copy(model, db, ignore: new[] { "createtime", "id", "isdel" });
                    _service.Update(db);
                    jsonResult.Success = true;
                }
            }
            catch (Exception e)
            {
                jsonResult = ErrorResponse(e.Message);
            }
            return jsonResult.ToJsonResult();
        }

        [HttpPost]
        [PermissionFilter("/Staff/Modify")]
        public ActionResult Delete(string ids)
        {
            JsonResponse jsonResult = new JsonResponse();
            try
            {
                int[] idArr = ids.SplitRemoveEmptyToInt32(',');
                _service.Delete(idArr);
                jsonResult.Success = true;
            }
            catch (Exception e)
            {
                jsonResult = ErrorResponse(e.Message);
            }
            return jsonResult.ToJsonResult();
        }
        [HttpPost]
        [PermissionFilter("/Staff/Modify")]
        public ActionResult Restore(string ids)
        {
            JsonResponse jsonResult = new JsonResponse();
            try
            {
                int[] idArr = ids.SplitRemoveEmptyToInt32(',');
                _service.Restore(idArr);
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