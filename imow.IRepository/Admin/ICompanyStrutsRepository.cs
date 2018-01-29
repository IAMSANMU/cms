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
        void LogicDelete(long[] ids);

        void Restore(long[] ids);


    }
}
