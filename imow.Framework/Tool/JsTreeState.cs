using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace imow.Framework.Tool
{
    public class JsTreeState
    {
        [JsonProperty("opened")]
        public bool Opened { get; set; }
        [JsonProperty("selected")]
        public bool Selected { get; set; }
        [JsonProperty("disabled")]
        public bool Disabled { get; set; }
    }
}
