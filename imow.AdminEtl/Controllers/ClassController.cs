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
    public class ClassController : BaseController
    {
        // GET: Class

        private readonly ClassService _service;

        public ClassController(ClassService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            IEnumerable<ClassEntity>  list=_service.GetAll();
            ClassModel model=new ClassModel();
            model.ClassList = list.ToList();
            model.TopList = list.Take(4).ToList();

            return View(model);
        }

        public ActionResult Info(int id)
        {
            ClassEntity entity = _service.Get(id);
            ClassModel model = new ClassModel();
            model.Entity = entity;
            return View(model);
        }
    }
}