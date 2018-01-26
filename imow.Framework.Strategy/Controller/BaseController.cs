using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using imow.Framework.Interface;
using imow.Framework.Tool;
using imow.Model.EntityModel.Admin;
using Imow.Framework.Engine;
using Imow.Framework.Log;

namespace imow.Framework.Strategy.Controller
{
    public class BaseController : System.Web.Mvc.Controller
    {
        public IWorkContext WorkContext
        {
            get { return ImowEngineContext.Current.Resolve<IWorkContext>(); }
        }

        public virtual string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }
        /// <summary>
        /// 渲染部分试图获得html
        /// </summary>
        /// <returns></returns>
        public virtual string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }
        /// <summary>
        /// 渲染部分试图获得html
        /// </summary>
        /// <returns></returns>
        public virtual string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }
        /// <summary>
        /// 渲染部分试图获得html
        /// </summary>
        /// <returns></returns>
        public virtual string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = this.ControllerContext.RouteData.GetRequiredString("action");

            this.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// 获得验证错误的msg
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public string GetErrorMsg()
        {
            var list = base.ModelState.Keys.Select(k => ModelState[k].Errors).ToList().FindAll(x => x.Count > 0);
            List<string> errorMsg = new List<string>();
            foreach (var modelErrorCollection in list)
            {
                foreach (var modelError in modelErrorCollection)
                {
                    errorMsg.Add(modelError.ErrorMessage);
                }
            }
            return string.Join(",", errorMsg);
        }

        /// <summary>
        /// 404
        /// </summary>
        /// <returns></returns>
        protected virtual ActionResult InvokeHttp404()
        {
            //todo:跳转到404页面
            return new EmptyResult();
        }



        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="exc"></param>
        protected void LogException(Exception exc)
        {
            var workContext = ImowEngineContext.Current.Resolve<IWorkContext>();
            var httpContextBase = ImowEngineContext.Current.Resolve<HttpContextBase>();
            ErrorLog.Write(new ErrorLogInfo()
            {
                logtag = httpContextBase.Request.Url.AbsoluteUri,
                msg = string.Format("用户id{0}", workContext.AdminEntity.Id)
            }, exc);
        }
        /// <summary>
        /// 获取当前登录账户,可能为缓存.不可直接更新
        /// </summary>
        /// <returns></returns>
        protected AdminUserEntity GetAdminUser()
        {
            return WorkContext.AdminEntity;
        }
        /// <summary>
        /// 已登录站点
        /// </summary>
        /// <returns></returns>


        protected JsonResponse ErrorResponse(string message)
        {
            return new JsonResponse() { Success = false, Message = "系统错误:" + message };
        }

        protected string RequestPath()
        {
            var routeData=RouteData.Route.GetRouteData(HttpContext);
            string path="/"+ routeData.Values["controller"] + "/" + routeData.Values["action"];
            return path;
        }
    }
}