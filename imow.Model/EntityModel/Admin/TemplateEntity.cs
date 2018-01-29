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
    public class TemplateEntity
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
        ///Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///Context
        /// </summary>
        [JsonIgnore]
        public string Context { get; set; }
        [JsonIgnore]
        public string DecodeContext
        {
            get
            {
                return string.IsNullOrEmpty(Context) ? "" :  StringHelper.UnBase64(Context);
            }
            set { Context = value; }
        }

        /// <summary>
        ///CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///IsDel
        /// </summary>
        public bool IsDel { get; set; }

    }

    /// <summary>
    /// Template：实体对象映射关系
    /// </summary>
    [Serializable]
    public class TemplateEntityOrmMapper : ClassMapper<TemplateEntity>
    {
        public TemplateEntityOrmMapper()
        {
            base.Table("ecTemplate");
            Map(f => f.DecodeContext).Ignore();//设置忽略
            Map(f => f.Id).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)      
            AutoMap();
        }
    }
}
