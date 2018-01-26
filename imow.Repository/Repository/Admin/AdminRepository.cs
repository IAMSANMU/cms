using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using imow.IRepository.Admin;
using imow.Model.EntityModel.Admin;
using Imow.Framework.Db;

namespace imow.Repository.Repository.Admin
{
    public class AdminRepository : BaseRepository<AdminUserEntity>, IAdminRepository
    {
       

        public void ActiveUser(long[] ids)
        {
            string sql = $"update ecAdminUser set IsStop=0,UpdateTime=getdate() where ID in ({string.Join(",", ids)})";
            DbHelper.ExecuteSql(sql, null);
        }

        public void UnActiveUser(long[] ids)
        {
            string sql = $"update ecAdminUser set IsStop=1,UpdateTime=getdate() where ID in ({string.Join(",", ids)})";
            DbHelper.ExecuteSql(sql, null);
        }

        public void LogicDelete(long[] ids)
        {
            string sql = $"update ecAdminUser set IsDel=1 where ID in ({string.Join(",", ids)})";
            DbHelper.ExecuteSql(sql, null);
        }

        public void Restore(long[] ids)
        {
            string sql = $"update ecAdminUser set IsDel=0 where ID in ({string.Join(",", ids)})";
            DbHelper.ExecuteSql(sql, null);
        }
    }
}
