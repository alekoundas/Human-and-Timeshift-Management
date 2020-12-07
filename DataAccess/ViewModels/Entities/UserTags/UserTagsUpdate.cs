using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataAccess.ViewModels
{
    public class UserTagsUpdate
    {
        [JsonProperty(PropertyName = "userId", Required = Required.Default)]
        public string UserId { get; set; }


        [JsonProperty(PropertyName = "values", Required = Required.Default)]
        public List<string> Values { get; set; }
    }
}
