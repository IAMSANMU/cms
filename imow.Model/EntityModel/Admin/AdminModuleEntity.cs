using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperExtensions.Mapper;
using Newtonsoft.Json;

namespace imow.Model.EntityModel.Admin
{
    /// <summary>
    /// imow.Model.EntityModel.Admin：实体对象
    /// </summary>
    [Serializable]
    public class AdminModuleEntity
    {
        /// <summary>
        ///ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        ///Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///ParentID
        /// </summary>
        public int Pid { get; set; }

        /// <summary>
        ///Url
        /// </summary>
        public string Url { get; set; }


        /// <summary>
        ///Depth
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        ///CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///是否显示在菜单上
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }

        public string Code { get; set; }

        public bool IsLast { get; set; }

        public List<AdminModuleEntity> Children { get; set; }

        /// <summary>
        /// AdminModule：实体对象映射关系
        /// </summary>
        [Serializable]
        public class AdminModuleEntityOrmMapper : ClassMapper<AdminModuleEntity>
        {
            public AdminModuleEntityOrmMapper()
            {
                Map(f => f.Children).Ignore();
                base.Table("ecAdminModule");
                Map(f => f.Id).Key(KeyType.Identity); //设置主键  (如果主键名称不包含字母“ID”，请设置)      
                AutoMap();
            }
        }
    }
}
