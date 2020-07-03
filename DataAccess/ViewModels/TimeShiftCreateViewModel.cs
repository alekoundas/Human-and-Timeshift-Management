using System;
using System.Collections.Generic;
using DataAccess.Models.Entity.WorkTimeShift;

namespace DataAccess.ViewModels
{
    public class TimeShiftCreateViewModel
    {
        public string Title { get; set; }
        public DateTime StartOn { get; set; }
        public DateTime EndOn { get; set; }
        public int WorkPlaceId { get; set; }

        public static TimeShift CreateFrom(TimeShiftCreateViewModel viewModel)
        {
            return new TimeShift
            {
                Title = viewModel.Title,
                StartOn = viewModel.StartOn,
                EndOn = viewModel.EndOn,
                WorkPlaceId = viewModel.WorkPlaceId,
                CreatedOn = DateTime.Now
            };
        }
    }
}
