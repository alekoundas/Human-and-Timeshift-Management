using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataAccess.Libraries.Select2
{
    public class Select2
    {
        [JsonProperty(PropertyName = "search")]
        public string Search { get; set; }

        [JsonProperty(PropertyName = "page", Required = Required.Default)]
        public int Page { get; set; }

        [JsonProperty(PropertyName = "fromEntityId")]
        public int FromEntityId { get; set; }


        [JsonProperty(PropertyName = "existingIds")]
        public List<int> ExistingIds { get; set; }





        //For Security entities id
        [JsonProperty(PropertyName = "fromEntityIdString")]
        public string FromEntityIdString { get; set; }


        [JsonProperty(PropertyName = "existingIdsString")]
        public List<string> ExistingIdsString { get; set; }
    }
}
