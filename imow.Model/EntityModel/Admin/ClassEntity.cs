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
        ///SchoolId
        /// </summary>
        public int SchoolId { get; set; }

        /// <summary>
        ///Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///Type
        /// </summary>
        public string Type { get; set; }

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
        ///UpdateTime
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        ///Info
        /// </summary>
        [JsonIgnore]
        public string Info { get; set; }
        [JsonIgnore]
        public string DecodeInfo
        {
            get
            {
                return string.IsNullOrEmpty(Info) ? "" : StringHelper.UnBase64(Info);
            }
            set { Info = value; }
        }

        /// <summary>
        ///Photo
        /// </summary>
        public string Photo { get; set; }

        public SchoolEntity SchoolEntity { get; set; }

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
            Map(f => f.SchoolEntity).Ignore();//设置忽略
            Map(f => f.DecodeInfo).Ignore();//设置忽略
            Map(f => f.Id).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)      
            Map(f => f.SchoolId).ForignKey<SchoolEntity>(s => s.Id);
            AutoMap();
        }
    }
}
