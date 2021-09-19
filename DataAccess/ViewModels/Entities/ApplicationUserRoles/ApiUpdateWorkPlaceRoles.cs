using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataAccess.ViewModels
{
    public class ApiUpdateWorkPlaceRoles
    {
        [JsonProperty(PropertyName = "userId", Required = Required.Default)]
        public string UserId { get; set; }


        [JsonProperty(PropertyName = "workPlaceIds")]
        public List<int> WorkPlaceIds { get; set; }
    }

}
