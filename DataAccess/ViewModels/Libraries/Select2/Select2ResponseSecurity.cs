using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataAccess.Libraries.Select2
{
    public class Select2ResponseSecurity
    {
        [JsonProperty(PropertyName = "results")]
        public ICollection<Select2ResultSecurity> Results { get; set; }

        [JsonProperty(PropertyName = "pagination")]
        public Select2Pagination Pagination { get; set; }
    }
}
