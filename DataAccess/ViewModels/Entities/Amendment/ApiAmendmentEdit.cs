using Newtonsoft.Json;
using System;

namespace DataAccess.ViewModels.Entities.Amendment
{
    public class ApiAmendmentEdit
    {
        [JsonProperty(PropertyName = "timeShiftId", Required = Required.Always)]
        public int TimeShiftId { get; set; }

        [JsonProperty(PropertyName = "realWorkHourId", Required = Required.AllowNull)]
        public int? RealWorkHourId { get; set; }

        [JsonProperty(PropertyName = "amendmentId", Required = Required.AllowNull)]
        public int? AmendmentId { get; set; }

        [JsonProperty(PropertyName = "employeeId", Required = Required.Always)]
        public int EmployeeId { get; set; }

        [JsonProperty(PropertyName = "comments", Required = Required.AllowNull)]
        public string Comments { get; set; }

        [JsonProperty(PropertyName = "newStartOn", Required = Required.Always)]
        public DateTime newStartOn { get; set; }

        [JsonProperty(PropertyName = "newEndOn", Required = Required.Always)]
        public DateTime newEndOn { get; set; }
    }
}
