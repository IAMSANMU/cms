namespace imow.Core
{
    public class CacheKey
    {
        /// <summary>
        /// 发送短信保存到缓存的验证码
        /// </summary>
        public static string MobileVerifyCode = "MessageService-SendMobileVerifyCode";
        /// <summary>
		/// ad.user.module:{0}
		/// </summary>
        public static readonly string VerifyCodeKey = "ao#8dIf6&a1A";

        public static readonly string AllModuleKey = "ad.all.module";

        public static readonly string AdminUserModuleKey = "ad.user.module:{0}";

        /// <summary>
        /// 所有省市区的缓存
        /// </summary>
        public static readonly string AllAreaCacheKey = "Area-All";
        /// <summary>
        /// 所有省
        /// </summary>
        public static readonly string ProvinceCacheKey = "Area-Province";
        /// <summary>
        /// 省下所有city
        /// </summary>
        public static readonly string ProCityCacheKey = "Area-ProviceCity";
        public static readonly string CityAreaCacheKey = "Area-CityArea";
        public static readonly string AreaStreeCacheKey = "Area-AreaStree";

        public static readonly string IndexLoopCacheKey = "Index-Loop";
        public static readonly string IndexSchoolCacheKey = "Index-School";
        public static readonly string IndexClassCacheKey = "Index-Class";
        public static readonly string IndexTeacherCacheKey = "Index-Teacher";




    }
}