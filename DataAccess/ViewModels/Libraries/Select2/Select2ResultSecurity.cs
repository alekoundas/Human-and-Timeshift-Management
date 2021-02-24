using Newtonsoft.Json;

namespace DataAccess.Libraries.Select2
{
    public class Select2ResultSecurity
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "text", Required = Required.Default)]
        public string Text { get; set; }
    }
}
