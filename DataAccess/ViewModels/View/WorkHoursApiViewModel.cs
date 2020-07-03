using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models;
using DataAccess.Models.Entity;
using DataAccess.Models.Entity.WorkTimeShift;
using Newtonsoft.Json;

namespace DataAccess.ViewModels.View
{
    public class WorkHoursApiViewModel : BaseEntity
    {
        [JsonProperty(PropertyName = "startOn", Required = Required.Default)]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "endOn", Required = Required.Default)]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "timeShiftId", Required = Required.Default)]
        public int TimeShiftId { get; set; }

        [JsonProperty(PropertyName = "employeeId", Required = Required.Default)]
        public int EmployeeId { get; set; }


        public static WorkHour CreateFrom(WorkHoursApiViewModel viewModel)
        {
            return new WorkHour
            {
                StartOn = viewModel.StartOn,
                EndOn = viewModel.EndOn,
                TimeShiftId = viewModel.TimeShiftId,
                CreatedOn = DateTime.Now
            };
        }
    }
}