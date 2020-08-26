using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DataAccess.ViewModels.UserRole
{
    public class ApiUpdateWorkPlaceRoles
    {
        [JsonProperty(PropertyName = "userId", Required = Required.Default)]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "workPlaceIdsToDelete")]
        public List<string> WorkPlaceIdsToDelete { get; set; }

        [JsonProperty(PropertyName = "workPlaceValues")]
        public List<WorkPlaceValues> WorkPlacesValues { get; set; }
    }
    public class WorkPlaceValues
    {
        [JsonProperty(PropertyName = "existingWorkPlaceId", Required = Required.Default)]
        public string ExistingWorkPlaceId { get; set; }
        [JsonProperty(PropertyName = "newWorkPlaceId", Required = Required.Default)]
        public int NewWorkPlaceId { get; set; }
    }

}
