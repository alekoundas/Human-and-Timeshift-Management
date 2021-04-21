using DataAccess.Models.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class RealWorkHourTimeClock
    {

        [Display(Name = "Χρονοδιάγραμμα")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό.")]
        public int TimeShiftId { get; set; }

        //[Display(Name = "Χρήστης")]
        //[Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό.")]
        //public string UserId { get; set; }

        [Display(Name = "CurrentDate")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό.")]
        public DateTime CurrentDate { get; set; }

        [Display(Name = "Σχόλια")]
        public string Comments { get; set; }

        public int EmployeeId { get; set; }




        public static RealWorkHour ClockIn(RealWorkHourTimeClock viewModel)
        {
            return new RealWorkHour
            {
                StartOn = viewModel.CurrentDate,
                EndOn = null,
                TimeShiftId = viewModel.TimeShiftId,
                EmployeeId = viewModel.EmployeeId,
                Comments = viewModel.Comments,
                IsInProgress = true,
                CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                CreatedOn = DateTime.Now
            };
        }
    }
}
