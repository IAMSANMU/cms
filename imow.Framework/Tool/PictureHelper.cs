using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using EC.Common.Extensions;

namespace imow.Framework.Tool
{
    public class PictureHelper
    {

        //private readonly string _uploadBasePhysicalPath;
        private static readonly string _uploadFileName = "Upload";
        private static readonly string _productPicFileName = "Product";
        private static readonly string _tempPicFileName = "Temp";
        private static readonly string _userFileName = "User";
        private static readonly string _picBaseUrl = "/";


        /// <summary>
        /// 图片上传物理基路径
        /// 如果未设置，则为默认网站的路径
        /// </summary>
        public static string UploadBasePhysicalPath()
        {
            return Path.Combine(PathHelper.GetAppBasePath(), _uploadFileName);

        }
        /// <summary>
        /// 商品上传基路径
        /// </summary>
        public static string ProductBasePhysicalPath()
        {
            return Path.Combine(UploadBasePhysicalPath(), _productPicFileName);
        }

        /// <summary>
        /// 图片上传临时目录基路径
        /// </summary>
        public static string TempBasePhysicalPath()
        {
            return Path.Combine(UploadBasePhysicalPath(), _tempPicFileName);
        }

        /// <summary>
        /// 图片urlhttp 基地址  
        /// 以 / 结尾
        /// </summary>
        public static string PicUploadUrl()
        {
            return _picBaseUrl.EndSuffix("/").Contact(_uploadFileName.EndSuffix("/"));
        }

        /// <summary>
        /// 商品图片urlhttp 基地址
        /// 以 / 结尾
        /// </summary>
        public static string ProductPicBaseUrl()
        {
            return PicUploadUrl().Contact(_productPicFileName.EndSuffix("/"));
        }

        /// <summary>
        /// 临时图片urlhttp 基地址
        /// 以 / 结尾
        /// </summary>
        public static string TempPicBaseUrl()
        {
            return PicUploadUrl().Contact(_tempPicFileName.EndSuffix("/"));
        }


        /// <summary>
        /// 用户图片上传基路径
        /// </summary>
        public static string UserBasePhysicalPath()
        {
            return Path.Combine(UploadBasePhysicalPath(), _userFileName);
        }

        /// <summary>
        /// 用户图片urlhttp 基地址
        /// 以 / 结尾
        /// </summary>
        public static string UserPicBaseUrl()
        {
            return PicUploadUrl().Contact(_userFileName.EndSuffix("/"));
        }
        public static bool IsAllowedExtension(HttpPostedFileBase file)
        {
            return true;
            //    bool ret = false;
            //    Stream fs = file.InputStream;
            //    BinaryReader r = new BinaryReader(fs);
            //    string fileclass = "";
            //    byte buffer;
            //    try
            //    {
            //        buffer = r.ReadByte();
            //        fileclass = buffer.ToString();
            //        buffer = r.ReadByte();
            //        fileclass += buffer.ToString();
            //    }
            //    catch
            //    {
            //        return false;
            //    }
            //    finally
            //    {
            //        r.Close();
            //        fs.Close();
            //    }
            //    /*文件扩展名说明
            //     *4946/104116 txt
            //     *7173        gif 
            //     *255216      jpg
            //     *13780       png
            //     *6677        bmp
            //     *239187      txt,aspx,asp,sql
            //     *208207      xls.doc.ppt
            //     *6063        xml
            //     *6033        htm,html
            //     *4742        js
            //     *8075        xlsx,zip,pptx,mmap,zip
            //     *8297        rar   
            //     *01          accdb,mdb
            //     *7790        exe,dll           
            //     *5666        psd 
            //     *255254      rdp 
            //     *10056       bt种子 
            //     *64101       bat 
            //     *4059        sgf
            //     */
            //    //String[] fileType = { "255216", "7173", "6677", "13780", "8297", "5549", "870", "87111", "8075" };

            //    //纯图片
            //    string[] fileType = {
            //    "7173",    //gif
            //    "255216",  //jpg
            //    "13780"    //png
            //};
            //    foreach (string t in fileType)
            //    {
            //        if (fileclass == t)
            //        {
            //            ret = true;
            //            break;
            //        }
            //    }
            //    return ret;
        }
    }
}
