using System;
using DapperExtensions.Mapper;
using Newtonsoft.Json;

namespace imow.Model.EntityModel
{
    [Serializable]
    public class AreaEntity
    {
        /// <summary>
        ///AreaID
        /// </summary>
        public int AreaID { get; set; }

        /// <summary>
        ///AreaCode
        /// </summary>
        [JsonIgnore]
        public string AreaCode { get; set; }

        /// <summary>
        ///AreaName
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        ///AreaLevel
        /// </summary>
        [JsonIgnore]
        public int AreaLevel { get; set; }

        /// <summary>
        ///ParentID
        /// </summary>
        [JsonIgnore]
        public int ParentID { get; set; }

        /// <summary>
        ///ChlidCount
        /// </summary>
        [JsonIgnore]
        public int ChlidCount { get; set; }

        /// <summary>
        ///OrderNum
        /// </summary>
        [JsonIgnore]
        public int OrderNum { get; set; }

        /// <summary>
        ///IsShipArea
        /// </summary>
        [JsonIgnore]
        public bool IsShipArea { get; set; }

        /// <summary>
        ///Zip
        /// </summary>
        [JsonIgnore]
        public string Zip { get; set; }

        /// <summary>
        ///IsStop
        /// </summary>
        [JsonIgnore]
        public bool? IsStop { get; set; }

    }

    [Serializable]
    public class AreaEntityOrmMapper : ClassMapper<AreaEntity>
    {
        public AreaEntityOrmMapper()
        {
            base.Table("ecArea");
            Map(f => f.AreaID).Key(KeyType.Assigned);
            AutoMap();
        }
    }

}