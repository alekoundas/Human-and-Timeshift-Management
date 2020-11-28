using DataAccess.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class RealWorkHourCreate
    {
        [Display(Name = "Έναρξη")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό.")]
        public DateTime StartOn { get; set; }

        [Display(Name = "Λήξη")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό.")]
        public DateTime EndOn { get; set; }

        [Display(Name = "Σχόλια")]
        public string Comments { get; set; }

        [Display(Name = "Υπάλληλοι")]
        public List<int> Employees { get; set; }

        [Display(Name = "Χρονοδιάγραμμα")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό.")]
        public int TimeShiftId { get; set; }
        public TimeShift TimeShift { get; set; }



        public static RealWorkHour CreateFrom(RealWorkHourCreate viewModel)
        {
            return new RealWorkHour
            {
                StartOn = viewModel.StartOn,
                EndOn = viewModel.EndOn,
                TimeShiftId = viewModel.TimeShiftId,
                Comments = viewModel.Comments,
                CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                CreatedOn = DateTime.Now
            };
        }
    }
}
