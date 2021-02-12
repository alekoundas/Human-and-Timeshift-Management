using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DataAccess.ViewModels
{
    public class ApiRealWorkHoursHasOverlapRange
    {
        [JsonProperty(PropertyName = "startOn")]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "endOn")]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "excludeStartOn")]
        public DateTime ExcludeStartOn { get; set; }

        [JsonProperty(PropertyName = "excludeEndOn")]
        public DateTime ExcludeEndOn { get; set; }

        [JsonProperty(PropertyName = "isEdit")]
        public bool IsEdit { get; set; }


        [JsonProperty(PropertyName = "timeShiftId")]
        public int TimeShiftId { get; set; }

        [JsonProperty(PropertyName = "employeeIds")]
        public List<int> EmployeeIds { get; set; }
    }
}
