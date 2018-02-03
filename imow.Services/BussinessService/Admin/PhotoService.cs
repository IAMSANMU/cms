using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;
using imow.Core;
using imow.Core.config;
using imow.IRepository;
using imow.Model.EntityModel.Admin;
using Imow.Framework.Cache.DistributedCache.Storage;
using Imow.Framework.Db;
using Imow.Framework.Engine;

namespace imow.Services.BussinessService.Admin
{
    public class PhotoService : IBaseService
    {
        private readonly IBaseRepository<PhotoEntity> _dao;
        private readonly IBaseRepository<PhotoImgEntity> _imgDao;
        private readonly CacheFactory _cache;

        public PhotoService(IBaseRepository<PhotoEntity> dao, IBaseRepository<PhotoImgEntity> imgDao,CacheFactory cache)
        {
            _dao = dao;
            _imgDao = imgDao;
            _cache = cache;
        }

        public IEnumerable<PhotoEntity> GetAll(bool isDel)
        {
            List<IPredicate> list =
                new List<IPredicate> { Predicates.Field<PhotoEntity>(f => f.IsDel, Operator.Eq, isDel) };

            var group = Predicates.Group(GroupOperator.And, list.ToArray());

            return _dao.GetList(group).OrderByDescending(f => f.CreateTime);
        }

        public PhotoEntity Get(int id)
        {
            return _dao.Get(id.ToString());
        }

        public void Add(PhotoEntity entity)
        {
            _dao.Add(entity);
        }
        public void Update(PhotoEntity entity)
        {
            _dao.Update(entity);
            RemoveCache(entity.Id);
        }

        public void Delete(int id)
        {
            PhotoEntity photo = _dao.Get(id.ToString());
            photo.IsDel = true;
            _dao.Update(photo);
        }

        public void Restore(int id)
        {
            PhotoEntity photo = _dao.Get(id.ToString());
            photo.IsDel = false;
            _dao.Update(photo);
        }


        public IEnumerable<PhotoImgEntity> GetImgAll(int photoId)
        {
            List<IPredicate> list =
                new List<IPredicate> {
                    Predicates.Field<PhotoImgEntity>(f => f.IsDel, Operator.Eq, false),
                    Predicates.Field<PhotoImgEntity>(f => f.PhotoId, Operator.Eq, photoId) 
                };

            var group = Predicates.Group(GroupOperator.And, list.ToArray());

            return _imgDao.GetList(group).OrderByDescending(f => f.CreateTime);
        }

        public void AddImg(PhotoImgEntity entity)
        {
            PhotoEntity photo = _dao.Get(entity.PhotoId.ToString());
            photo.Img = entity.Url;
            using (TransactionScope scope = new TransactionScope())
            {
                _imgDao.Add(entity);
                Update(photo);
                scope.Complete();
            }
        }
        public void UpdateImg(PhotoImgEntity entity)
        {
            _imgDao.Update(entity);
            RemoveCache(entity.PhotoId);
        }
        public void DeleteImg(int id)
        {
            PhotoImgEntity img = _imgDao.Get(id.ToString());
            img.IsDel = true;
            _imgDao.Update(img);
            RemoveCache(img.PhotoId);
        }

        public PhotoImgEntity GetImg(int id)
        {
            return _imgDao.Get(id.ToString());
        }



        #region 前台方法

        public IEnumerable<PhotoImgEntity> GetIndexImg()
        {

            ImowConfig imowConfig = ImowEngineContext.Current.ResolveConfig<ImowConfig>();
            int photoId = imowConfig.PhotoId;
            string cacheKey = string.Format(CacheKey.IndexPhotoCacheKey, photoId);
            return _cache.GetOrSetValue(cacheKey, () => GetImgAll(photoId));
        }

        public void RemoveCache(int pid)
        {
            ImowConfig imowConfig = ImowEngineContext.Current.ResolveConfig<ImowConfig>();
            int photoId = imowConfig.PhotoId;
            if (pid == photoId)
            {
                _cache.Delete(new string[] { string.Format(CacheKey.IndexPhotoCacheKey, photoId) });
            }
        }

        #endregion

    }
}
