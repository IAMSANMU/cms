using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using imow.AdminEtl.Areas.Admin.Models.Admin;
using imow.Framework.Strategy.Controller;
using imow.Framework.Tool;
using imow.Model.EntityModel.Admin;
using imow.Services.BussinessService.Admin;
using Imow.Framework.Tool;

namespace imow.AdminEtl.Areas.Admin.Controllers
{
    public class PhotoController : BaseController
    {
        private readonly PhotoService _service;

        public PhotoController(PhotoService service)
        {
            _service = service;
        }
        public ActionResult Index(string tab,bool? isDel)
        {
            tab = string.IsNullOrEmpty(tab) ? "list" : tab;
            ViewBag.tab = tab;
            isDel = isDel ?? false;

            IEnumerable<PhotoEntity> list= _service.GetAll(isDel.Value);
            PhotoModel model=new PhotoModel();
            model.PhotoList = list;
            return View(model);
        }

        [HttpPost]
        [PermissionFilter("/Photo/Modify")]
        public ActionResult Modify(PhotoEntity model)
        {
            if (model.Id == 0)
            {
                return Add(model);
            }
            return Edit(model);
        }

        [HttpPost]
        [PermissionFilter("/Photo/Modify")]
        public ActionResult Delete(int id)
        {
            JsonResponse json = new JsonResponse();
            try
            {
                _service.Delete(id);
                json.Success = true;
            }
            catch (Exception e)
            {
                json = ErrorResponse(e.Message);
            }
            return json.ToJsonResult();
        }
        [HttpPost]
        [PermissionFilter("/Photo/Modify")]
        public ActionResult Restore(int id)
        {
            JsonResponse json = new JsonResponse();
            try
            {
                _service.Restore(id);
                json.Success = true;
            }
            catch (Exception e)
            {
                json = ErrorResponse(e.Message);
            }
            return json.ToJsonResult();
        }
        [HttpGet]
        [PermissionFilter("/Photo/Modify")]
        public ActionResult Add()
        {
            PhotoModel model = new PhotoModel {PhotoEntity = new PhotoEntity()};
            return View("Edit",model);
        }
        private ActionResult Add(PhotoEntity model)
        {
            JsonResponse json=new JsonResponse();
            try
            {
                model.CreateTime = DateTime.Now;
                _service.Add(model);
                json.Success = true;
            }
            catch (Exception e)
            {
                json = ErrorResponse(e.Message);
            }

            return json.ToJsonResult();
        }

        [HttpGet]
        [PermissionFilter("/Photo/Modify")]
        public ActionResult Edit(int id)
        {
            PhotoEntity entity=_service.Get(id);
            if (entity == null)
            {
                return InvokeHttp404();
            }
            PhotoModel model = new PhotoModel {PhotoEntity = entity};
            return View(model);
        }

        private ActionResult Edit(PhotoEntity model)
        {
            JsonResponse json = new JsonResponse();
            try
            {
                PhotoEntity db = _service.Get(model.Id);
                if (db != null)
                {
                    MapperHelper.Copy(model, db, ignore: new[] {"isdel", "createtime", "img"});
                    _service.Update(db);
                    json.Success = true;
                }
            }
            catch (Exception e)
            {
                json = ErrorResponse(e.Message);
            }
            return json.ToJsonResult();
        }



        [HttpGet]
        [PermissionFilter("/Photo/Modify")]
        public ActionResult ImgList(int pid)
        {
            PhotoEntity entity=_service.Get(pid);
            if (entity == null)
            {
                return InvokeHttp404();
            }

            IEnumerable<PhotoImgEntity> list=_service.GetImgAll(pid);
            PhotoModel model = new PhotoModel {ImgList = list,PhotoEntity = entity };
            return View(model);
        }
        [HttpGet]
        [PermissionFilter("/Photo/Modify")]
        public ActionResult AddImg(int id)
        {
            ViewBag.PhotoId = id;
            return View();
        }
        [HttpPost]
        [PermissionFilter("/Photo/Modify")]
        public ActionResult AddImg(PhotoImgEntity model)
        {
            JsonResponse json = new JsonResponse();
            try
            {
                model.IsDel = false;
                model.CreateTime = DateTime.Now;
                _service.AddImg(model);
                json.Success = true;
            }
            catch (Exception e)
            {
                json = ErrorResponse(e.Message);
            }
            return json.ToJsonResult();
        }

        [HttpGet]
        [PermissionFilter("/Photo/Modify")]
        public ActionResult EditImg(int id)
        {
            PhotoImgEntity entity = _service.GetImg(id);
            if (entity == null)
            {
                return InvokeHttp404();
            }
            PhotoModel model = new PhotoModel { PhotoImgEntity = entity };
            return View(model);
        }
        [HttpPost]
        [PermissionFilter("/Photo/Modify")]
        public ActionResult EditImg(PhotoImgEntity model)
        {
            JsonResponse json = new JsonResponse();
            try
            {
                PhotoImgEntity db = _service.GetImg(model.Id);
                if (db != null)
                {
                    MapperHelper.Copy(model, db, ignore: new[] { "isdel", "createtime", "photoid","url" });
                    _service.UpdateImg(db);
                    json.Success = true;
                }
            }
            catch (Exception e)
            {
                json = ErrorResponse(e.Message);
            }
            return json.ToJsonResult();
        }

        [HttpPost]
        [PermissionFilter("/Photo/Modify")]
        public ActionResult DeleteImg(int id)
        {
            JsonResponse json = new JsonResponse();
            try
            {
                _service.DeleteImg(id);
                json.Success = true;
            }
            catch (Exception e)
            {
                json = ErrorResponse(e.Message);
            }
            return json.ToJsonResult();
        }

    }
}