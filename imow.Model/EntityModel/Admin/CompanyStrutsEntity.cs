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
    public class CompanyStrutsEntity
    {
        /// <summary>
        ///Id
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        ///名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///父级
        /// </summary>
        public Int64 Pid { get; set; }

        /// <summary>
        ///深度
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        ///级别 1/2/3=公司/部门/岗位
        /// </summary>
        public int Levels { get; set; }

        /// <summary>
        ///是否删除
        /// </summary>
        public bool IsDel { get; set; }

        /// <summary>
        ///是否最底部
        /// </summary>
        public bool IsLast { get; set; }

        /// <summary>
        ///编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///创建日期
        /// </summary>
        public DateTime CreateTime { get; set; }


      
    }

    /// <summary>
    /// CompanyStruts：实体对象映射关系
    /// </summary>
    [Serializable]
    public class CompanyStrutsEntityOrmMapper : ClassMapper<CompanyStrutsEntity>
    {
        public CompanyStrutsEntityOrmMapper()
        {
            base.Table("ecCompanyStruts");
            //Map(f => f.UserID).Ignore();//设置忽略
            Map(f => f.Id).Key(KeyType.Assigned);//设置主键  (如果主键名称不包含字母“ID”，请设置)    
           
            AutoMap();
        }
    }
}
