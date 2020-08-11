using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DataAccess.Models.Select2
{
    public class Select2Pagination
    {
        [JsonProperty(PropertyName = "more")]
        public bool More { get; set; }
    }
}
