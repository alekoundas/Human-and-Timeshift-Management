using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DataAccess.ViewModels
{
    public class GetForEditCellWorkHoursApiViewModel
    {
        [JsonProperty(PropertyName = "startOn")]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "endOn")]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "isDayOff")]
        public bool IsDayOff { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public string Comments { get; set; }


        [JsonProperty(PropertyName = "timeShiftId", Required = Required.Default)]
        public int TimeShiftId { get; set; }


        [JsonProperty(PropertyName = "cellDay", Required = Required.Default)]
        public int CellDay { get; set; }


        [JsonProperty(PropertyName = "employeeIds")]
        public List<int> EmployeeIds { get; set; }



        [JsonProperty(PropertyName = "employeeId", Required = Required.Default)]
        public int EmployeeId { get; set; }


        [JsonProperty(PropertyName = "workHourId", Required = Required.Default)]
        public int WorkHourId { get; set; }
    }
}
