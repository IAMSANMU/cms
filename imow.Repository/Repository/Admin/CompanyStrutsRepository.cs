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



    }
}
