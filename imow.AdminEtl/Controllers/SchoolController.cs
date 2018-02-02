using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using imow.AdminEtl.Models;
using imow.Framework.Strategy.Controller;
using imow.Model.EntityModel.Admin;
using imow.Services.BussinessService.Admin;

namespace imow.AdminEtl.Controllers
{
    public class SchoolController : BaseController
    {
        private readonly SchoolService _service;
        private readonly ClassService _classService;
        private readonly PhotoService _photoService;

        public SchoolController(SchoolService service, ClassService classService, PhotoService photoService)
        {
            _service = service;
            _classService = classService;
            _photoService = photoService;
        }

        public ActionResult Index()
        {
            var list = _service.GetAll();
            SchoolModel model = new SchoolModel();
            model.SchoolList = list.ToList();

            return View(model);
        }

        public ActionResult Info(int id)
        {
            SchoolEntity entity = _service.Get(id);
            if (entity == null || entity.IsDel)
            {
                return InvokeHttp404();
            }
            var photoList=_photoService.GetImgAll(entity.PhotoId);


            SchoolModel model = new SchoolModel();
            model.Entity = entity;
            model.ClassList = _classService.GetAll().ToList();
            model.PhotoList = photoList.ToList();
            return View(model);
        }
    }
}