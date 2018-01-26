using System.Linq;
using System.Web.Mvc;
using imow.Framework.Interface;
using imow.Model.EntityModel;
using Imow.Framework.Engine;

namespace imow.Framework.Strategy.Extensions
{
    public static class HtmlHelpExtensions
    {

        public static bool CheckPopPerm(this HtmlHelper helper, string checkUrl)
        {
            bool flag = true;
            //if (!string.IsNullOrEmpty(checkUrl))
            //{
            //    var workContext = ImowEngineContext.Current.Resolve<IWorkContext>();
            //    var moduleList = workContext.PopModuleList;
            //    var module = moduleList.FirstOrDefault(f => f.Url?.ToLower() == checkUrl.ToLower());
            //    if (module == null)
            //    {
            //        flag = false;
            //    }
            //}
            return flag;
        }
    }
}