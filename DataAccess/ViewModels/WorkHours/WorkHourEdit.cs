using Newtonsoft.Json;
using System;

namespace DataAccess.ViewModels
{
    public class WorkHourEdit
    {
        [JsonProperty(PropertyName = "startOn", Required = Required.Default)]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "endOn", Required = Required.Default)]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "newStartOn", Required = Required.Default)]
        public DateTime NewStartOn { get; set; }

        [JsonProperty(PropertyName = "newEndOn", Required = Required.Default)]
        public DateTime NewEndOn { get; set; }

        [JsonProperty(PropertyName = "isDayOff", Required = Required.Default)]
        public bool IsDayOff { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public string Comments { get; set; }

        [JsonProperty(PropertyName = "timeShiftId", Required = Required.Default)]
        public int TimeShiftId { get; set; }


        [JsonProperty(PropertyName = "employeeId", Required = Required.Default)]
        public int EmployeeId { get; set; }
    }
}
