using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace imow.AdminEtl.Models.Admin
{
    public class SaveUserModel
    {
        public long Id { get; set; }

        public string RealName { get; set; }

        public string Sex { get; set; }

        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Remark { get; set; }

        public string Roles { get; set; }

        public string QQ { get; set; }

        public string IsStop { get; set; }

        public string UserName { get; set; }
        public string Pwd { get; set; }
        public string PwdSure { get; set; }



    }
}