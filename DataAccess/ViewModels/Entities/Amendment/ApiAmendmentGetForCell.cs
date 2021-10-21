using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DataAccess.ViewModels
{
    public class ApiAmendmentGetForCell
    {
        [JsonProperty(PropertyName = "timeShiftId", Required = Required.Always)]
        public int TimeShiftId { get; set; }

        [JsonProperty(PropertyName = "day", Required = Required.Always)]
        public int Day { get; set; }

        [JsonProperty(PropertyName = "employeeId", Required = Required.AllowNull)]
        public int? EmployeeId { get; set; }

    }
}
