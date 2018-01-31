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

        public IndexController(LoopService loopService)
        {
            _loopService = loopService;
        }

        public ActionResult Index()
        {
            IEnumerable<IndexLoopEntity> loopList = _loopService.GetAll();
            IndexModel model = new IndexModel();
            model.LoopList = loopList.ToList();
            return View(model);
        }
    }
}