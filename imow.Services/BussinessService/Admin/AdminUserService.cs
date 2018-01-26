using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using DapperExtensions;
using imow.Core;
using imow.Core.cookieModel;
using imow.Framework.Cookie;
using imow.IRepository;
using imow.IRepository.Admin;
using imow.Model.EntityModel.Admin;
using Imow.Framework.Db;
using Imow.Framework.Engine;
using Imow.Framework.Tool;
using imow.Model.EntityModel;
using Imow.Framework.Cache.DistributedCache.Storage;

namespace imow.Services.BussinessService.Admin
{
    public class AdminUserService : IBaseService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IBaseRepository<AdminUserRoleEntity> _userRoleDao;
        private readonly IPermissionRepository _moduleDao;

        public AdminUserService(IAdminRepository adminRepository, IPermissionRepository moduleDao, IBaseRepository<AdminUserRoleEntity> userRoleDao)
        {
            _adminRepository = adminRepository;
            _userRoleDao = userRoleDao;
            _moduleDao = moduleDao;
        }

        #region 用户操作

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
                if (entity.IsDel || entity.IsStop)
                {
                    code = 4;
                }
                else
                {
                    //更新登录信息
                    entity.LastLoginTime = DateTime.Now;
                    _adminRepository.Update(entity);
                    //存放cookie
                    ImowEngineContext.Current.Resolve<CookieManager<AdminTicketCookie>>().Remove();
                    ImowEngineContext.Current.Resolve<AdminTicketService>().Add(entity);
                    //登录成功,清除缓存
                    string[] keys ={
                        string.Format(CacheKey.AdminUserModuleKey, entity.Id),
                        CacheKey.AllModuleKey
                    };
                    ImowEngineContext.Current.Resolve<CacheFactory>().Delete(keys);

                    //加载权限

                }
            }
            return entity;
        }

        public void Logout()
        {
            ImowEngineContext.Current.Resolve<CookieManager<AdminTicketCookie>>().Remove();
            FormsAuthentication.SignOut();
        }
        public List<AdminUserEntity> GetUserList(int pageSize, int pageIndex, string sort, string sortType, List<SearchModel> searchModels, out long count)
        {
            return _adminRepository.GetListByPage<AdminUserEntity>(pageSize, pageIndex, sort, sortType, searchModels, out count).ToList();
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
                user.RoleList = _moduleDao.GetUserRoleList(user.Id).ToList();
            }
            return user;
        }

        public AdminUserEntity GetAdminByUserName(string userName)
        {
            var list = _adminRepository.GetList(Predicates.Field<AdminUserEntity>(f => f.UserName, Operator.Eq, userName));
            AdminUserEntity user = list.FirstOrDefault();
            if (user != null)
            {
                user.RoleList = _moduleDao.GetUserRoleList(user.Id).ToList();
            }
            return user;
        }

        public void Update(AdminUserEntity user)
        {
            _adminRepository.Update(user);
        }

        public void UpdateUser(AdminUserEntity entity)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                _adminRepository.Update(entity);
                //删除原来权限
                string[] roleIdArr = entity.RoleId.Split(',');
                _userRoleDao.DeleteList(Predicates.Field<AdminUserRoleEntity>(f => f.AdminId, Operator.Eq, entity.Id));
                //插入新权限
                foreach (var s in roleIdArr)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        AdminUserRoleEntity roleEntity = new AdminUserRoleEntity
                        {
                            AdminId = entity.Id,
                            RoleId = long.Parse(s)
                        };
                        _userRoleDao.Add(roleEntity);
                    }
                }
                scope.Complete();
            }
        }

        public void AddUser(AdminUserEntity entity)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                _adminRepository.Add(entity);
                string[] roleIdArr = entity.RoleId.Split(',');
                //插入新权限
                foreach (var s in roleIdArr)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        AdminUserRoleEntity roleEntity = new AdminUserRoleEntity
                        {
                            AdminId = entity.Id,
                            RoleId = long.Parse(s)
                        };
                        _userRoleDao.Add(roleEntity);
                    }
                }
                scope.Complete();
            }
        }

        public void Active(long[] ids)
        {
            _adminRepository.ActiveUser(ids);
        }

        public void UnActive(long[] ids)
        {
            _adminRepository.UnActiveUser(ids);
        }

        public void LogicDelete(long[] ids)
        {
            _adminRepository.LogicDelete(ids);
        }

        public void Restore(long[] ids)
        {
            _adminRepository.Restore(ids);
        }

        #endregion

    }
}
