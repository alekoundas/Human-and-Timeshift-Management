using Newtonsoft.Json;
using System;

namespace DataAccess.ViewModels
{
    public class WorkHourDelete
    {
        [JsonProperty(PropertyName = "startOn")]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "endOn")]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "timeShiftId")]
        public int TimeShiftId { get; set; }

        [JsonProperty(PropertyName = "employeeId")]
        public int EmployeeId { get; set; }
    }
}
