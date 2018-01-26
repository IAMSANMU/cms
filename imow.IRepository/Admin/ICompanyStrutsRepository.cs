using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using imow.Model.EntityModel.Admin;

namespace imow.IRepository.Admin
{
    public interface ICompanyStrutsRepository :IBaseRepository<CompanyStrutsEntity>
    {
        /// <summary>
        /// 根据adminId获取父级(包含当前)
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        IEnumerable<CompanyStrutsEntity> GetParentByAdminId(long adminId);
        /// <summary>
        /// 根据adminId获取父级(包含当前)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<CompanyStrutsEntity> GetParentById(long id);
        /// <summary>
        /// 获取子级别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<CompanyStrutsEntity> GetChildrenById(long id);
        void LogicDelete(long[] ids);

        void Restore(long[] ids);

        IEnumerable<CompanyStrutsEntity> GetDepartList(string code);


        IEnumerable<CompanyStrutsEntity> GetDepartByAdminId(long adminId, long comapnyId);

        /// <summary>
        /// 根据编码获取部门信息
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns></returns>
        IEnumerable<CompanyStrutsEntity> GetDepartByCode(string code);
        /// <summary>
        /// 用户所属的公司列表
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        IEnumerable<CompanyStrutsEntity> GetOwnerCompany(long adminId);

    }
}
