using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using DapperExtensions;
using imow.Core;
using imow.Framework.Api;
using imow.Framework.Tool;
using imow.IRepository;
using imow.Model.EntityModel;
using Imow.Framework.Cache.DistributedCache.Storage;
using Imow.Framework.Engine;
using Imow.Framework.Log;
using Imow.Framework.Serialization;

namespace imow.Services.BussinessService
{
    public class AreaService : IBaseService
    {

        private readonly IBaseRepository<AreaEntity> _areaDao;
        private readonly CacheFactory _cache;
        public AreaService(IBaseRepository<AreaEntity> areaRepository, CacheFactory cache)
        {
            _areaDao = areaRepository;
            _cache = cache;
        }

        /// <summary>
        /// 判断是否已缓存Area,并加入缓存
        /// </summary>
        public void InitCache()
        {
            //所有省
            var noCache = CheckNoCache();
            if (noCache)
            {
                IEnumerable<AreaEntity> all = GetAll();
                //省下所有市
                Dictionary<int, IEnumerable<AreaEntity>> provinceCityMap = new Dictionary<int, IEnumerable<AreaEntity>>();
                //市下所有区
                Dictionary<int, IEnumerable<AreaEntity>> cityAreaMap = new Dictionary<int, IEnumerable<AreaEntity>>();
                //区下所有街道
                Dictionary<int, IEnumerable<AreaEntity>> areaStreeMap = new Dictionary<int, IEnumerable<AreaEntity>>();

                IEnumerable<AreaEntity> proviceList = all.Where(f => f.AreaLevel == 3);
                IEnumerable<AreaEntity> cityList = all.Where(f => f.AreaLevel == 4);
                IEnumerable<AreaEntity> areaList = all.Where(f => f.AreaLevel == 5);
                IEnumerable<AreaEntity> streeList = all.Where(f => f.AreaLevel == 6);
                foreach (var item in proviceList)
                {
                    provinceCityMap.Add(item.AreaID, cityList.Where(f => f.ParentID == item.AreaID));
                }
                foreach (var item in cityList)
                {
                    cityAreaMap.Add(item.AreaID, areaList.Where(f => f.ParentID == item.AreaID));
                }
                foreach (var item in areaList)
                {
                    areaStreeMap.Add(item.AreaID, streeList.Where(f => f.ParentID == item.AreaID));
                }
                _cache.SetValue(CacheKey.ProCityCacheKey, provinceCityMap);
                _cache.SetValue(CacheKey.CityAreaCacheKey, cityAreaMap);
                _cache.SetValue(CacheKey.AreaStreeCacheKey, areaStreeMap);
                _cache.SetValue(CacheKey.ProvinceCacheKey, proviceList);
            }
        }

        private bool CheckNoCache()
        {
            return _cache.GetValue<IEnumerable<AreaEntity>>(CacheKey.ProvinceCacheKey)==null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AreaEntity> GetAll()
        {
           return _cache.GetOrSetValue(CacheKey.AllAreaCacheKey, () => _areaDao.GetList());
        }

        public IEnumerable<AreaEntity> GetProvince()
        {
            InitCache();
            var data= _cache.GetValue<IEnumerable<AreaEntity>>(CacheKey.ProvinceCacheKey);
            return data;
        }

        public IEnumerable<AreaEntity> GetCityByProvince(int id)
        {
            InitCache();
            var data= _cache.GetValue<Dictionary<int,IEnumerable<AreaEntity>>>(CacheKey.ProCityCacheKey);
            return data[id];
        }
        public IEnumerable<AreaEntity> GetAreaByCity(int id)
        {
            InitCache();
            var data = _cache.GetValue<Dictionary<int, IEnumerable<AreaEntity>>>(CacheKey.CityAreaCacheKey);
            return data[id];
        }
        public IEnumerable<AreaEntity> GetStreeByArea(int id)
        {
            InitCache();
            var data = _cache.GetValue<Dictionary<int, IEnumerable<AreaEntity>>>(CacheKey.AreaStreeCacheKey);
            return data[id];
        }

    }
}