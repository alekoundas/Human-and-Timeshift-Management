using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DataAccess.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace DataAccess.ViewModels.RealWorkHours
{
    public class RealWorkHourCreateViewModel
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



        public static RealWorkHour CreateFrom(RealWorkHourCreateViewModel viewModel)
        {
            return new RealWorkHour
            {
                StartOn = viewModel.StartOn,
                EndOn = viewModel.EndOn,
                TimeShiftId = viewModel.TimeShiftId,
                Comments=viewModel.Comments,
                CreatedOn = DateTime.Now
            };
        }
    }
}
