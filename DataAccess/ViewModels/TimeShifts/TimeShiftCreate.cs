using DataAccess.DataAnnotation.Unique;
using DataAccess.Models.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class TimeShiftCreate
    {
        [Display(Name = "Τίτλος")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [TimeShiftValidateUnique("Month")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Πόστο")]
        [TimeShiftValidateUnique("WorkPlaceId")]
        public int WorkPlaceId { get; set; }

        public static TimeShift CreateFrom(TimeShiftCreate viewModel)
        {
            return new TimeShift
            {
                Title = viewModel.Title,
                Month = viewModel.Month,
                Year = viewModel.Year,
                WorkPlaceId = viewModel.WorkPlaceId,
                IsActive = true,
                CreatedOn = DateTime.Now
            };
        }
    }
}
