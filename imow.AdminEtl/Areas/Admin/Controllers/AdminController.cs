using System;
using System.Linq;
using System.Web.Mvc;
using EC.Common.Extensions;
using imow.AdminEtl.Areas.Admin.Models.Admin;
using imow.AdminEtl.Models;
using imow.Framework.Strategy.Controller;
using imow.Framework.Tool;
using imow.Model.EntityModel.Admin;
using imow.Services.BussinessService.Admin;
using Imow.Framework.Tool;

namespace imow.AdminEtl.Areas.Admin.Controllers
{
    public class AdminController : BaseController
    {
        private readonly AdminUserService _adminUserService;
        private readonly PermissionService _permService;

        public AdminController(AdminUserService adminUserService,  PermissionService permService)
        {
            _adminUserService = adminUserService;
            _permService = permService;
        }
        #region 后台账户操作

        [HttpGet]
        [PermissionFilter("/Admin/Index")]
        public ActionResult Index(string tab)
        {
            tab = string.IsNullOrEmpty(tab) ? "list" : tab;
            ViewBag.tab = tab;
            return View();
        }

        [HttpPost]
        [PermissionFilter("/Admin/Index")]
        public ActionResult GetUserList(ListSearchModel searchModel)
        {
            long count = 0L;
            var list = _adminUserService.GetUserList(searchModel.PageSize, searchModel.PageIndex, searchModel.Order, searchModel.OrderType, searchModel.SearchModels, out count);
            DatatablesJson<AdminUserEntity> json = new DatatablesJson<AdminUserEntity>
            {
                Data = list,
                RecordsFiltered = count,
                RecordsTotal = count
            };
            return Content(json.ToJsonString());
        }
        [PermissionFilter("/Admin/Modify")]
        public ActionResult Edit(string id)
        {
            AdminUserEntity entity = null;
            if (!string.IsNullOrEmpty(id))
            {
                entity = _adminUserService.GetAdminById(id);
            }
            //获取权限模块
            var roleList = _permService.GetRoleList(false).ToList();
            AdminUserModel model = new AdminUserModel
            {
                RoleList = roleList,
                UserEntity = entity
            };
            return View(model);
        }
        [PermissionFilter("/Admin/Modify")]
        public ActionResult Add()
        {
            var roleList = _permService.GetRoleList(false).ToList();
            AdminUserModel model = new AdminUserModel
            {
                RoleList = roleList,
                UserEntity = new AdminUserEntity()
            };
            return View(model);
        }
        [PermissionFilter("/Admin/Modify")]
        public ActionResult UpdateUser(SaveUserModel model)
        {
            JsonResponse jsonResult = new JsonResponse();
            if (string.IsNullOrEmpty(model.Roles))
            {
                jsonResult.Success = false;
                jsonResult.Message = "请分配权限";
            }
            else
            {
                AdminUserEntity entity = _adminUserService.GetAdminById(model.Id.ToString());
                if (entity == null)
                {
                    jsonResult.Success = false;
                    jsonResult.Message = "数据错误,用户不存在";
                }
                else
                {
                    try
                    {
                        entity.RealName = model.RealName;
                        entity.Email = model.Email;
                        entity.Mobile = model.Mobile;
                        entity.IsStop = model.IsStop == "on";
                        entity.RoleId = model.Roles;
                        entity.Sex = model.Sex;
                        entity.Remark = model.Remark;
                        entity.UpdateTime = DateTime.Now;
                        entity.QQ = model.QQ;
                        _adminUserService.UpdateUser(entity);
                        jsonResult.Success = true;
                    }
                    catch (Exception e)
                    {
                        jsonResult = ErrorResponse(e.Message);
                    }
                }
            }
            return jsonResult.ToJsonResult();
        }
        [HttpPost]
        [PermissionFilter("/Admin/Modify")]
        public ActionResult AddUser(SaveUserModel model)
        {
            JsonResponse jsonResult = new JsonResponse();
            if (string.IsNullOrEmpty(model.Roles))
            {
                jsonResult.Success = false;
                jsonResult.Message = "请分配权限";
            }
            else if (string.IsNullOrEmpty(model.UserName))
            {
                jsonResult.Success = false;
                jsonResult.Message = "登录帐号不能为空";
            }
            else if (string.IsNullOrEmpty(model.Pwd) || string.IsNullOrEmpty(model.PwdSure) || model.Pwd != model.PwdSure)
            {
                jsonResult.Success = false;
                jsonResult.Message = "密码不一致";
            }
            else
            {
                AdminUserEntity entity = _adminUserService.GetAdminByUserName(model.UserName);
                if (entity != null)
                {
                    jsonResult.Success = false;
                    jsonResult.Message = "账号已存在";
                }
                else
                {
                    try
                    {
                        entity = new AdminUserEntity
                        {
                            Id = ID.GetNextId(),
                            RealName = model.RealName,
                            Email = model.Email,
                            Mobile = model.Mobile,
                            IsStop = model.IsStop == "on",
                            RoleId = model.Roles,
                            UserName = model.UserName,
                            CreateTime = DateTime.Now,
                            UpdateTime = DateTime.Now,
                            Pwd = EncryptionHelper.GetMD5AndSHA256(model.Pwd.Trim()),
                            Sex = model.Sex,
                            Remark = model.Remark
                        };
                        _adminUserService.AddUser(entity);
                        jsonResult.Success = true;
                    }
                    catch (Exception e)
                    {
                        jsonResult = ErrorResponse(e.Message);
                    }
                }
            }
            return jsonResult.ToJsonResult();

        }
        [HttpPost]
        [PermissionFilter("/Admin/Modify")]
        public ActionResult Active(string ids)
        {
            return CRUD(ids, "active");
        }
        [HttpPost]
        [PermissionFilter("/Admin/Modify")]
        public ActionResult UnActive(string ids)
        {
            return CRUD(ids, "unActive");
        }
        [HttpPost]
        [PermissionFilter("/Admin/Modify")]
        public ActionResult Delete(string ids)
        {
            return CRUD(ids, "delete");
        }
        [HttpPost]
        [PermissionFilter("/Admin/Modify")]
        public ActionResult Restore(string ids)
        {
            return CRUD(ids, "restore");
        }
        private ActionResult CRUD(string ids, string type)
        {
            JsonResponse jsonResponse = new JsonResponse();
            if (string.IsNullOrEmpty(ids))
            {
                jsonResponse.Success = false;
                jsonResponse.Message = "请至少选择一条记录!";
            }
            else
            {
                try
                {
                    if (type.Equals("active"))
                    {
                        _adminUserService.Active(ids.SplitRemoveEmptyToInt64(','));
                    }
                    else if (type.Equals("unActive"))
                    {
                        _adminUserService.UnActive(ids.SplitRemoveEmptyToInt64(','));
                    }
                    else if (type.Equals("delete"))
                    {
                        _adminUserService.LogicDelete(ids.SplitRemoveEmptyToInt64(','));
                    }
                    else if (type.Equals("restore"))
                    {
                        _adminUserService.Restore(ids.SplitRemoveEmptyToInt64(','));
                    }
                    jsonResponse.Success = true;
                }
                catch (Exception e)
                {

                    jsonResponse = ErrorResponse(e.Message);
                }
            }
            return jsonResponse.ToJsonResult();
        }


        [HttpGet]
        [PermissionFilter("/Admin/Modify")]
        public ActionResult ModifyPwd(string id)
        {
            var user = _adminUserService.GetAdminById(id);
            if (user == null)
            {
                return InvokeHttp404();
            }
            AdminUserModel model = new AdminUserModel
            {
                UserEntity = user
            };
            return View(model);
        }

        [HttpPost]
        [PermissionFilter("/Admin/Modify")]
        public ActionResult ModifyPwd(string id, string pwd, string pwdSure)
        {
            JsonResponse jsonResult = new JsonResponse();
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(pwdSure))
            {
                jsonResult.Success = false;
                jsonResult.Message = "数据错误,请刷新重试!";
            }
            else if (!pwd.Equals(pwdSure))
            {
                jsonResult.Success = false;
                jsonResult.Message = "两次密码不一致";
            }
            else
            {
                try
                {
                    var user = _adminUserService.GetAdminById(id);
                    if (user == null)
                    {
                        jsonResult.Success = false;
                        jsonResult.Message = "用户信息不存在!";
                    }
                    else
                    {
                        //修改密码
                        pwd = EncryptionHelper.GetMD5AndSHA256(pwd);
                        user.Pwd = pwd;
                        _adminUserService.Update(user);
                        jsonResult.Success = true;
                    }
                }
                catch (Exception e)
                {

                    jsonResult = ErrorResponse(e.Message);
                }

            }
            return jsonResult.ToJsonResult();
        }

        #endregion

    }
}