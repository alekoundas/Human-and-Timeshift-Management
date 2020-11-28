using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DataAccess.Libraries.Select2
{
    public class Select2Result
    {
        [JsonProperty(PropertyName = "id")]
        public int id { get; set; }
        [JsonProperty(PropertyName = "text", Required = Required.Default)]
        public string Text { get; set; }
    }
}
