using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DataAccess.Models.Entity;

namespace DataAccess.ViewModels.RealWorkHours
{
    public class RealWorkHourCreateViewModel
    {
        [Display(Name = "Έναρξη")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό.")]
        public DateTime StartOn { get; set; }

        [Display(Name = "Λήξη")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό.")]
        //[RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((20)\d\d))$", ErrorMessage = "Invalid date formatasdfadfadfadfasdfasdfasdfasdfasdfasdfasdfasfwdeassdfsadfsdaf.")]
        public DateTime EndOn { get; set; }

        [Display(Name = "Υπάλληλοι")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό.")]
        public List<Employee> Employees { get; set; }

        [Display(Name = "Χρονοδιάγραμμα")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό.")]
        public int TimeShiftId { get; set; }
        public TimeShift TimeShift { get; set; }



        public static RealWorkHour CreateFrom(RealWorkHourCreateViewModel viewModel)
        {
            return new RealWorkHour
            {
                StartOn = viewModel.StartOn,
                EndOn = viewModel.EndOn,
                TimeShiftId = viewModel.TimeShiftId,
                CreatedOn = DateTime.Now
            };
        }
    }
}
