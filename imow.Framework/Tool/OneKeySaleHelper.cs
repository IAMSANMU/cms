using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imow.Framework.Tool
{
    public class OneKeySaleHelper
    {

        private static List<OneKeySaleModel> _saleModel;
        public static void Load(string filePath)
        {
            if (_saleModel == null)
            {
                DataTable excelData = ExcelHelper.ExcelToDataTable(filePath);
                BuildModel(excelData);
            }
        }

        private static void BuildModel(DataTable excelData)
        {

            List<OneKeySaleModel> ts = new List<OneKeySaleModel>();
            foreach (DataRow dr in excelData.Rows)
            {
                var columns = dr.ItemArray;
                OneKeySaleModel t = new OneKeySaleModel
                {
                    Class = columns[0].ToString(),
                    Type = columns[1].ToString(),
                    SearchKey = columns[2].ToString()
                };
                 ts.Add(t);
            }
            _saleModel = ts;
        }

        public static List<OneKeySaleModel> ExcelData()
        {
            return _saleModel;
        }

    }

    public class OneKeySaleModel
    {
        public string SearchKey { get; set; }
        public string Type { get; set; }
        public string Class { get; set; }
    }
}
