using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EC.Common.Extensions;
using imow.AdminEtl.Areas.Admin.Models.Admin;
using imow.AdminEtl.Models;
using imow.Framework.Strategy.Controller;
using imow.Framework.Tool;
using imow.Model.EntityModel.Admin;
using imow.Services.BussinessService.Admin;
using Imow.Framework.Tool;

namespace imow.AdminEtl.Areas.Admin.Controllers
{
    public class ContextController : BaseController
    {
        private readonly ContextService _service;
        private readonly SectionService _sectionService;
        private readonly TemplateService _templateService;
        public ContextController(ContextService service, SectionService sectionService, TemplateService templateService)
        {
            _service = service;
            _templateService = templateService;
            _sectionService = sectionService;
        }
        public ActionResult Index(string tab)
        {
            tab = string.IsNullOrEmpty(tab) ? "list" : tab;
            ViewBag.tab = tab;

            IEnumerable<SectionEntity> sectionList= _sectionService.GetAll();
            ContextModel model=new ContextModel();
            model.SectionList = sectionList;
            return View(model);
        }

        [HttpPost]
        [PermissionFilter("/Context/Index")]
        public ActionResult List(ListSearchModel searchModel)
        {
            long count = 0L;
            var list = _service.GetListByPage(searchModel.PageSize, searchModel.PageIndex, searchModel.Order, searchModel.OrderType, searchModel.SearchModels, out count);
            DatatablesJson<ContextEntity> json = new DatatablesJson<ContextEntity>
            {
                Data = list,
                RecordsFiltered = count,
                RecordsTotal = count
            };
            return Content(json.ToJsonString());
        }

        [HttpGet]
        [PermissionFilter("/Context/Modify")]
        public ActionResult Add()
        {
            ContextEntity entity = new ContextEntity();
            IEnumerable<SectionEntity> sectionList = _sectionService.GetAll();

            //获取模板列表
            IEnumerable<TemplateEntity> templateList = _templateService.GetAll();

            ContextModel model = new ContextModel();
            model.SectionList = sectionList;
            model.ContextEntity = entity;
            model.TemplateList = templateList;


            return View("Edit", model);
        }

        private ActionResult Add(ContextEntity model)
        {
            JsonResponse jsonResult = new JsonResponse();
            try
            {
                if (string.IsNullOrEmpty(model.Title))
                {
                    jsonResult.Success = false;
                    jsonResult.Message = "文章标题不能为空";
                }
                else
                {
                    model.CreateTime = DateTime.Now;
                    model.IsDel = false;
                    _service.Add(model);
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
        public ActionResult Modify(ContextEntity model)
        {
            if (model.Id == 0)
            {
                return Add(model);
            }
            return Edit(model);
        }

        [HttpGet]
        [PermissionFilter("/Context/Modify")]
        public ActionResult Edit(int id)
        {
            ContextEntity entity = _service.Get(id);
            if (entity == null)
            {
                return InvokeHttp404();
            }
            return View(entity);
        }

        private ActionResult Edit(ContextEntity model)
        {
            JsonResponse jsonResult = new JsonResponse();
            try
            {
                if (string.IsNullOrEmpty(model.Title))
                {
                    jsonResult.Success = false;
                    jsonResult.Message = "文章标题不能为空";
                }
                else
                {
                    ContextEntity db = _service.Get(model.Id);
                    if (db == null)
                    {
                        jsonResult.Success = false;
                        jsonResult.Message = "数据不存在";
                    }
                    else
                    {
                        MapperHelper.Copy(model, db, ignore: new[] { "createtime", "id", "isdel", "decodecontext" });
                        _service.Update(db);
                        jsonResult.Success = true;
                    }
                }
            }
            catch (Exception e)
            {
                jsonResult = ErrorResponse(e.Message);
            }
            return jsonResult.ToJsonResult();
        }

        [HttpPost]
        [PermissionFilter("/Context/Modify")]
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
        [PermissionFilter("/Context/Modify")]
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