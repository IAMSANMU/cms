using System.Data;
using System.IO;
using System.IO.Compression;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace imow.Framework.Tool
{
    public class ExcelHelper
    {
        /// <summary>
        /// Excel转DataTable
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            DataTable dt = new DataTable();
            string xlsType = "2003";
            string ext = Path.GetExtension(filePath);
            if (".xls".Equals(ext))
            {
                xlsType = "2003";
            }
            else if (".xlsx".Equals(ext))
            {
                xlsType = "2007";
            }
            else
            {
                return null;
            }

            ISheet sheet = null;
            if ("2007".Equals(xlsType))
            {
                XSSFWorkbook xssfworkbook;
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    xssfworkbook = new XSSFWorkbook(file);
                }
                sheet = xssfworkbook.GetSheetAt(0);
            }
            else if ("2003".Equals(xlsType))
            {
                HSSFWorkbook hssfworkbook;
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
                sheet = hssfworkbook.GetSheetAt(0);
            }

            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null)
                {
                    continue;
                }
                DataRow dataRow = dt.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }


        /// <summary>
        /// Excel转DataTable
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(ZipArchiveEntry excelArchive)
        {

            DataTable dt = new DataTable();
            string xlsType = "2003";
            string ext = Path.GetExtension(excelArchive.Name);
            if (".xls".Equals(ext))
            {
                xlsType = "2003";
            }
            else if (".xlsx".Equals(ext))
            {
                xlsType = "2007";
            }
            else
            {
                return null;
            }

            ISheet sheet = null;
            using (Stream excelStream = excelArchive.Open())
            {
                if (".xls".Equals(ext))
                {
                    var xssfworkbook = new XSSFWorkbook(excelStream);
                    sheet = xssfworkbook.GetSheetAt(0);
                }
                else if (".xlsx".Equals(ext))
                {
                    var workbook = new XSSFWorkbook(excelStream);
                    sheet = workbook.GetSheetAt(0);
                }
            }

            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                if (row == null)
                {
                    continue;
                }
                DataRow dataRow = dt.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }


        /// <summary>
        /// 將DataTable轉成Stream輸出.
        /// </summary>
        /// <param name="SourceTable">The source table.</param>
        /// <returns></returns>
        public static Stream RenderDataTableToExcel(DataTable SourceTable)
        {
            var workbook = new HSSFWorkbook();
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            dsi.Company = "阿母工业";
            si.Author = "阿母工业";
            si.Comments = "感谢阿母工业";
            workbook.SummaryInformation = si;
            workbook.DocumentSummaryInformation = dsi;
            //    InitializeWorkbook();
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workbook.CreateSheet();
            IRow headerRow = sheet.CreateRow(0);

            // handling header.
            foreach (DataColumn column in SourceTable.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

            // handling value.
            int rowIndex = 1;

            foreach (DataRow row in SourceTable.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);

                foreach (DataColumn column in SourceTable.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }

                rowIndex++;
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;

            sheet = null;
            headerRow = null;
            workbook = null;

            return ms;
        }

    }
}