using System;
using System.Linq;
using System.Web;

namespace EC.Web.Util
{
    public class WebHelper
    {
        private static readonly string[] _staticFileExtensions= new[] { ".axd", ".ashx", ".bmp", ".css", ".gif", ".htm", ".html", ".ico", ".jpeg", ".jpg", ".js", ".png", ".rar", ".zip" };
        public static bool IsStaticResource(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            string path = request.Path;
            string extension = VirtualPathUtility.GetExtension(path);

            if (extension == null) return false;
            var contains = _staticFileExtensions.Contains(extension);
            return contains;
        }
    }
}