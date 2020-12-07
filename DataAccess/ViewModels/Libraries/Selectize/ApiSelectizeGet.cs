using Newtonsoft.Json;

namespace DataAccess.ViewModels
{
    public class ApiSelectizeGet
    {
        [JsonProperty(PropertyName = "search", Required = Required.Default)]
        public string Search { get; set; }

        [JsonProperty(PropertyName = "page_limit", Required = Required.Default)]
        public int PageLimit { get; set; }
    }
}
