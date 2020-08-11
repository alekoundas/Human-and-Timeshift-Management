using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DataAccess.ViewModels.Leaves
{
    public class ApiLeavesHasOverlapResponse
    {
        [JsonProperty(PropertyName = "id", Required = Required.Default)]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "employeeId", Required = Required.Default)]
        public int EmployeeId { get; set; }

        [JsonProperty(PropertyName = "givenEmployeeId", Required = Required.Default)]
        public int GivenEmployeeId { get; set; }

        [JsonProperty(PropertyName = "typeOf", Required = Required.Default)]
        public string TypeOf { get; set; }
    }
}
