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
    public class StaffStrutsEntity
    {
        /// <summary>
        ///Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///StaffId
        /// </summary>
        public Int64 StaffId { get; set; }

        /// <summary>
        ///SchoolId
        /// </summary>
        public int SchoolId { get; set; }

        /// <summary>
        ///DepartId
        /// </summary>
        public int DepartId { get; set; }

        /// <summary>
        ///PlaceId
        /// </summary>
        public int PlaceId { get; set; }

    }

    /// <summary>
    /// StaffStruts：实体对象映射关系
    /// </summary>
    [Serializable]
    public class StaffStrutsEntityORMMapper : ClassMapper<StaffStrutsEntity>
    {
        public StaffStrutsEntityORMMapper()
        {
            base.Table("ecStaffStruts");
            //Map(f => f.UserID).Ignore();//设置忽略
            Map(f => f.Id).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)      
            AutoMap();
        }
    }
}
