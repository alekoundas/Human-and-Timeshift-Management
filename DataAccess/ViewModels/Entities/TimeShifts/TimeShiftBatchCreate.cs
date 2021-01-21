using DataAccess.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DataAccess.ViewModels
{
    public class TimeShiftBatchCreate
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Πόστο")]
        public List<int> WorkPlaceIds { get; set; }

        public static List<TimeShift> CreateFrom(TimeShiftBatchCreate viewModel)
        {
            var items = new List<TimeShift>();
            foreach (var workPlaceId in viewModel.WorkPlaceIds)
            {
                items.Add(new TimeShift
                {
                    Title = viewModel.Year + " " + CultureInfo.CreateSpecificCulture("el-GR").DateTimeFormat.GetMonthName(viewModel.Month),
                    Month = viewModel.Month,
                    Year = viewModel.Year,
                    WorkPlaceId = workPlaceId,
                    IsActive = true,
                    CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                    CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                    CreatedOn = DateTime.Now
                });
            }
            return items;
        }
    }
}
