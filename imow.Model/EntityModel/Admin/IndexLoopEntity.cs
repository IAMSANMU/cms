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
    public class IndexLoopEntity
    {
        /// <summary>
        ///Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///Url
        /// </summary>
        public string Img { get; set; }

        public string Link { get; set; }

        /// <summary>
        ///IsDel
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        ///CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///OrderNum
        /// </summary>
        public int? OrderNum { get; set; }

    }

    /// <summary>
    /// IndexLoop：实体对象映射关系
    /// </summary>
    [Serializable]
    public class IndexLoopEntityOrmMapper : ClassMapper<IndexLoopEntity>
    {
        public IndexLoopEntityOrmMapper()
        {
            base.Table("ecIndexLoop");
            //Map(f => f.UserID).Ignore();//设置忽略
            Map(f => f.Id).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)      
            AutoMap();
        }
    }
}
