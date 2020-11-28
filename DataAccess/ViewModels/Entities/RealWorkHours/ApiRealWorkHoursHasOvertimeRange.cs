using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DataAccess.ViewModels
{
    public class ApiRealWorkHoursHasOvertimeRange
    {
        [JsonProperty(PropertyName = "startOn", Required = Required.Default)]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "endOn", Required = Required.Default)]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "isEdit")]
        public bool IsEdit { get; set; }

        [JsonProperty(PropertyName = "timeShiftId", Required = Required.Default)]
        public int TimeShiftId { get; set; }

        [JsonProperty(PropertyName = "employeeIds")]
        public List<int> EmployeeIds { get; set; }

        [JsonProperty(PropertyName = "excludeStartOn")]
        public DateTime ExcludeStartOn { get; set; }

        [JsonProperty(PropertyName = "excludeEndOn")]
        public DateTime ExcludeEndOn { get; set; }
    }
}
