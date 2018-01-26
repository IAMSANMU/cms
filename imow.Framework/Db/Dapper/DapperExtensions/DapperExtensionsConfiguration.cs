using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using DapperExtensions.Mapper;
using DapperExtensions.Sql;

namespace DapperExtensions
{
    public interface IDapperExtensionsConfiguration
    {
        Type DefaultMapper { get; }
        IList<Assembly> MappingAssemblies { get; }
        ISqlDialect Dialect { get; }
        IClassMapper GetMap(Type entityType);
        IClassMapper GetMap<T>() where T : class;

        IClassMapper[] GetMap<TFirst, TSecond>();
        IClassMapper[] GetMap<TFirst, TSecond,TThird>();
        IClassMapper[] GetMap<TFirst, TSecond, TThird, TFourth>();

        IClassMapper[] GetMap<TFirst, TSecond, TThird, TFourth,TFive>();

        IClassMapper[] GetMap<TFirst, TSecond, TThird, TFourth, TFive,TSix>();
        void ClearCache();
        Guid GetNextGuid();
    }

    public class DapperExtensionsConfiguration : IDapperExtensionsConfiguration
    {
        private readonly ConcurrentDictionary<Type, IClassMapper> _classMaps = new ConcurrentDictionary<Type, IClassMapper>();

        public DapperExtensionsConfiguration()
            : this(typeof(AutoClassMapper<>), new List<Assembly>(), new SqlServerDialect())
        {
        }


        public DapperExtensionsConfiguration(Type defaultMapper, IList<Assembly> mappingAssemblies, ISqlDialect sqlDialect)
        {
            DefaultMapper = defaultMapper;
            MappingAssemblies = mappingAssemblies ?? new List<Assembly>();
            Dialect = sqlDialect;
        }

        public Type DefaultMapper { get; private set; }
        public IList<Assembly> MappingAssemblies { get; private set; }
        public ISqlDialect Dialect { get; private set; }

        public IClassMapper GetMap(Type entityType)
        {
            IClassMapper map;
            if (!_classMaps.TryGetValue(entityType, out map))
            {
                Type mapType = GetMapType(entityType);
                if (mapType == null)
                {
                    mapType = DefaultMapper.MakeGenericType(entityType);
                } 
                
                map = Activator.CreateInstance(mapType) as IClassMapper;
                _classMaps[entityType] = map;
            }
            return map;
        }
 

        public IClassMapper GetMap<T>() where T : class
        {
            return GetMap(typeof (T));
        }

        public IClassMapper[] GetMap<TFirst, TSecond>()
        {
            var tFirstClassMapper = GetMap(typeof(TFirst));
            var tSecondClassMapper = GetMap(typeof(TSecond));
            return new IClassMapper[] { tFirstClassMapper, tSecondClassMapper };

        }
        //CX.ADD
        public IClassMapper[] GetMap<TFirst, TSecond,TThird>()
        {
            var tFirstClassMapper = GetMap(typeof(TFirst));
            var tSecondClassMapper = GetMap(typeof(TSecond));
            var tThirdClassMapper = GetMap(typeof(TThird));
            IClassMapper[] l= new IClassMapper[] { tFirstClassMapper, tSecondClassMapper, tThirdClassMapper };
            return l;
        }

        public IClassMapper[] GetMap<TFirst, TSecond, TThird, TFourth>()
        {
            var tFirstClassMapper = GetMap(typeof(TFirst));
            var tSecondClassMapper = GetMap(typeof(TSecond));
            var tThirdClassMapper = GetMap(typeof(TThird));
            var tFourthClassMapper = GetMap(typeof(TFourth));
            IClassMapper[] l = new IClassMapper[] { tFirstClassMapper, tSecondClassMapper, tThirdClassMapper, tFourthClassMapper };
            return l;
        }

        public IClassMapper[] GetMap<TFirst, TSecond, TThird, TFourth, TFive>()
        {
            var tFirstClassMapper = GetMap(typeof(TFirst));
            var tSecondClassMapper = GetMap(typeof(TSecond));
            var tThirdClassMapper = GetMap(typeof(TThird));
            var tFourthClassMapper = GetMap(typeof(TFourth));
            var tFiveClassMapper = GetMap(typeof(TFive));
            IClassMapper[] l = new IClassMapper[] { tFirstClassMapper, tSecondClassMapper, tThirdClassMapper, tFourthClassMapper, tFiveClassMapper };
            return l;
        }

        public IClassMapper[] GetMap<TFirst, TSecond, TThird, TFourth, TFive, TSix>()
        {
            var tFirstClassMapper = GetMap(typeof(TFirst));
            var tSecondClassMapper = GetMap(typeof(TSecond));
            var tThirdClassMapper = GetMap(typeof(TThird));
            var tFourthClassMapper = GetMap(typeof(TFourth));
            var tFiveClassMapper = GetMap(typeof(TFive));
            var tSixClassMapper = GetMap(typeof(TSix));
            IClassMapper[] l = new IClassMapper[] { tFirstClassMapper, tSecondClassMapper, tThirdClassMapper, tFourthClassMapper, tFiveClassMapper , tSixClassMapper };
            return l;
        }

        public void ClearCache()
        {
            _classMaps.Clear();
        }

        public Guid GetNextGuid()
        {
            byte[] b = Guid.NewGuid().ToByteArray();
            DateTime dateTime = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            TimeSpan timeSpan = new TimeSpan(now.Ticks - dateTime.Ticks);
            TimeSpan timeOfDay = now.TimeOfDay;
            byte[] bytes1 = BitConverter.GetBytes(timeSpan.Days);
            byte[] bytes2 = BitConverter.GetBytes((long)(timeOfDay.TotalMilliseconds / 3.333333));
            Array.Reverse(bytes1);
            Array.Reverse(bytes2);
            Array.Copy(bytes1, bytes1.Length - 2, b, b.Length - 6, 2);
            Array.Copy(bytes2, bytes2.Length - 4, b, b.Length - 4, 4);
            return new Guid(b);
        }

        protected virtual Type GetMapType(Type entityType)
        {
            Func<Assembly, Type> getType = a =>
            {
                Type[] types = a.GetTypes();
                return (from type in types 
                        let interfaceType = type.GetInterface(typeof(IClassMapper<>).FullName)
                        where
                            interfaceType != null &&
                            interfaceType.GetGenericArguments()[0] == entityType
                        select type).SingleOrDefault();
            };

            Type result = getType(entityType.Assembly);
            if (result != null)
            {
                return result;
            }

            foreach (var mappingAssembly in MappingAssemblies)
            {
                result = getType(mappingAssembly);
                if (result != null)
                {
                    return result;
                }
            }

            return getType(entityType.Assembly);
        }
    }
}