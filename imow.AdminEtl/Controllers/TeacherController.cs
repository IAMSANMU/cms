using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using imow.AdminEtl.Models;
using imow.Model.EntityModel.Admin;
using imow.Services.BussinessService.Admin;

namespace imow.AdminEtl.Controllers
{
    public class TeacherController : Controller
    {
        private readonly StaffService _service;

        public TeacherController(StaffService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            IEnumerable<StaffEntity> list= _service.GetAll();
            TeacherModel model = new TeacherModel();
            model.ChinaList = list.Where(f => f.IsChina).ToList();
            model.OtherList = list.Where(f => !f.IsChina).ToList();
            return View(model);
        }
    }
}