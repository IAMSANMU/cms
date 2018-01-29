using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperExtensions.Mapper;
using FluentValidation.Attributes;
using Imow.Framework.Tool;
using Newtonsoft.Json;

namespace imow.Model.EntityModel.Admin
{
    /// <summary>
    /// imow.Model.EntityModel.Admin：实体对象
    /// </summary>
    [Serializable]
    public class SchoolEntity
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
        ///Tel
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        ///ProvinceId
        /// </summary>
        public int? ProvinceId { get; set; }

        /// <summary>
        ///CityId
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        ///AreaId
        /// </summary>
        public int? AreaId { get; set; }

        /// <summary>
        ///StreetId
        /// </summary>
        public int? StreetId { get; set; }

        /// <summary>
        ///Addr
        /// </summary>
        public string Addr { get; set; }

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
                return string.IsNullOrEmpty(Info) ? "" : (StringHelper.IsBase64String(Info) ? StringHelper.UnBase64(Info) : Info);
            }
            set { Info = value; }
        }

        /// <summary>
        ///IsDel
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        ///CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///UpdateTime
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        ///Province
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        ///City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        ///Area
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        ///Street
        /// </summary>
        public string Street { get; set; }

    }

    /// <summary>
    /// School：实体对象映射关系
    /// </summary>
    [Serializable]
    public class SchoolEntityOrmMapper : ClassMapper<SchoolEntity>
    {
        public SchoolEntityOrmMapper()
        {
            base.Table("ecSchool");
            Map(f => f.DecodeInfo).Ignore();//设置忽略
            Map(f => f.Id).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)      
            AutoMap();
        }
    }
}
