﻿using System;
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
    public class SchoolController : BaseController
    {
        private readonly SchoolService _service;
        public SchoolController(SchoolService schoolService)
        {
            _service = schoolService;
        }
        public ActionResult Index(string tab)
        {
            tab = string.IsNullOrEmpty(tab) ? "list" : tab;
            ViewBag.tab = tab;
            return View();
        }

        [HttpPost]
        [PermissionFilter("/School/Index")]
        public ActionResult List(ListSearchModel searchModel)
        {
            long count = 0L;
            var list = _service.GetListByPage(searchModel.PageSize, searchModel.PageIndex, searchModel.Order, searchModel.OrderType, searchModel.SearchModels, out count);
            DatatablesJson<SchoolEntity> json = new DatatablesJson<SchoolEntity>
            {
                Data = list,
                RecordsFiltered = count,
                RecordsTotal = count
            };
            return Content(json.ToJsonString());
        }

        [HttpGet]
        [PermissionFilter("/School/Modify")]
        public ActionResult Add()
        {
            SchoolEntity model=new SchoolEntity();
            return View("Edit",model);
        }

        private ActionResult Add(SchoolEntity model)
        {
            JsonResponse jsonResult=new JsonResponse();
            try
            {
                if (string.IsNullOrEmpty(model.Name))
                {
                    jsonResult.Success = false;
                    jsonResult.Message="校区名称不能为空";
                }
                else
                {
                    model.CreateTime = DateTime.Now;
                    model.UpdateTime = model.CreateTime;
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
        public ActionResult Modify(SchoolEntity model)
        {
            if (model.Id == 0)
            {
                return Add(model);
            }
            return Edit(model);
        }

        [HttpGet]
        [PermissionFilter("/School/Modify")]
        public ActionResult Edit(int id)
        {
            SchoolEntity entity= _service.Get(id);
            if (entity == null)
            {
                return InvokeHttp404();
            }
            return View(entity);
        }

        private ActionResult Edit(SchoolEntity model)
        {
            JsonResponse jsonResult = new JsonResponse();
            try
            {
                if (string.IsNullOrEmpty(model.Name))
                {
                    jsonResult.Success = false;
                    jsonResult.Message = "校区名称不能为空";
                }
                else
                {
                    SchoolEntity db=_service.Get(model.Id);
                    if (db == null)
                    {
                        jsonResult.Success = false;
                        jsonResult.Message = "数据不存在";
                    }
                    else
                    {
                        model.UpdateTime =DateTime.Now;
                        MapperHelper.Copy(model,db,ignore:new [] {"createtime","id","isdel", "decodeinfo" });
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
        [PermissionFilter("/School/Modify")]
        public ActionResult Delete(string ids)
        {
            JsonResponse jsonResult=new JsonResponse();
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
        [PermissionFilter("/School/Modify")]
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