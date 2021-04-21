using Newtonsoft.Json;
using System;

namespace DataAccess.ViewModels
{
    public class WorkHourApiCreate
    {
        [JsonProperty(PropertyName = "startOn", Required = Required.Default)]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "endOn", Required = Required.Default)]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public string Comments { get; set; }

        [JsonProperty(PropertyName = "timeShiftId", Required = Required.Default)]
        public int TimeShiftId { get; set; }

        [JsonProperty(PropertyName = "employeeId", Required = Required.Default)]
        public int EmployeeId { get; set; }
    }
}
