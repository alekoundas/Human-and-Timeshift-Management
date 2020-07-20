using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAccess.Models.Entity;

namespace DataAccess.ViewModels
{
    public class TimeShiftCreateViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public int WorkPlaceId { get; set; }

        public static TimeShift CreateFrom(TimeShiftCreateViewModel viewModel)
        {
            return new TimeShift
            {
                Title = viewModel.Title,
                Month = viewModel.Month,
                Year = viewModel.Year,
                WorkPlaceId = viewModel.WorkPlaceId,
                CreatedOn = DateTime.Now
            };
        }
    }
}
