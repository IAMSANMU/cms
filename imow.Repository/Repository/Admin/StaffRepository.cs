using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using imow.IRepository.Admin;
using imow.Model.EntityModel;
using imow.Model.EntityModel.Admin;

namespace imow.Repository.Repository.Admin
{
    public class StaffRepository : BaseRepository<StaffEntity>, IStaffRepository
    {
        public override IEnumerable<T> GetListByPage<T>(int pageSize, int pageIndex, string sort, string sortType, List<SearchModel> searchModels, out long count)
        {
            Func<StaffEntity, SchoolEntity, StaffEntity> map = (staff, school) =>
            {
                staff.SchoolEntity = school;
                return staff;
            };

            PredicateGroup group = BuildPredGroup<T>(searchModels);

            IList<ISort> sortList = BuildSort<T>(sort, sortType);

            return (IEnumerable<T>)DapperHelper.GetPageList(pageIndex, pageSize, out count, map, group, sortList);
        }
    }
}
