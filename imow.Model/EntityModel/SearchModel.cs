using System;
using DapperExtensions;

namespace imow.Model.EntityModel
{
    public class SearchModel
    {
        public string Value { get; set; }

        public string Operator { get; set; }

        public string Column { get; set; }
        /// <summary>
        /// 等于 or 不等于
        /// </summary>
        public bool Not { get; set; } = false;

        /// <summary>
        /// 自动/不自动  拼接查询条件
        /// </summary>
        public bool NotAuto { get; set; } = false;

        public Operator Op => (Operator)Enum.Parse(typeof(Operator),Operator );

        public string DisposeValue => Op == DapperExtensions.Operator.Like ? "%" + Value + "%" : Value;
    }
}