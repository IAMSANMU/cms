using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using imow.AdminEtl.Areas.Admin.Models.Admin;
using imow.Framework.Strategy.Controller;
using imow.Framework.Strategy.Filter;
using imow.Model.EntityModel.Admin;
using imow.Services.BussinessService.Admin;
using Imow.Framework.Cache.DistributedCache.Storage;

namespace imow.AdminEtl.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly PermissionService _permService;
        private readonly CacheFactory _cache;
        public HomeController(AdminUserService adminService, CacheFactory cache, PermissionService permService)
        {
            _permService = permService;
            _cache = cache;
        }

        [NoPerm]
        public ActionResult Index()
        {
            //加载权限
            AdminUserEntity user = GetAdminUser();
            var moduleList = _permService.GetModuleByUser(user).ToList();
            HomeModel model = new HomeModel
            {
                ModuleList = moduleList,
            };
            return View(model);
        }
        [ChildActionOnly]
        [NoPerm]
        public ActionResult Menu()
        {
            AdminUserEntity user = GetAdminUser();
            var moduleList = _permService.GetModuleByUser(user).ToList();
            ViewContext parent= RouteData.DataTokens["ParentActionViewContext"] as ViewContext;
            string url = string.Concat("/", parent.RouteData.Values["controller"], "/" + parent.RouteData.Values["action"]);
            //判断URL 高亮菜单栏
            var lastLevel = moduleList.FirstOrDefault(f => f.Url == url);
            //需要选中的模块
            List<AdminModuleEntity> selectedList = new List<AdminModuleEntity>();

            if (lastLevel != null)
            {
                selectedList.Add(lastLevel);
                GetParent(moduleList,lastLevel,selectedList);
            }
            HomeModel model = new HomeModel
            {
                ModuleList = moduleList,
                SelectedList = selectedList
            };
            return View(model);
        }

        private void GetParent(List<AdminModuleEntity> all,AdminModuleEntity current, List<AdminModuleEntity> selectedModule)
        {
            var module=all.FirstOrDefault(f => f.Id == current.Pid);
            if (module != null)
            {
                selectedModule.Add(module);
                GetParent(all,module,selectedModule);
            }
        }

    }
}