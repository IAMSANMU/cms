using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using imow.AdminEtl.Models;
using imow.Framework.Strategy.Controller;
using imow.Framework.Tool;
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
        private readonly SchoolService _schoolService;
        private readonly OrderService _orderService;
        public IndexController(OrderService orderService, SchoolService schoolService, LoopService loopService, ClassService classService, StaffService staffService, PhotoService photoService)
        {
            _loopService = loopService;
            _classService = classService;
            _staffService = staffService;
            _photoService = photoService;
            _schoolService = schoolService;
            _orderService = orderService;
        }

        public ActionResult Index()
        {
            IEnumerable<IndexLoopEntity> loopList = _loopService.GetAll();
            IEnumerable<ClassEntity> classList = _classService.GetAll();
            IEnumerable<StaffEntity> staffList = _staffService.GetCommond();
            IEnumerable<PhotoImgEntity> imgList = _photoService.GetIndexImg();

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
            //加载学校 和课程
            IEnumerable<SchoolEntity> schoolList = _schoolService.GetAll();
            IEnumerable<ClassEntity> classList = _classService.GetAll();

            OrderModel model = new OrderModel();
            model.ClassList = classList.ToList();
            model.SchoolList = schoolList.ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Order(OrderEntity model)
        {
            JsonResponse json = new JsonResponse();
            try
            {
                string msg;
                _orderService.Add(model, out msg);
                if (!string.IsNullOrEmpty(msg))
                {
                    json.Success = false;
                    json.Message = msg;
                }
                else
                {
                    json.Success = true;
                }
            }
            catch (Exception e)
            {
                json = ErrorResponse(e.Message);
            }

            return json.ToJsonResult();
        }

    }
}