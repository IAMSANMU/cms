using System.Web.Mvc;

namespace imow.AdminEtl
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new PermissionFilter());
        }
    }
}
