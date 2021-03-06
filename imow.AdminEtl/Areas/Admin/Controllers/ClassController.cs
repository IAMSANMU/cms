﻿using System;
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
using ClassModel = imow.AdminEtl.Areas.Admin.Models.Admin.ClassModel;

namespace imow.AdminEtl.Areas.Admin.Controllers
{
    public class ClassController : BaseController
    {
        private readonly ClassService _service;
        private readonly SchoolService _schoolService;
        public ClassController(SchoolService schoolService, ClassService service)
        {
            _schoolService = schoolService;
            _service = service;
        }
        public ActionResult Index(string tab)
        {
            tab = string.IsNullOrEmpty(tab) ? "list" : tab;
            ViewBag.tab = tab;
            IEnumerable<SchoolEntity> schoolList = _schoolService.GetAll();
            ClassModel model = new ClassModel();
            model.SchoolList = schoolList;
            return View(model);
        }

        [HttpPost]
        [PermissionFilter("/Class/Index")]
        public ActionResult List(ListSearchModel searchModel)
        {
            long count = 0L;
            var list = _service.GetListByPage(searchModel.PageSize, searchModel.PageIndex, searchModel.Order, searchModel.OrderType, searchModel.SearchModels, out count);
            DatatablesJson<ClassEntity> json = new DatatablesJson<ClassEntity>
            {
                Data = list,
                RecordsFiltered = count,
                RecordsTotal = count
            };
            return Content(json.ToJsonString());
        }

        [HttpGet]
        [PermissionFilter("/Class/Modify")]
        public ActionResult Add()
        {
            //获取校区列表
            IEnumerable<SchoolEntity>  schoolList=_schoolService.GetAll();
            ClassModel model = new ClassModel();
            model.SchoolList = schoolList;
            model.Entity=new ClassEntity();
            return View("Edit", model);
        }

        private ActionResult Add(ClassEntity model)
        {
            JsonResponse jsonResult = new JsonResponse();
            try
            {
                if (string.IsNullOrEmpty(model.Name))
                {
                    jsonResult.Success = false;
                    jsonResult.Message = "课程名称不能为空";
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
        public ActionResult Modify(ClassEntity model)
        {
            if (model.Id == 0)
            {
                return Add(model);
            }
            return Edit(model);
        }

        [HttpGet]
        [PermissionFilter("/Class/Modify")]
        public ActionResult Edit(int id)
        {
            ClassEntity entity = _service.Get(id);
            //获取校区列表
           
            if (entity == null)
            {
                return InvokeHttp404();
            }
            IEnumerable<SchoolEntity> schoolList = _schoolService.GetAll();
            ClassModel model = new ClassModel();
            model.SchoolList = schoolList;
            model.Entity = entity;
            return View(model);
        }

        private ActionResult Edit(ClassEntity model)
        {
            JsonResponse jsonResult = new JsonResponse();
            try
            {
                if (string.IsNullOrEmpty(model.Name))
                {
                    jsonResult.Success = false;
                    jsonResult.Message = "课程名称不能为空";
                }
                else
                {
                    ClassEntity db = _service.Get(model.Id);
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
        [PermissionFilter("/Class/Modify")]
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
        [PermissionFilter("/Class/Modify")]
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