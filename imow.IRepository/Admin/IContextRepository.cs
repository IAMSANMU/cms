using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using imow.Model.EntityModel.Admin;

namespace imow.IRepository.Admin
{
    public interface IContextRepository : IBaseRepository<ContextEntity>
    {
        #region 前端方法
        IEnumerable<ContextEntity> GetListByPage(int pageSize, int pageIndex, out long count);

        IEnumerable<ContextEntity> GetTop(int pageSize);

        #endregion
    }
}
