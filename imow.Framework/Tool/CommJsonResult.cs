using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;

namespace imow.Framework.Tool
{
    public class CommJsonResult
    {

        public int Code { get; set; }
        public dynamic Data { get; set; }

        public string Message { get; set; }

        public JsonResult ToJsonResult()
        {
            var tmpData = new {code = Code, message = Message,data=Data};
            JsonResult json = new JsonResult {Data = new {code = 0, data = tmpData}};
            return json;
        }
    }
}
