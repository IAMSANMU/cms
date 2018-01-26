using System;
using System.Web.Mvc;
using EC.Common.Extensions;
using imow.AdminEtl.Models;
using imow.AdminEtl.Models.Admin;
using imow.Framework.Strategy.Controller;
using imow.Framework.Tool;
using imow.Model.EntityModel.Admin;
using imow.Services.BussinessService.Admin;

namespace imow.AdminEtl.Areas.Admin.Controllers
{
    public class ModuleController : BaseController
    {
        private readonly PermissionService _permService;

        public ModuleController(PermissionService permService)
        {
            _permService = permService;
        }
        #region 模块相关操作

        public ActionResult Index(int? pid,string tab)
        {
            tab = string.IsNullOrEmpty(tab) ? "list" : tab;
            ViewBag.tab = tab;
            pid = pid ?? 0;
            ViewBag.pid = pid;
            //根据pid 获取parentId
            if (pid > 0)
            {
                var entity = _permService.Get<AdminModuleEntity>(pid.Value.ToString());
                ViewBag.ppid = entity.Pid;
            }
            else
            {
                ViewBag.ppid = 0;
            }
            return View();
        }
        [HttpPost]
        [PermissionFilter("/Module/Index")]
        public ActionResult List(ListSearchModel model)
        {
            long count;
            var list = _permService.GetModuleList(model.PageSize, model.PageIndex, model.Order, model.OrderType, model.SearchModels, out count);
            DatatablesJson<AdminModuleEntity> json = new DatatablesJson<AdminModuleEntity>
            {
                Data = list,
                RecordsFiltered = count,
                RecordsTotal = count
            };
            return Content(json.ToJsonString());
        }
        [PermissionFilter("/Module/Modify")]
        public ActionResult Edit(int id)
        {
            var entity = _permService.Get<AdminModuleEntity>(id.ToString());
            if (entity == null)
            {
                return InvokeHttp404();
            }
            var parent = _permService.Get<AdminModuleEntity>(entity.Pid.ToString());
            AdminModuleModel model = new AdminModuleModel
            {
                Entity = entity,
                ParentModule = parent
            };
            return View(model);
        }
        [PermissionFilter("/Module/Modify")]
        public ActionResult Add(int pid)
        {
            var parent = _permService.Get<AdminModuleEntity>(pid.ToString());
            AdminModuleModel model = new AdminModuleModel
            {
                ParentModule = parent
            };
            return View(model);
        }
        [PermissionFilter("/Module/Modify")]
        public ActionResult Update(AdminModuleEntity model)
        {
            JsonResponse jsonResult = new JsonResponse();
            var entity = _permService.Get<AdminModuleEntity>(model.Id.ToString());
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
                    entity.Url = model.Url;
                    entity.IsShow = model.IsShow;
                    _permService.Update(entity);
                    jsonResult.Success = true;
                }
                catch (Exception e)
                {
                    jsonResult = ErrorResponse(e.Message);
                }
            }
            return jsonResult.ToJsonResult();
        }
        [PermissionFilter("/Module/Modify")]
        public ActionResult Save(AdminModuleEntity model)
        {
            JsonResponse jsonResult = new JsonResponse();
            try
            {
                _permService.AddModule(model);
                jsonResult.Success = true;
            }
            catch (Exception e)
            {
                jsonResult = ErrorResponse(e.Message);
            }
            return jsonResult.ToJsonResult();
        }

        [PermissionFilter("/Module/Modify")]
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
                    _permService.DeleteModule(ids.SplitRemoveEmptyToInt32(','));
                    jsonResult.Success = true;
                }
            }
            catch (Exception e)
            {
                jsonResult = ErrorResponse(e.Message);
            }
            return jsonResult.ToJsonResult();
        }
        [PermissionFilter("/Module/Modify")]
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
                    _permService.RestoreModule(ids.SplitRemoveEmptyToInt32(','));
                    jsonResult.Success = true;
                }
            }
            catch (Exception e)
            {
                jsonResult = ErrorResponse(e.Message);
            }
            return jsonResult.ToJsonResult();
        }
        #endregion

    }
}