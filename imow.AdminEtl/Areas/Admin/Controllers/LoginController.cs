using System.Web;
using System.Web.Mvc;
using imow.Core.cookieModel;
using imow.Framework.Cookie;
using imow.Framework.Strategy.Controller;
using imow.Framework.Strategy.Filter;
using imow.Framework.Tool;
using imow.Services.BussinessService.Admin;
using Imow.Framework.Engine;

namespace imow.AdminEtl.Areas.Admin.Controllers
{
   
    public class LoginController : BaseController
    {

        private readonly AdminUserService _adminService;

        public LoginController()
        {
        }

        public LoginController(AdminUserService adminService)
        {
            _adminService = adminService;
        }
        [NoPerm]
        [NoLogin]
        public ActionResult Index()
        {
            CookieManager<AdminNameCookie> cookieManager =
                  ImowEngineContext.Current.Resolve<CookieManager<AdminNameCookie>>();
            HttpCookie cookie = cookieManager.Get();
            var name = cookie==null ? "":cookie.Value;
            ViewBag.remeber = name;
            return View();
        }
        #region 登陆
        /// <summary>
        /// 登陆页面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [NoPerm]
        [NoLogin]
        public ActionResult Login(string name, string password,string remeber)
        {
            int code = 0;
            _adminService.Login(name, password, out code);
            CommJsonResult result = new CommJsonResult()
            {
                Code = code,
                Message = ""
            };
            if (code == 0)
            {
                CookieManager<AdminNameCookie> cookieManager =
                    ImowEngineContext.Current.Resolve<CookieManager<AdminNameCookie>>();
                if (string.IsNullOrEmpty(remeber))
                {
                    //清除记住帐户
                    cookieManager.Remove();
                }
                else
                {
                    //记住帐户
                    cookieManager.SetCookie(name);
                }
            }


            return result.ToJsonResult();
        }
        #endregion

        #region 登出
        [HttpGet]
        [NoPerm]
        public ActionResult Logout()
        {
            Session.Clear();
            _adminService.Logout();
            return View("Index");
        }
        #endregion

    }
}