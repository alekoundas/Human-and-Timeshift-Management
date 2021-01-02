using Newtonsoft.Json;

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


    }
}
