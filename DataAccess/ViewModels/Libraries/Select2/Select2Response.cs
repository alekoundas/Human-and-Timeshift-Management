using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataAccess.Libraries.Select2
{
    public class Select2Response
    {
        [JsonProperty(PropertyName = "results")]
        public ICollection<Select2Result> Results { get; set; }

        [JsonProperty(PropertyName = "pagination")]
        public Select2Pagination Pagination { get; set; }
    }
}
