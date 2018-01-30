using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EC.Common.Extensions;
using imow.AdminEtl.Models;
using imow.Framework.Strategy.Controller;
using imow.Framework.Tool;
using imow.Model.EntityModel.Admin;
using imow.Services.BussinessService.Admin;
using Imow.Framework.Tool;

namespace imow.AdminEtl.Areas.Admin.Controllers
{
    public class LoopController : BaseController
    {
        private readonly LoopService _service;
        public LoopController(LoopService service)
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
        [PermissionFilter("/Loop/Index")]
        public ActionResult List(ListSearchModel searchModel)
        {
            long count = 0L;
            var list = _service.GetListByPage(searchModel.PageSize, searchModel.PageIndex, searchModel.Order, searchModel.OrderType, searchModel.SearchModels, out count);
            DatatablesJson<IndexLoopEntity> json = new DatatablesJson<IndexLoopEntity>
            {
                Data = list,
                RecordsFiltered = count,
                RecordsTotal = count
            };
            return Content(json.ToJsonString());
        }

        [HttpGet]
        [PermissionFilter("/Loop/Modify")]
        public ActionResult Add()
        {
            IndexLoopEntity model = new IndexLoopEntity();
            return View("Edit", model);
        }

        private ActionResult Add(IndexLoopEntity model)
        {
            JsonResponse jsonResult = new JsonResponse();
            try
            {
                if (string.IsNullOrEmpty(model.Img))
                {
                    jsonResult.Success = false;
                    jsonResult.Message = "请上传图片";
                }
                else if (string.IsNullOrEmpty(model.Link))
                {
                    jsonResult.Success = false;
                    jsonResult.Message = "跳转地址不能为空";
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
        public ActionResult Modify(IndexLoopEntity model)
        {
            if (model.Id == 0)
            {
                return Add(model);
            }
            return Edit(model);
        }

        [HttpGet]
        [PermissionFilter("/Loop/Modify")]
        public ActionResult Edit(int id)
        {
            IndexLoopEntity entity = _service.Get(id);
            if (entity == null)
            {
                return InvokeHttp404();
            }
            return View(entity);
        }

        private ActionResult Edit(IndexLoopEntity model)
        {
            JsonResponse jsonResult = new JsonResponse();
            try
            {
                if (string.IsNullOrEmpty(model.Img))
                {
                    jsonResult.Success = false;
                    jsonResult.Message = "请上传图片";
                }
                else if (string.IsNullOrEmpty(model.Link))
                {
                    jsonResult.Success = false;
                    jsonResult.Message = "跳转地址不能为空";
                }
                else
                {
                    IndexLoopEntity db = _service.Get(model.Id);
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
            }
            catch (Exception e)
            {
                jsonResult = ErrorResponse(e.Message);
            }
            return jsonResult.ToJsonResult();
        }

        [HttpPost]
        [PermissionFilter("/Loop/Modify")]
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
        [PermissionFilter("/Loop/Modify")]
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