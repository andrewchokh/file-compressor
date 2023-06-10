using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace CW_FileCompressor.Entities
{
    /// <summary>
    /// The class represents user of the program. 
    /// Used for read / write data.
    /// </summary>
    public class User
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        public override string ToString()
        {
            return Username;
        }
    }
}
