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
    public class ContextEntity
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
        ///Context
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        ///ArticleId
        /// </summary>
        public int ArticleId { get; set; }

        /// <summary>
        ///Author
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        ///CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///Status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        ///IsDel
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        ///ViewCount
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        ///AdminId
        /// </summary>
        public Int64 AdminId { get; set; }

        /// <summary>
        ///Link
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        ///1/2=内容/跳转
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        ///Tags
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        ///IsTop
        /// </summary>
        public bool IsTop { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        public string Remark { get; set; }

    }

    /// <summary>
    /// Context：实体对象映射关系
    /// </summary>
    [Serializable]
    public class ContextEntityOrmMapper : ClassMapper<ContextEntity>
    {
        public ContextEntityOrmMapper()
        {
            base.Table("ecContext");
            //Map(f => f.UserID).Ignore();//设置忽略
            Map(f => f.Id).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)      
            AutoMap();
        }
    }
}
