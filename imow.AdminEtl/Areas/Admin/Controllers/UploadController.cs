using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using imow.Core.config;
using imow.Framework.Strategy.Controller;
using imow.Framework.Strategy.Filter;
using imow.Framework.Tool;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;
using imow.Services.BussinessService;
using Imow.Framework.Engine;

namespace imow.AdminEtl.Areas.Admin.Controllers
{
    public class UploadController : BaseController
    {
        [NoPerm]
        public ActionResult Index(string type)
        {
            JsonResponse json = new JsonResponse();
            try
            {
                string rootPath = "";
                string date = DateTime.Now.ToString("yyyyMMdd");
                rootPath = "/Upload/" + type+"/";
                rootPath += date + "/";
                string path = Server.MapPath(rootPath);
                List<string> datas = new List<string>();
                var objs = Request.Files;
                foreach (string str in objs)
                {
                    var file = objs[str];
                    if (file != null)
                    {
                        string fileName = ID.GetNextId() + "_" + file.FileName;
                        string savePath = rootPath+ fileName;
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                        file.SaveAs(path + fileName);
                        datas.Add(savePath);
                    }
                }
                json.Success = true;
                json.Data = datas;
            }
            catch (Exception e)
            {
                json = ErrorResponse(e.Message);
            }
            return json.ToJsonResult();
        }


    }
}