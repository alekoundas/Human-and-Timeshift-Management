using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DataAccess.ViewModels.WorkHours
{
    public class EditWorkHoursApiViewModel
    {
        [JsonProperty(PropertyName = "startOn")]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "endOn")]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "newStartOn")]
        public DateTime NewStartOn { get; set; }

        [JsonProperty(PropertyName = "newEndOn")]
        public DateTime NewEndOn { get; set; }


        [JsonProperty(PropertyName = "timeShiftId")]
        public int TimeShiftId { get; set; }


        [JsonProperty(PropertyName = "employeeId")]
        public int EmployeeId { get; set; }
    }
}
