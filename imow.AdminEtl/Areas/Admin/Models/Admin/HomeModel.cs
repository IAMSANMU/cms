using System.Collections.Generic;
using System.Linq;
using imow.Model.EntityModel.Admin;

namespace imow.AdminEtl.Areas.Admin.Models.Admin
{
    public class HomeModel
    {
        public List<AdminModuleEntity> ModuleList { get; set; }


        public List<AdminModuleEntity> SelectedList { get; set; }

        private List<AdminModuleEntity> _firstLevelList;
        /// <summary>
        /// 一级模块
        /// </summary>
        public List<AdminModuleEntity> FirstLevelList {
            get
            {
                if (ModuleList.Any())
                {
                    _firstLevelList = ModuleList.Where(i => i.Depth == 0).OrderBy(j => j.Id).ToList();
                }
                return _firstLevelList;
            }
        }

    }
}