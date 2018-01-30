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
    public class OrderEntity
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
        ///SchoolId
        /// </summary>
        public int SchoolId { get; set; }

        /// <summary>
        ///ClassId
        /// </summary>
        public int ClassId { get; set; }

        /// <summary>
        ///Msg
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        ///0/1/2=新加/已联系/结束
        /// </summary>
        public int Status { get; set; }

        public string StatusStr {
            get
            {
                string str = "";
                switch (Status)
                {
                    case 0:
                        str = "未回访";
                        break;
                    case 1:
                        str = "回访中";
                        break;
                    case 2:
                        str = "已回访";
                        break;
                }
                return str;

            }
        }

        /// <summary>
        ///Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///UpdateTime
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        ///AdminId
        /// </summary>

        /// <summary>
        ///Source
        /// </summary>
        public string Source { get; set; }

        public ClassEntity ClassEntity { get; set; }

        public SchoolEntity SchoolEntity { get; set; }

    }

    /// <summary>
    /// Order：实体对象映射关系
    /// </summary>
    [Serializable]
    public class OrderEntityOrmMapper : ClassMapper<OrderEntity>
    {
        public OrderEntityOrmMapper()
        {
            base.Table("ecOrder");
            Map(f => f.ClassEntity).Ignore();//设置忽略
            Map(f => f.SchoolEntity).Ignore();//设置忽略
            Map(f => f.StatusStr).Ignore();//设置忽略
            Map(f => f.Id).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)    
            Map(f => f.ClassId).ForignKey<ClassEntity>(c => c.Id);
            Map(f => f.SchoolId).ForignKey<SchoolEntity>(c => c.Id);
            AutoMap();
        }
    }
}
