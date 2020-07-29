using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models;
using DataAccess.Models.Entity;
using Newtonsoft.Json;

namespace DataAccess.ViewModels.WorkHours
{
    public class WorkHoursApiViewModel : BaseEntity
    {
        [JsonProperty(PropertyName = "startOn")]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "endOn")]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "cellDay")]
        public int CellDay { get; set; }

        [JsonProperty(PropertyName = "timeShiftId")]
        public int TimeShiftId { get; set; }

        [JsonProperty(PropertyName = "workHourId")]
        public int WorkHourId { get; set; }

        [JsonProperty(PropertyName = "employeeId")]
        public int EmployeeId { get; set; }

        [JsonProperty(PropertyName = "employeeIds")]
        public List<int> EmployeeIds { get; set; }


        //public static WorkHour CreateFrom(WorkHoursApiViewModel viewModel)
        //{
        //    return new WorkHour
        //    {
        //        StartOn = viewModel.StartOn,
        //        EndOn = viewModel.EndOn,
        //        TimeShiftId = viewModel.TimeShiftId,
        //        CreatedOn = DateTime.Now
        //    };
        //}
    }
}