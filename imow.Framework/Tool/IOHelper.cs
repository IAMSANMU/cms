using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using System.Web;
using System.Xml.Serialization;

namespace Imow.Framework.Tool
{
    /// <summary>
    /// IO操作帮助类
    /// </summary>
    public class IOHelper
    {

        /// <summary>
        /// 获得文件物理路径
        /// </summary>
        /// <returns></returns>
        public static string GetMapPath(string path)
        {
            var mapPath = "";
            if (HttpContext.Current != null)
            {
                mapPath = HttpContext.Current.Server.MapPath(path);
            }
            else
            {
                mapPath = System.Web.Hosting.HostingEnvironment.MapPath(path);
            }
            return mapPath;
        }

        /// <summary>
        /// 根据文件路径，创建文件对应的文件夹，若已存在则跳过
        /// </summary>
        /// <param name="filepath"></param>
        public static void CreateDirectory(string filepath)
        {
            try
            {
                string dir = System.IO.Path.GetDirectoryName(filepath);
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);
            }
            catch (Exception exp)
            {
                throw new Exception("路径"+filepath,exp);
            }
        }

        /// <summary>
        /// 目录拷贝
        /// 不支持父子目录拷贝，否则出现死循环递归
        /// </summary>
        /// <param name="srcDir"></param>
        /// <param name="tgtDir"></param>
        public static void CopyDirectory(string srcDir, string tgtDir)
        {
            DirectoryInfo source = new DirectoryInfo(srcDir);
            DirectoryInfo target = new DirectoryInfo(tgtDir);

            if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("父目录不能拷贝到子目录！");
            }

            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i].FullName, target.FullName + @"\" + files[i].Name, true);
            }

            DirectoryInfo[] dirs = source.GetDirectories();

            for (int j = 0; j < dirs.Length; j++)
            {
                CopyDirectory(dirs[j].FullName, target.FullName + @"\" + dirs[j].Name);
            }
        } 
        /// <summary>
        /// datatable 导出 excel文件
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="filefullname"></param>
        /// <returns></returns>
        public static bool DataTableToExcelFile(System.Data.DataTable tb, string filefullname)
        {
            try
            {
                //写入列标题   
                string filename = filefullname;
                System.IO.FileInfo fi = new System.IO.FileInfo(filename);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                if (System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);
                }
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                app.Visible = false;
                app.UserControl = true;
                object missing = Missing.Value;

                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(missing); ; //加载模板
                Microsoft.Office.Interop.Excel._Worksheet worksheet = workbook.Worksheets.Add(missing, missing, 1, missing) as Microsoft.Office.Interop.Excel._Worksheet; ; //第一个工作薄。
                worksheet.Name = "数据导出";

                for (int i = 0; i < tb.Columns.Count; i++)
                {
                    System.Data.DataColumn a = tb.Columns[i];
                    worksheet.Cells[1, i + 1] = a.ColumnName;
                    Range rngA = (Range)worksheet.Columns[i + 1, Type.Missing];//设置单元格格式
                    rngA.NumberFormatLocal = "@";//字符型格式
                }
                //写入列内容  
                for (int j = 0; j < tb.Rows.Count; j++)
                {
                    for (int k = 0; k < tb.Columns.Count; k++)
                    {
                        string columnValue = "";
                        if (tb.Rows[j][k] != null)
                            columnValue = tb.Rows[j][k].ToString().Trim();
                        worksheet.Cells[j + 2, k + 1] = columnValue;
                    }
                }
                //worksheet.Columns.AutoFit(); //自动调整列宽。
                workbook.SaveAs(filename, missing, missing, missing, missing,
                    missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, missing, missing, missing,
                    missing, missing);
                app.Quit();
                System.Diagnostics.Process[] ExcelProcesses;
                ExcelProcesses = System.Diagnostics.Process.GetProcessesByName("EXCEL");
                foreach (System.Diagnostics.Process IsProcedding in ExcelProcesses)
                {
                    if (IsProcedding.ProcessName == "EXCEL" && IsProcedding.MainWindowTitle == "")
                    {
                        IsProcedding.Kill();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "excel.log", e.Message + "\r\n\r\n");
                return false;
            }
        }
        /// <summary>
        /// datatable 导出 excel文件
        /// </summary>
        public static byte[] DateTableToExcelFileBytes(System.Data.DataTable dt, string fileFullname)
        {
            System.Web.UI.WebControls.DataGrid dgExport = null;
            // 当前对话 
         
            System.IO.StringWriter strWriter = null;
            System.Web.UI.HtmlTextWriter htmlWriter = null;

         //   System.Xml.XmlTextWriter xmlWriter = null;
            if (dt != null)
            {
                // 导出excel文件               
                strWriter = new System.IO.StringWriter();
                htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

                // 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid 
                dgExport = new System.Web.UI.WebControls.DataGrid();
                dgExport.DataSource = dt.DefaultView;
                dgExport.AllowPaging = false;
                dgExport.DataBind();
                // 返回客户端 
                dgExport.RenderControl(htmlWriter);
                string re = "<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + strWriter.ToString();
                return Encoding.GetEncoding("gb2312").GetBytes(re);
            }
            return null;
        }
        /// <summary>
        /// datatable 导出 excel csv文件
        /// </summary>
        public static bool DataTableToCSVFile(System.Data.DataTable tb, string filefullname)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tb.Columns.Count; i++)
            {
                sb.Append(ToCSVSafeString(tb.Columns[i].ColumnName)+",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("\r\n");
            for (int r = 0; r< tb.Rows.Count; r++)
            {
                for (int c= 0; c< tb.Columns.Count;c++)
                {
                    sb.Append(ToCSVSafeString(tb.Rows[r][c] == null ? "" : tb.Rows[r][c].ToString()) + ",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("\r\n");
            }
            sb.Remove(sb.Length - 2, 2);
            string filename = filefullname;
            System.IO.FileInfo fi = new System.IO.FileInfo(filename);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }
            if (System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }
            System.IO.File.AppendAllText(filename, sb.ToString(),Encoding.GetEncoding("GBK"));
            return true;
        }

        /// <summary>
        /// 读取文本类型文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static string ReadTextFile(string path)
        {
            return System.IO.File.ReadAllText(path);

        }

        /// <summary>
        /// 写入文本类型文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public static void WriteTextFile(string path,string content)
        {
            System.IO.File.WriteAllText(path, content);
        }
        public static void WriteTextFileUTF8(string path, string content)
        {
            System.IO.File.WriteAllText(path, content,Encoding.UTF8);
        }


        public static void SaveToFile(Stream stream, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                stream.CopyTo(sw.BaseStream);
            }
        }
        private static string ToCSVSafeString(string s)
        {
            s = s ?? "";
            string oldstring = s;
            bool has_d = s.Contains(",");
            bool has_y = s.Contains("\"");
            if (has_y)
            {
                s = s.Replace("\"", "\"\"");
            }
            if (has_d || has_y)
            {
                s = "\"" + s + "\"";
            }
            return s;
        }

        #region  序列化

        /// <summary>
        /// XML序列化
        /// </summary>
        /// <param name="obj">序列对象</param>
        /// <param name="filePath">XML文件路径</param>
        /// <returns>是否成功</returns>
        public static bool SerializeToXml(object obj, string filePath)
        {
            bool result = false;
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return result;

        }

        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <param name="filePath">XML文件路径</param>
        /// <returns>序列对象</returns>
        public static T DeserializeFromXMLText<T>(string xmlStr) where T : class
        { 
            try
            {
                byte[] array = Encoding.ASCII.GetBytes(xmlStr);
                MemoryStream stream = new MemoryStream(array);      
                StreamReader reader = new StreamReader(stream);
               

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <param name="filePath">XML文件路径</param>
        /// <returns>序列对象</returns>
        public static T DeserializeFromXML<T>(string filePath) where T:class
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        public static object DeserializeFromXML(Type type,string filePath)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        #endregion
    }
}
