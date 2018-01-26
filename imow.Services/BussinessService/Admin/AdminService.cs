using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using DapperExtensions;
using imow.Core;
using imow.Core.cookieModel;
using imow.Framework.Cookie;
using imow.IRepository.Admin;
using imow.Model.EntityModel.Admin;
using Imow.Framework.Engine;
using Imow.Framework.Tool;

namespace imow.Services.BussinessService.Admin
{
    public class AdminService : IBaseService
    {
        private readonly IAdminRepository _adminRepository;
        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }


        public AdminUserEntity Login(string name, string pwd, out int code)
        {
            code = 0;
            AdminUserEntity entity = null;
            if (string.IsNullOrEmpty(name))
            {
                code = 1;
            }
            else if (string.IsNullOrEmpty(pwd))
            {
                code = 2;
            }
            pwd = EncryptionHelper.GetMD5AndSHA256(pwd);

            IList<IPredicate> predList = new List<IPredicate>();
            predList.Add(Predicates.Field<AdminUserEntity>(f => f.UserName, Operator.Eq, name));
            predList.Add(Predicates.Field<AdminUserEntity>(f => f.Pwd, Operator.Eq, pwd));
            var list = _adminRepository.GetList(Predicates.Group(GroupOperator.And, predList.ToArray())).ToList();
            if (!list.Any())
            {
                code = 3;
            }
            else
            {
                entity = list.First();
                if (entity.IsDel || entity.IsDisabled || entity.IsStop)
                {
                    code = 4;
                }
                else
                {
                    //更新登录信息
                    entity.LoginTimes++;
                    entity.LastLoginTime = DateTime.Now;
                    _adminRepository.Update(entity);
                    //存放cookie
                    ImowEngineContext.Current.Resolve<CookieManager<AdminTicketCookie>>().Remove();
                    ImowEngineContext.Current.Resolve<AdminTicketService>().Add(entity);
                }
            }
            return entity;
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        public AdminUserEntity GetCurrentUserEntity()
        {
            var adminTicketService = ImowEngineContext.Current.Resolve<AdminTicketService>();
            FormsAuthenticationTicket ticket = adminTicketService.GetTicket();
            if (ticket != null && !string.IsNullOrEmpty(ticket.Name))
            {
                return GetAdminById(ticket.UserData);
            }
            else
            {
                return null;
            }
        }

        public AdminUserEntity GetAdminById(string id)
        {
            AdminUserEntity user = _adminRepository.Get(id);
            if (user != null)
            {
                user.RoleList = _adminRepository.GetUserRoleList(user.ID).ToList();
            }
            return user;
        }


        public IEnumerable<AdminModuleEntity> GetModuleByUser(AdminUserEntity user)
        {
            //如果是管理员角色，则获取所有菜单
            IEnumerable<AdminModuleEntity> models = null;
            if (user.IsAdmin)
            {
                models = Gets().Where(i => i.IsGroup != user.IsSite);
            }
            else
            {
                models = _adminRepository.GetModuleByRoles(user.RoleID.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                     .Select(i => long.Parse(i)).ToArray());

            }
            if (models != null)
            {

                foreach (var item in models.Where(i => i.HasCode))
                {
                    item.ModuleCodeList = _adminRepository.GetModuleCodeList(item.ID).ToList();
                }
            }
            return models;
        }


        public IEnumerable<AdminModuleEntity> Gets()
        {
            var models = _adminRepository.GetAdminModuleList();
            return models;
        }

    }
}
