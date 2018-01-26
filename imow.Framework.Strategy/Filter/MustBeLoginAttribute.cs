using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using imow.Framework.Interface;
using imow.Framework.Tool;
using Imow.Framework.Engine;

namespace imow.Framework.Strategy.Filter
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class MustBeLoginAttribute : FilterAttribute,IActionFilter
    {
        /// <summary>
        /// 	在action执行前执行
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

            bool ispostRequest = String.Equals(filterContext.HttpContext.Request.HttpMethod, "POST",
                StringComparison.OrdinalIgnoreCase);
            //不是登录信息,判断是否登录,未登录跳转登录页
            var controller = filterContext.RouteData.Values["controller"].ToString();
            if (controller == "Login") return;

            IWorkContext workContext = ImowEngineContext.Current.Resolve<IWorkContext>();
            bool islogin = workContext.IsAdminLogined;

            //如果没有登陆跳转登录页
            if (islogin) return;

            if (!ispostRequest)
            {
                filterContext.Result = new RedirectToRouteResult("Default",
                    new RouteValueDictionary(new { controller = "Login", action = "Index" }));
            }
            else
            {
                filterContext.Result = new JsonResponse() {Success = false, Message = "未登录" }.ToJsonResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}
