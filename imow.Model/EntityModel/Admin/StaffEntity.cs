using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperExtensions.Mapper;

namespace imow.Model.EntityModel.Admin
{
    /// <summary>
    /// imow.Model.EntityModel.Admin：实体对象
    /// </summary>
    [Serializable]
    public class StaffEntity
    {
        /// <summary>
        ///Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///IsChina
        /// </summary>
        public bool IsChina { get; set; }

        /// <summary>
        ///CName
        /// </summary>
        public string CName { get; set; }

        /// <summary>
        ///EName
        /// </summary>
        public string EName { get; set; }

        /// <summary>
        ///Sex
        /// </summary>
        public bool Sex { get; set; }

        /// <summary>
        ///Birthday
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        ///Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///Info
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        ///Tel
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        ///Head
        /// </summary>
        public string Head { get; set; }

        /// <summary>
        ///Origo
        /// </summary>
        public string Origo { get; set; }

        /// <summary>
        ///SchoolId
        /// </summary>
        public int SchoolId { get; set; }

        /// <summary>
        ///IsDel
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        ///CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///IsCommand
        /// </summary>
        public bool IsCommand { get; set; }

        /// <summary>
        ///IsStop
        /// </summary>
        public bool IsStop { get; set; }

        public SchoolEntity SchoolEntity { get; set; }

    }

    /// <summary>
    /// Staff：实体对象映射关系
    /// </summary>
    [Serializable]
    public class StaffEntityOrmMapper : ClassMapper<StaffEntity>
    {
        public StaffEntityOrmMapper()
        {
            base.Table("ecStaff");
            Map(f => f.SchoolEntity).Ignore();//设置忽略
            Map(f => f.Id).Key(KeyType.Identity); //设置主键  (如果主键名称不包含字母“ID”，请设置)      
            Map(f => f.SchoolId).ForignKey<SchoolEntity>(f => f.Id);
            AutoMap();
        }
    }
}

