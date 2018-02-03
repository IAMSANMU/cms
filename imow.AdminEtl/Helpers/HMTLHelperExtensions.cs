using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using imow.Core.config;
using imow.Framework.Interface;
using Imow.Framework.Engine;

public static class HMTLHelperExtensions
{
    public static string IsSelected(this HtmlHelper html, string controller = null, string action = null, string cssClass = null)
    {

        if (String.IsNullOrEmpty(cssClass))
            cssClass = "active";

        string currentAction = (string)html.ViewContext.RouteData.Values["action"];
        string currentController = (string)html.ViewContext.RouteData.Values["controller"];

        if (String.IsNullOrEmpty(controller))
            controller = currentController;

        if (String.IsNullOrEmpty(action))
            action = currentAction;

        return controller == currentController && action == currentAction ?
            cssClass : String.Empty;
    }

    public static string PageClass(this HtmlHelper html)
    {
        string currentAction = (string)html.ViewContext.RouteData.Values["controller"];
        return currentAction.ToLower();
    }
    /// <summary>
    /// 按钮是否有权限
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static bool HasPERMS(this HtmlHelper html,string code)
    {
        string[] unCheck = new string[] { "refresh" };
        bool flag = false;
        if (!string.IsNullOrEmpty(code))
        {
            code = code.ToLower();
            if (unCheck.Contains(code))
            {
                flag = true;
            }
            else {
                var workContext = ImowEngineContext.Current.Resolve<IWorkContext>();

            }

          
        }
        return flag;
    }
}
