using System;
using System.IO;

namespace imow.Framework.Tool
{
    public sealed class PathHelper
    {
        /// <summary>
        /// 得到应用程序的基物理路径
        /// </summary>
        /// <returns>String</returns>
        public static String GetAppBasePath()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }
        /// <summary>
        /// 合并路径 类似Path.Combine
        /// </summary>
        /// <param name="path1">path1</param>
        /// <param name="path2">path2</param>
        /// <returns>String</returns>
        public static String Combine(String path1, String path2)
        {
            return Path.Combine(path1, path2);
        }
        /// <summary>
        /// 得到文件的扩展名
        /// <remarks>
        ///     返回的值带点号
        /// </remarks>
        /// <example>
        ///     .gif
        /// </example>
        /// </summary>
        /// <param name="file">文件名</param>
        /// <returns></returns>
        public static String GetExtension(String file)
        {
            return Path.GetExtension(file);
        }
        /// <summary>
        /// 确定指定的文件是否存在。
        /// </summary>
        /// <param name="filePath">要检查的文件。</param>
        /// <returns>
        /// 如果调用方具有要求的权限并且 path 包含现有文件的名称，则为 true；
        /// 否则为 false。
        /// 如果 path 为 null、无效路径或零长度字符串，
        /// 则此方法也将返回false。
        /// 如果调用方不具有读取指定文件所需的足够权限，则不引发异常并且该方法返回 false，这与 path 是否存在无关。</returns>
        public static Boolean ExistFile(String filePath)
        {
            return File.Exists(filePath);
        }
        /// <summary>
        /// 是否存在置顶的目录
        /// </summary>
        /// <param name="path">测试的目录</param>
        public static Boolean ExistDirectory(String path)
        {
            return Directory.Exists(path);
        }
		/// <summary>
        /// 皮肤的物理基路径
        /// </summary>
        /// <returns></returns>
        public static String GetThemePath()
        {
            return PathHelper.Combine(PathHelper.GetAppBasePath(), "Themes");
        }
        /// <summary>
        /// 皮肤的物理基路径
        /// </summary>
        /// <returns></returns>
        public static String GetThemePath(String themeName)
        {
			return PathHelper.Combine(PathHelper.GetAppBasePath(), "Themes\\" + themeName);
        }
		/// <summary>
		/// 得到pop店铺模板物理基路径
		/// </summary>
		/// <param name="popID">popid</param>
		/// <param name="themeName">名称</param>
		public static string GetPOPPCThemePath(string popID, string themeName)
		{
			return Path.Combine(PathHelper.GetAppBasePath(), "Themes\\POP", popID, "PC",  themeName);
		}

		/// <summary>
		/// 得到pop店铺模板物理基路径
		/// </summary>
		public static string GetPOPSystemPCThemePath()
		{
			return Path.Combine(PathHelper.GetAppBasePath(), "Themes\\POP\\System\\PC");
		}

		/// <summary>
		/// 得到pop店铺模板相对基路径
		/// </summary>
		/// <param name="popID">popid</param>
		/// <param name="themeName">名称</param>
		public static string GetPOPPCRelThemePath(string popID, string themeName)
		{
		    return string.Concat("Themes/POP/", popID, "/PC/", themeName);
		}

        public static String GetThemeTemplatePath(String themeId, String source)
        {
            return PathHelper.Combine(PathHelper.GetThemePath(), PathHelper.Combine(themeId, source));
        }
        /// <summary>
        /// 得到文件名称（包括扩展名）
        /// </summary>
        public static String GetFileName(String path)
        {
            return Path.GetFileName(path);
        }
    }
}
