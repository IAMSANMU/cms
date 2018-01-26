using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Web;
using System.Web.Mvc;
using imow.Framework.Interface;
using Imow.Framework.Engine;
using System.Web.Routing;
using imow.Framework.Strategy.Filter;
using imow.Framework.Tool;
using imow.Services.BussinessService.Admin;

namespace imow.AdminEtl
{
    /// <summary>
    /// 全局过滤器
    /// </summary>
    public class PermissionFilter : FilterAttribute, IActionFilter
    {

        private readonly string _url;

        public PermissionFilter(string url = null)
        {
            _url = url;
        }

        /// <summary>
        /// 	在action执行前执行
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //只过滤Areas 是Admin的

            string areaName = (string)filterContext.RouteData.DataTokens["area"];
            bool isCheckAdmin = "Admin".Equals(areaName); //总后台管理

            if (isCheckAdmin)
            {
                bool ispostRequest = String.Equals(filterContext.HttpContext.Request.HttpMethod, "POST",
                    StringComparison.OrdinalIgnoreCase);

                var list = filterContext.ActionDescriptor.GetFilterAttributes(true).ToList();
                var workContext = ImowEngineContext.Current.Resolve<IWorkContext>();
                //判断是否有NoLogin,NoPermission 没有则判断登录和授权
                var noLogin = list.OfType<NoLoginAttribute>().ToList();
                bool flag = true;
                if (!noLogin.Any())
                {
                    //判断是否站点登录 还是 总后台登录
                    bool islogin = workContext.IsAdminLogined;
                    if (!islogin)
                    {
                        if (!ispostRequest)
                        {
                            filterContext.Result = new RedirectToRouteResult("Admin_default",
                                new RouteValueDictionary(new { controller = "Login", action = "Index" }));
                        }
                        else
                        {
                            filterContext.Result = new JsonResponse() { Success = false, Message = "未登录" }.ToJsonResult();
                        }
                        flag = false;
                    }
                }

                var noPrem = list.OfType<NoPermAttribute>().ToList();
                if (!noPrem.Any() && flag)
                {//判断权限
                    var adminUser = workContext.AdminEntity;
                    PermissionService permService = ImowEngineContext.Current.Resolve<PermissionService>();
                    var moduleList = permService.GetModuleByUser(adminUser).ToList();

                    //实际验证权限URL
                    string rawUrl = "/" + filterContext.RouteData.Values["controller"];
                    rawUrl = string.Concat(rawUrl, "/" + filterContext.RouteData.Values["action"]).ToLower();
                    string checkUrl = string.IsNullOrEmpty(_url) ? rawUrl : _url;

                    var module = moduleList.FirstOrDefault(f => f.Url?.ToLower() == checkUrl.ToLower());
                    if (module == null)
                    {
                        if (!ispostRequest)
                        {
                            filterContext.Result = new RedirectResult("/NoPermission.html");
                        }
                        else
                        {
                            filterContext.Result = new JsonResponse() { Success = false, Message = "访问没有权限" }.ToJsonResult();
                        }
                    }
                    else
                    {
                        workContext.ModuleList = moduleList;
                    }
                }
            }







        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}