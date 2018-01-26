using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using imow.IRepository.Admin;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;
using Imow.Framework.Db;

namespace imow.Repository.Repository.Admin
{
    public class PermissionRepository : BaseRepository<AdminModuleEntity>, IPermissionRepository
    {
        public void RestoreModule(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {
                StringBuilder sql = new StringBuilder("update ecAdminModule set isDel =0 where isDel=1 and   ");
                sql.AppendFormat(" id in ({0})", string.Join(",", ids));
                foreach (var id in ids)
                {
                    sql.AppendFormat(" or (code like (select '%'+code+'%' from ecAdminModule where id={0}))", id);
                }
                DbHelper.ExecuteSql(sql.ToString(), null);
            }
        }

        public void DeleteModule(int[] ids)
        {
            if (ids != null && ids.Length > 0)
            {

                StringBuilder sql=new StringBuilder("update ecAdminModule set isDel = 1 where    ");
                sql.AppendFormat(" id in ({0})",string.Join(",", ids));
                foreach (var id in ids)
                {
                    sql.AppendFormat(" or (code like (select '%'+code+'%' from ecAdminModule where id={0}))", id);
                }
                DbHelper.ExecuteSql(sql.ToString(),null);
            }
        }

        public IEnumerable<AdminModuleEntity> GetAdminModuleList()
        {
            return DapperHelper.GetList<AdminModuleEntity>();
        }

        public IEnumerable<AdminModuleEntity> GetModuleByIds(int[] moduleIds)
        {
            IEnumerable<AdminModuleEntity> result = null;
            if (moduleIds != null && moduleIds.Length > 0)
            {
                List<IPredicate> orList=new List<IPredicate>();
                foreach (int id in moduleIds)
                {
                   orList.Add(Predicates.Field<AdminModuleEntity>(f => f.Id, Operator.Eq, id));
                }
                var group = Predicates.Group(GroupOperator.And,Predicates.Field<AdminModuleEntity>(f=>f.IsDel,Operator.Eq, false));

                var orGroup = Predicates.Group(GroupOperator.Or, orList.ToArray());
                group.Predicates.Add(orGroup);
                result = DapperHelper.GetList<AdminModuleEntity>(group);
            }
            return result;
        }


        public IEnumerable<AdminRoleEntity> GetRoleList(IPredicate group)
        {
            return DapperHelper.GetList<AdminRoleEntity>(group);
        }

        public IEnumerable<AdminRoleModuleEntity> GetRoleModuleList(long roleId)
        {
            var pre = Predicates.Field<AdminRoleModuleEntity>(f => f.RoleId, Operator.Eq, roleId);

            return DapperHelper.GetList<AdminRoleModuleEntity>(pre, null);
        }

        public T Get<T>(string id) where T : class
        {
            return DapperHelper.GetById<T>(id);
        }

        public void Update<T>(T t) where T : class
        {
            DapperHelper.Update(t);
        }
        public void Add<T>(T t) where T : class
        {
            DapperHelper.Insert(t);
        }

        public void Delete<T>(T t) where T : class
        {
            DapperHelper.Delete<T>(t);
        }

        public void DeleteBatch<T>(IPredicate predGroup) where T : class
        {
            DapperHelper.DeleteList<T>(predGroup);
        }

        public void LogicDeleteRole(long[] ids)
        {

            string sql = $"update ecAdminRole set isDel=1 where isDel=0 and id in ({string.Join(",", ids)})";
            DbHelper.ExecuteSql(sql, null);
        }


        public void RestoreRole(long[] ids)
        {
            string sql = $"update ecAdminRole set isDel=0 where isDel=1 and id in ({string.Join(",", ids)})";
            DbHelper.ExecuteSql(sql, null);
        }

        public IEnumerable<AdminUserRoleEntity> GetUserRoleList(long userId)
        {
            Func<AdminUserRoleEntity, AdminRoleEntity, AdminUserRoleEntity> map = (entity, role) =>
            {
                entity.AdminRoleEntity = role;
                return entity;
            };
            var predicates = Predicates.Field<AdminUserRoleEntity>(f => f.AdminId, Operator.Eq, userId);
            var list = DapperHelper.Get(map, JoinType.Inner, predicates);
            if (list.Any())
            {
                return list;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<AdminRoleEntity> GetRoleList(bool isDel)
        {
            var predicate = Predicates.Field<AdminRoleEntity>(f => f.IsDel, Operator.Eq, isDel);

            return DapperHelper.GetList<AdminRoleEntity>(predicate, null);
        }
    }
}
