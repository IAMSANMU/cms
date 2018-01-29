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
    public class ClassRepository:BaseRepository<ClassEntity>, IClassRepository
    {

        public override IEnumerable<T> GetListByPage<T>(int pageSize, int pageIndex, string sort, string sortType, List<SearchModel> searchModels, out long count)
        {
            Func<ClassEntity, SchoolEntity, ClassEntity> map = (classEntity, school) =>
            {
                classEntity.SchoolEntity = school;
                return classEntity;
            };

            PredicateGroup group = BuildPredGroup<T>(searchModels);

            IList<ISort> sortList = BuildSort<T>(sort, sortType);

            return (IEnumerable<T>) DapperHelper.GetPageList(pageIndex, pageSize, out count,map, group, sortList);
        }
    }
}
