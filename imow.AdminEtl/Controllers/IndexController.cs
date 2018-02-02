using System;
using System.Collections;
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
    public class IndexController : BaseController
    {
        private readonly LoopService _loopService;
        private readonly ClassService _classService;
        private readonly StaffService _staffService;
        private readonly PhotoService _photoService;

        public IndexController(LoopService loopService, ClassService classService, StaffService staffService, PhotoService photoService)
        {
            _loopService = loopService;
            _classService = classService;
            _staffService = staffService;
            _photoService = photoService;
        }

        public ActionResult Index()
        {
            IEnumerable<IndexLoopEntity> loopList = _loopService.GetAll();
            IEnumerable<ClassEntity> classList = _classService.GetAll();
            IEnumerable<StaffEntity> staffList= _staffService.GetCommond();
            IEnumerable<PhotoImgEntity> imgList= _photoService.GetIndexImg();

            IndexModel model = new IndexModel
            {
                LoopList = loopList.ToList(),
                ClassList = classList.Take(4).ToList(),
                StaffList = staffList.Take(4).ToList(),
                ImgList = imgList.ToList()
            };
            return View(model);
        }
        [ChildActionOnly]
        public ActionResult Order()
        {

            return View();
        }

    }
}