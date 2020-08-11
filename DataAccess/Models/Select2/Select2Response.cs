using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DataAccess.Models.Select2
{
    public class Select2Response
    {
        [JsonProperty(PropertyName = "results")]
        public ICollection<Select2Result> Results { get; set; }
        [JsonProperty(PropertyName = "pagination")]
        public Select2Pagination Pagination { get; set; }
    }
}
