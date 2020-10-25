using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.HourRestrictions;

namespace DataAccess.ViewModels.WorkPlaceHourRestrictions
{
    public class WorkPlaceHourRestrictionCreate
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Μήνας")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Έτος")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Πόστο")]
        public int WorkPlaceId { get; set; }

        [Display(Name = "Περιορσμός μέγιστης εισαγωγής π.βαρδιών")]
        public IList<HourRestrictionCreate> HourRestrictions { get; set; }

        public static WorkPlaceHourRestriction CreateFrom(WorkPlaceHourRestrictionCreate viewModel)
        {
            return new WorkPlaceHourRestriction
            {
                Month = viewModel.Month,
                Year = viewModel.Year,
                WorkPlaceId = viewModel.WorkPlaceId,
                HourRestrictions = HourRestrictionCreate.CreateFrom(viewModel.HourRestrictions),
                CreatedOn = DateTime.Now
            };
        }
    }
}
