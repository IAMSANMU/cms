using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperExtensions.Mapper;
using Imow.Framework.Tool;
using Newtonsoft.Json;

namespace imow.Model.EntityModel.Admin
{
    /// <summary>
    /// imow.Model.EntityModel.Admin：实体对象
    /// </summary>
    [Serializable]
    public class ClassEntity
    {
        /// <summary>
        ///Id
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        ///Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///IsDel
        /// </summary>
        public bool IsDel { get; set; }
        /// <summary>
        /// 主修/辅修
        /// </summary>
        public bool IsMain { get; set; }

        /// <summary>
        ///CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }


        /// <summary>
        ///Photo
        /// </summary>
        public string Photo { get; set; }

        public int? OrderNum { get; set; }

        public string Remark { get; set; }

        public string Student { get; set; }

        public string Teacher { get; set; }
        public string Book { get; set; }

    }

    /// <summary>
    /// Class：实体对象映射关系
    /// </summary>
    [Serializable]
    public class ClassEntityOrmMapper : ClassMapper<ClassEntity>
    {
        public ClassEntityOrmMapper()
        {
            base.Table("ecClass");
            Map(f => f.Id).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)      
            AutoMap();
        }
    }
}
