using DataAccess.Models.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DataAccess.ViewModels
{
    public class LeaveCreate
    {
        [JsonProperty(PropertyName = "startOn", Required = Required.Default)]
        public DateTime StartOn { get; set; }

        [JsonProperty(PropertyName = "endOn", Required = Required.Default)]
        public DateTime EndOn { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "approvedBy")]
        public string ApprovedBy { get; set; }

        [JsonProperty(PropertyName = "leaveTypeId", Required = Required.Default)]
        public int LeaveTypeId { get; set; }

        [JsonProperty(PropertyName = "employeeIds", Required = Required.Default)]
        public List<int> EmployeeIds { get; set; }

        public static List<Leave> CreateRangeFrom(LeaveCreate viewModel)
        {
            var results = new List<Leave>();
            foreach (var employeeId in viewModel.EmployeeIds)
            {
                results.Add(new Leave()
                {
                    StartOn = viewModel.StartOn,
                    EndOn = viewModel.EndOn,
                    Description = viewModel.Description,
                    ApprovedBy = viewModel.ApprovedBy,
                    LeaveTypeId = viewModel.LeaveTypeId,
                    EmployeeId = employeeId,
                    CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                    CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                    CreatedOn = DateTime.Now
                });
            }
            return results;
        }
    }
}
