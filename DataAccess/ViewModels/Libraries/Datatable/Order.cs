using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DataAccess.Libraries.Datatable
{
    public class Order
    {
        [JsonPropertyName("column")]
        public int Column { get; set; }

        [JsonPropertyName("dir")]
        public string Dir { get; set; }
    }
}
