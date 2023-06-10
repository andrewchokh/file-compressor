using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CW_FileCompressor.Entities
{
    /// <summary>
    /// The class represents side menu item. 
    /// Used for dynamic creation.
    /// </summary>
    public class SideMenuItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("icon_id")]
        public int IconID { get; set; }
        [JsonProperty("category_index")]
        public int CategoryIndex { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
