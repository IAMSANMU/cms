using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using imow.AdminEtl.Models;
using imow.Framework.Strategy.Controller;
using imow.Model.EntityModel.Admin;
using imow.Services.BussinessService.Admin;

namespace imow.AdminEtl.Controllers
{
    public class NewsController : BaseController
    {
        private readonly ContextService _service;

        public NewsController(ContextService service)
        {
            _service = service;
        }

        public ActionResult Index(int? s, int? p)
        {
            int pageIndex = p ?? 1;
            int pageSize = s ?? 10;
            long count;
            IEnumerable<ContextEntity> list = _service.GetListByPage(pageSize, pageIndex, out count);
            NewsModel model = new NewsModel();
            model.NewsList = list.ToList();

            Paging paging=new Paging
            {
                PageSize = pageSize,
                CurrentPage = pageIndex,
                TotalItemCount = (int)count,
                RouteData = RouteData,
                RouteValueDictionary=new RouteValueDictionary()
            };
            model.Page = paging;

            return View(model);
        }

        public ActionResult Info(int id)
        {
            ContextEntity entity = _service.Get(id);
            if (entity == null || !entity.IsShow || entity.IsDel)
            {
                return InvokeHttp404();
            }
            NewsModel model = new NewsModel();
            model.News = entity;
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult Top()
        {
            IEnumerable<ContextEntity> list = _service.GetTop(10);
            NewsModel model = new NewsModel();
            model.NewsList = list.ToList();
            return View(model);
        }
    }
}