using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using imow.AdminEtl.Models;
using imow.Framework.Strategy.Controller;
using imow.Framework.Strategy.Filter;
using imow.Framework.Tool;
using imow.Model.EntityModel.Admin;
using imow.Services.BussinessService.Admin;
using Newtonsoft.Json;

namespace imow.AdminEtl.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly PermissionService _permService;

        public RoleController(PermissionService permService)
        {
            _permService = permService;
        }

        public ActionResult Index(string tab)
        {
            tab = string.IsNullOrEmpty(tab) ? "list" : tab;
            ViewBag.tab = tab;
            return View();
        }

        [HttpPost]
        [PermissionFilter("/Role/Index")]
        public ActionResult List(ListSearchModel searchModel)
        {
            long count = 0L;
            var list = _permService.GetRoleList(searchModel.PageSize, searchModel.PageIndex, searchModel.Order, searchModel.OrderType, searchModel.SearchModels, out count);
            DatatablesJson<AdminRoleEntity> json = new DatatablesJson<AdminRoleEntity>
            {
                Data = list,
                RecordsFiltered = count,
                RecordsTotal = count
            };
            return Content(json.ToJsonString());
        }

        [NoPerm]
        public ActionResult GetTreeJson(long? roleId)
        {
            var adminUser = GetAdminUser();
            List<JsTreeJson> treeList = new List<JsTreeJson>();
            IEnumerable<AdminModuleEntity> all = _permService.GetsAllModule(adminUser);

            IEnumerable<AdminRoleModuleEntity> existModules = null;
            if (roleId.HasValue)
            {
                existModules = _permService.GetRoleModuleList(roleId.Value);
            }

            var topModules = all.Where(i => i.Depth == 0);
            foreach (var top in topModules)
            {
                JsTreeJson tree = new JsTreeJson
                {
                    Id = top.Id.ToString(),
                    Text = top.Name,
                    State = new JsTreeState()
            };

                treeList.Add(tree);
                if (!top.IsLast)
                {
                    tree.Children = new List<JsTreeJson>();
                    GetChildrenTreeJson(all, tree, existModules);
                }
                else
                {
                    if (existModules != null)
                    {
                        tree.State.Selected = existModules.Any(i => i.ModuleId == top.Id);
                    }
                }
            }
            return Content(JsonConvert.SerializeObject(treeList));
        }


        [NoPerm]
        public void GetChildrenTreeJson(IEnumerable<AdminModuleEntity> all, JsTreeJson parent, IEnumerable<AdminRoleModuleEntity> existModules)
        {
            var childrens = all.Where(i => i.Pid == int.Parse(parent.Id));
            foreach (var c in childrens)
            {
                var easy = new JsTreeJson
                {
                    Id = c.Id.ToString(),
                    Text = c.Name
                };
                parent.Children.Add(easy);
                //非最后节点继续添加
                if (!c.IsLast)
                {
                    easy.State = new JsTreeState();

                    if (easy.Children == null)
                    {
                        easy.Children = new List<JsTreeJson>();
                    }
                    GetChildrenTreeJson(all, easy, existModules);
                }
                else
                {
                    if (existModules != null )
                    {
                        easy.State = new JsTreeState {Selected = existModules.Any(i => i.ModuleId == c.Id)};
                    }
                }
            }
        }

        [HttpGet]
        [PermissionFilter("/Role/Modify")]
        public ActionResult Add()
        {
            return View();
        }
        [HttpGet]
        [PermissionFilter("/Role/Modify")]
        public ActionResult Edit(long id)
        {
            AdminRoleEntity entity = _permService.Get<AdminRoleEntity>(id.ToString());
            if (entity == null)
            {
                return InvokeHttp404();
            }
            return View(entity);
        }
        [HttpPost]
        [PermissionFilter("/Role/Modify")]
        public ActionResult Add(AdminRoleEntity model, string codes)
        {
            JsonResponse jsonResult = new JsonResponse();
            if (string.IsNullOrEmpty(codes))
            {
                jsonResult.Success = false;
                jsonResult.Message = "请分配权限";
            }
            else if (string.IsNullOrEmpty(model.Name))
            {
                jsonResult.Success = false;
                jsonResult.Message = "名称不能为空";
            }
            else
            {
                try
                {
                    //判断是否存在
                    if (_permService.CheckRole(model.Name,  null))
                    {
                        string[] addModules = codes.Trim().Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        _permService.AddRole(model, addModules);
                        jsonResult.Success = true;
                    }
                    else
                    {
                        jsonResult.Success = false;
                        jsonResult.Message = "名称已存在";
                    }
                }
                catch (Exception e)
                {
                    jsonResult = ErrorResponse(e.Message);
                }

            }
            return jsonResult.ToJsonResult();
        }
        [HttpPost]
        [PermissionFilter("/Role/Modify")]
        public ActionResult Update(AdminRoleEntity model, string codes)
        {
            JsonResponse jsonResult = new JsonResponse();
            if (string.IsNullOrEmpty(codes))
            {
                jsonResult.Success = false;
                jsonResult.Message = "请分配权限";
            }
            else if (string.IsNullOrEmpty(model.Name))
            {
                jsonResult.Success = false;
                jsonResult.Message = "名称不能为空";
            }
            else
            {
                try
                {
                    var entity = _permService.Get<AdminRoleEntity>(model.Id.ToString());
                    if (entity == null)
                    {
                        jsonResult.Success = false;
                        jsonResult.Message = "角色不存在";
                    }
                    else
                    {
                        //判断是否存在
                        if (_permService.CheckRole(model.Name, model.Id))
                        {
                            model.CreateTime = DateTime.Now;

                            string[] addModules = codes.Trim().Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            _permService.UpdateRole(model, addModules);
                            jsonResult.Success = true;
                        }
                        else
                        {
                            jsonResult.Success = false;
                            jsonResult.Message = "名称已存在";
                        }
                    }

                }
                catch (Exception e)
                {
                    jsonResult = ErrorResponse(e.Message);
                }

            }
            return jsonResult.ToJsonResult();
        }

        [HttpPost]
        [PermissionFilter("/Role/Modify")]
        public ActionResult Delete(string ids)
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
                    _permService.DeleteRole(ids);
                    jsonResponse.Success = true;
                }
                catch (Exception e)
                {

                    jsonResponse = ErrorResponse(e.Message);
                }
            }
            return jsonResponse.ToJsonResult();
        }

        [HttpPost]
        [PermissionFilter("/Role/Modify")]
        public ActionResult Restore(string ids)
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
                    _permService.RestoreRole(ids);
                    jsonResponse.Success = true;
                }
                catch (Exception e)
                {

                    jsonResponse = ErrorResponse(e.Message);
                }
            }
            return jsonResponse.ToJsonResult();
        }

    }
}