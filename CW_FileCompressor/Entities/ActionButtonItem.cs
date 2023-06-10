using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CW_FileCompressor.Entities
{
    /// <summary>
    /// The class represents action button item. 
    /// Used for dynamic creation.
    /// </summary>
    public class ActionButtonItem
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("icon_id")]
        public int IconID { get; set; }
        [JsonProperty("button_id")]
        public int ButtonID { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
