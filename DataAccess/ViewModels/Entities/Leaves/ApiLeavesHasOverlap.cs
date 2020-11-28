using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DataAccess.ViewModels
{
    public class ApiLeavesHasOverlap
    {
        [JsonProperty(PropertyName = "startOn", Required = Required.Default)]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "endOn", Required = Required.Default)]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "employeeIds", Required = Required.Default)]
        public List<int> EmployeeIds { get; set; }
    }
}
