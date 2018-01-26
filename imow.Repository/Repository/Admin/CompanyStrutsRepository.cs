using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using imow.IRepository.Admin;
using imow.Model.EntityModel.Admin;
using Imow.Framework.Db;
using DapperExtensions;

namespace imow.Repository.Repository.Admin
{
    public class CompanyStrutsRepository : BaseRepository<CompanyStrutsEntity>, ICompanyStrutsRepository
    {
        public IEnumerable<CompanyStrutsEntity> GetParentByAdminId(long adminId)
        {
            string sql =
                "WITH etc AS(SELECT *  FROM ecCompanyStruts  WHERE Code IN (SELECT placeCode FROM ecAdminStruts WHERE AdminId="+adminId + ")" +
                "UNION ALL  SELECT A.*  FROM ecCompanyStruts AS A JOIN etc AS B ON A.ID = B.PID)" +
                "SELECT DISTINCT * FROM etc ORDER BY etc.Code";
            return DapperHelper.Get<CompanyStrutsEntity>(sql);
        }

        public IEnumerable<CompanyStrutsEntity> GetParentById(long id)
        {
            string sql =
                "WITH etc AS(SELECT *  FROM ecCompanyStruts  WHERE id = "+id +
                "UNION ALL  SELECT A.*  FROM ecCompanyStruts AS A JOIN etc AS B ON A.ID = B.PID)" +
                "SELECT DISTINCT * FROM etc ORDER BY etc.Code";
            return DapperHelper.Get<CompanyStrutsEntity>(sql);
        }


        public IEnumerable<CompanyStrutsEntity> GetChildrenById(long id)
        {
            string sql = "WITH etc AS(SELECT *  FROM ecCompanyStruts  WHERE id ="+id +
                         "UNION ALL  SELECT A.*FROM ecCompanyStruts AS A JOIN etc AS B ON A.Pid = B.ID)"
                         + "SELECT* FROM etc ORDER BY etc.Code";

            return DapperHelper.Get<CompanyStrutsEntity>(sql);
        }

        public void Restore(long[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                StringBuilder sql = new StringBuilder("update ecCompanyStruts set isDel =0 where isDel=1 and   ");
                sql.AppendFormat(" id in ({0})", string.Join(",", ids));
                foreach (var id in ids)
                {
                    sql.AppendFormat(" or (code like (select '%'+code+'%' from ecCompanyStruts where id={0}))", id);
                }
                DbHelper.ExecuteSql(sql.ToString(), null);
            }
        }

        public void LogicDelete(long[] ids)
        {
            if (ids != null && ids.Length > 0)
            {

                StringBuilder sql = new StringBuilder("update ecCompanyStruts set isDel = 1 where    ");
                sql.AppendFormat(" id in ({0})", string.Join(",", ids));
                foreach (var id in ids)
                {
                    sql.AppendFormat(" or (code like (select '%'+code+'%' from ecCompanyStruts where id={0}))", id);
                }
                DbHelper.ExecuteSql(sql.ToString(), null);
            }
        }

        /// <summary>
        /// 获得当前部门列表
        /// </summary>
        /// <param name="code">部门code</param>
        /// <returns></returns>
        public IEnumerable<CompanyStrutsEntity> GetDepartList(string code)
        {
            var value = $"{code}%";
            List<IPredicate> preList = new List<IPredicate>
            {
                Predicates.Field<CompanyStrutsEntity>(f => f.Levels, Operator.Eq, 2),
                Predicates.Field<CompanyStrutsEntity>(f => f.Code, Operator.Like, value),
                Predicates.Field<CompanyStrutsEntity>(f => f.IsLast, Operator.Eq, 1)
            };
            var group = Predicates.Group(GroupOperator.And, preList.ToArray());

            return DapperHelper.GetList<CompanyStrutsEntity>(group);
        }



        /// <summary>
        /// 根据编码获取部门信息
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns></returns>
        public IEnumerable<CompanyStrutsEntity> GetDepartByCode(string code)
        {
            string sql = "WITH etc AS(SELECT *  FROM ecCompanyStruts  WHERE Code =" + code +
                         ")"
                         + "SELECT* FROM etc ORDER BY etc.Code";

            return DapperHelper.Get<CompanyStrutsEntity>(sql);
        }

        public IEnumerable<CompanyStrutsEntity> GetOwnerCompany(long adminId)
        {
            string sql = $"select distinct s.* from ecCompanyStruts s inner join ecAdminStruts st on  s.id=st.companyId where st.adminId={adminId}";
            var list = DapperHelper.Get<CompanyStrutsEntity>(sql, null);
            return list;
        }
    }
}
