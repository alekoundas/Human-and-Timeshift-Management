using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataAccess.ViewModels.HourRestrictions
{
    public class ApiHourRestrictionValidate
    {
        [JsonProperty(PropertyName = "startOn", Required = Required.Default)]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "endOn", Required = Required.Default)]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "timeShiftId", Required = Required.Default)]
        public int TimeShiftId { get; set; }

        [JsonProperty(PropertyName = "employeeIds")]
        public List<int> EmployeeIds { get; set; }

    }
}
