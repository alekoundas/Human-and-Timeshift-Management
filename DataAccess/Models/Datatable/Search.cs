using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DataAccess.Models.Datatable
{
    public class Search
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }

        //[JsonPropertyName("regex")]
        //public string Regex { get; set; }
    }

}
