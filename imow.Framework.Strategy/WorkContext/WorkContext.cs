using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using imow.Framework.Interface;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;
using imow.Services;
using imow.Services.BussinessService.Admin;
using Imow.Framework.Engine;
using Imow.Framework.Extensions;

namespace imow.Framework.Strategy.WorkContext
{
    public class WorkContext : IWorkContext
    {

        private AdminUserEntity _adminEntity;
        public IEnumerable<AdminModuleEntity> ModuleList { get; set; }
        public string PermStr
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (ModuleList != null && ModuleList.Any())
                {
                    foreach (var item in ModuleList)
                    {
                        if (!String.IsNullOrEmpty(item.Url))
                        {
                            sb.Append("," + item.Url);
                        }
                    }
                    if (sb.Length > 0)
                    {
                        sb.Remove(0, 1);
                    }
                }
                string strPath = "";
                if (sb.Length > 0)
                {
                    Encoding encode = Encoding.ASCII;
                    byte[] bytedata = encode.GetBytes(sb.ToString());
                    strPath = Convert.ToBase64String(bytedata, 0, bytedata.Length);
                }
                return strPath;
            }
        }

        public AdminUserEntity AdminEntity
        {
            get
            {
                if (_adminEntity == null)
                {
                    _adminEntity = ImowEngineContext.Current.Resolve<AdminUserService>().GetCurrentUserEntity();

                }
                return _adminEntity;
            }
        }


        public bool IsAdminLogined => AdminEntity != null;

        public string LoginUrl { get; }
    }
}