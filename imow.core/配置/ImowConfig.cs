using System;

namespace imow.Core.config
{
    /// <summary>
    /// 阿姆配置
    /// </summary>
    [Serializable]
    public class ImowConfig: IConfig
    {

        private string _cdnpath;

        public string CndPath
        {
            get { return _cdnpath; }
            set { _cdnpath = value; }
        }

        #region 日志配置
      
        private bool _isWriteCommonLogToLocalFile;
        /// <summary>
        /// 是否把一般日志写到本地
        /// </summary>
        public bool IsWriteCommonLogToLocalFile
        {
            get { return _isWriteCommonLogToLocalFile; }
            set { _isWriteCommonLogToLocalFile = value; }
        }
        private bool _isWriteErrorLogToLocalFile;

        /// <summary>
        /// 错误日志写入本地文件
        /// </summary>
        public bool IsWriteErrorLogToLocalFile
        {
            get { return _isWriteErrorLogToLocalFile; }
            set { _isWriteErrorLogToLocalFile = value; }
        }

    
        private bool _isWriteTimeWatchLogToLocalFile;

        /// <summary>
        /// 写耗时日志到本地
        /// </summary>
        public bool IsWriteTimeWatchLogToLocalFile
        {
            get { return _isWriteTimeWatchLogToLocalFile; }
            set { _isWriteTimeWatchLogToLocalFile = value; }
        }
        #endregion

        #region path
        public string GetConfigPath()
        {
            return "/App_Data/Imow.config";
        }
        #endregion

        #region ID配置

        private string _idzone;

        public string Idzone
        {
            get { return _idzone; }
            set { _idzone = value; }
        }

        private string _idmachine;
        public string Idmachine
        {
            get { return _idmachine; }
            set { _idmachine = value; }
        }

        private string _idstep;

        public string Idstep
        {
            get { return _idstep; }
            set { _idstep = value; }
        }

        private string _idfile;

        public string Idfile
        {
            get { return _idfile; }
            set { _idfile = value; }
        }

        #endregion


        public string SolrUrl { get; set; }

        private double _couponImowDiscountRate;

        /// <summary>
        /// 2-7大类阿母币可抵用比例
        /// </summary>
        public decimal CouponImowDiscountRate;

        /// <summary>
        /// 图片上传路径
        /// </summary>
        public string ProductUploadImageUrl { get; set; }

        public string UserUploadImageUrl { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 是否监控请求效率
        /// </summary>
        public bool Profiler { get; set; }

        #region 团购活动用于统计的peizhi

        public  string ProductActiveStartTime { get; set; }
        public string ProductActiveEndTime { get; set; }
        #endregion



        public string ProductBasePhysicalPath { get; set; }

    }
}