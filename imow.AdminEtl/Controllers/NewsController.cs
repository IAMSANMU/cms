using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using imow.Framework.Strategy.Controller;

namespace imow.AdminEtl.Controllers
{
    public class NewsController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Info()
        {
            return View();
        }
    }
}