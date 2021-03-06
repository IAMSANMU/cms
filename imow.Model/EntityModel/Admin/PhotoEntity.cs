﻿using System;
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
    public class PhotoEntity
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
        ///Title
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        ///Info
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        ///IsDel
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        ///CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

        public string Img { get; set; }

    }

    /// <summary>
    /// Photo：实体对象映射关系
    /// </summary>
    [Serializable]
    public class PhotoEntityOrmMapper : ClassMapper<PhotoEntity>
    {
        public PhotoEntityOrmMapper()
        {
            base.Table("ecPhoto");
            //Map(f => f.UserID).Ignore();//设置忽略
            Map(f => f.Id).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)      
            AutoMap();
        }
    }
}
