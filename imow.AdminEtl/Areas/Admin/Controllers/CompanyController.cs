using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EC.Common.Extensions;
using imow.AdminEtl.Areas.Admin.Models.Admin;
using imow.AdminEtl.Models;
using imow.Framework.Strategy.Controller;
using imow.Framework.Strategy.Filter;
using imow.Framework.Tool;
using imow.Model.EntityModel.Admin;
using imow.Services.BussinessService.Admin;
using Newtonsoft.Json;

namespace imow.AdminEtl.Areas.Admin.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly CompanyStrutsService _companyService;

        public CompanyController(CompanyStrutsService companyService)
        {
            _companyService = companyService;
        }

        public ActionResult Index(long? pid, string tab)
        {
            tab = string.IsNullOrEmpty(tab) ? "list" : tab;
            ViewBag.tab = tab;
            pid = pid ?? 0;
            ViewBag.pid = pid;
            //根据pid 获取parentId
            if (pid > 0)
            {
                var entity = _companyService.Get(pid.Value.ToString());
                ViewBag.ppid = entity.Pid;
            }
            else
            {
                ViewBag.ppid = 0;
            }
            return View();
        }
        [PermissionFilter("/Company/Index")]

        public ActionResult List(ListSearchModel model)
        {

            long count;
            var list = _companyService.GetListByPage(model.PageSize, model.PageIndex, model.Order, model.OrderType, model.SearchModels, out count);
            DatatablesJson<CompanyStrutsEntity> json = new DatatablesJson<CompanyStrutsEntity>
            {
                Data = list,
                RecordsFiltered = count,
                RecordsTotal = count
            };
            return Content(json.ToJsonString());
        }

        [PermissionFilter("/Company/Modify")]
        public ActionResult Edit(long id)
        {
            var entity = _companyService.Get(id.ToString());
            if (entity == null)
            {
                return InvokeHttp404();
            }
            var parent = _companyService.Get(entity.Pid.ToString());
            CompanyModel model = new CompanyModel
            {
                Entity = entity,
                ParentModule = parent
            };
            return View(model);
        }
        [PermissionFilter("/Company/Modify")]
        public ActionResult Add(long pid)
        {
            var parent = _companyService.Get(pid.ToString());
            CompanyModel model = new CompanyModel
            {
                ParentModule = parent
            };
            return View(model);
        }
        [PermissionFilter("/Company/Modify")]
        public ActionResult Update(CompanyStrutsEntity model)
        {
            JsonResponse jsonResult = new JsonResponse();
            var entity = _companyService.Get(model.Id.ToString());
            if (entity == null)
            {
                jsonResult.Success = false;
                jsonResult.Message = "数据不存在";
            }
            else
            {
                try
                {
                    entity.Name = model.Name;
                    entity.IsDel = model.IsDel;
                    _companyService.Update(entity);
                    jsonResult.Success = true;
                }
                catch (Exception e)
                {
                    jsonResult = ErrorResponse(e.Message);
                }
            }
            return jsonResult.ToJsonResult();
        }
        [PermissionFilter("/Company/Modify")]
        public ActionResult Save(CompanyStrutsEntity model)
        {
            JsonResponse jsonResult = new JsonResponse();
            try
            {
                _companyService.Add(model);
                jsonResult.Success = true;
            }
            catch (Exception e)
            {
                jsonResult = ErrorResponse(e.Message);
            }
            return jsonResult.ToJsonResult();
        }

        [PermissionFilter("/Company/Modify")]
        public ActionResult Delete(string ids)
        {
            JsonResponse jsonResult = new JsonResponse();
            try
            {
                if (string.IsNullOrEmpty(ids))
                {
                    jsonResult.Success = false;
                    jsonResult.Message = "至少选择一条数据";
                }
                else
                {
                    _companyService.Delete(ids.SplitRemoveEmptyToInt64(','));
                    jsonResult.Success = true;
                }
            }
            catch (Exception e)
            {
                jsonResult = ErrorResponse(e.Message);
            }
            return jsonResult.ToJsonResult();
        }
        [PermissionFilter("/Company/Modify")]
        public ActionResult Restore(string ids)
        {
            JsonResponse jsonResult = new JsonResponse();
            try
            {
                if (string.IsNullOrEmpty(ids))
                {
                    jsonResult.Success = false;
                    jsonResult.Message = "至少选择一条数据";
                }
                else
                {
                    _companyService.Restore(ids.SplitRemoveEmptyToInt64(','));
                    jsonResult.Success = true;
                }
            }
            catch (Exception e)
            {
                jsonResult = ErrorResponse(e.Message);
            }
            return jsonResult.ToJsonResult();
        }



    }
}