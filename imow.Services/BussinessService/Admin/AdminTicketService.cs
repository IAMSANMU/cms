using System.Web.Security;
using imow.Core.cookieModel;
using imow.Framework.Cookie;
using imow.Model.EntityModel.Admin;

namespace imow.Services.BussinessService.Admin
{
    public class AdminTicketService : BaseTicketService<AdminTicketCookie>,IBaseService
    {
        public AdminTicketService(CookieManager<AdminTicketCookie> ticketCookie) : base(ticketCookie)
        {
        }
        public void Add(AdminUserEntity admin)
        {
            base.Add(admin.UserName,admin.Id.ToString(), (int)FormsAuthentication.Timeout.TotalMinutes);
        }

    }

}
