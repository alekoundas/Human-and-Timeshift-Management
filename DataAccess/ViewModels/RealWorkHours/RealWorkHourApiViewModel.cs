using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models;
using Newtonsoft.Json;

namespace DataAccess.ViewModels.RealWorkHours
{
    public class RealWorkHourApiViewModel : BaseEntity
    {
        [JsonProperty(PropertyName = "startOn", Required = Required.Default)]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "endOn", Required = Required.Default)]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "timeShiftId", Required = Required.Default)]
        public int TimeShiftId { get; set; }

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