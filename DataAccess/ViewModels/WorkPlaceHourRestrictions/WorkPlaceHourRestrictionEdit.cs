using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.HourRestrictions;

namespace DataAccess.ViewModels.WorkPlaceHourRestrictions
{
    public class WorkPlaceHourRestrictionEdit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Μήνας")]
        public int Month { get; set; }
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Έτος")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Πόστο")]
        public int WorkPlaceId { get; set; }
        public WorkPlace WorkPlace { get; set; }

        [Display(Name = "Περιορσμός μέγιστης εισαγωγής π.βαρδιών")]
        public ICollection<HourRestrictionEdit> HourRestrictions { get; set; }

        public int DaysInMonth
        {
            get
            {
                return Year == 0 || Month == 0 ?
                     0
                :
                    DateTime.DaysInMonth(Year, Month);

            }
            set { }
        }

        public static WorkPlaceHourRestriction CreateFrom(WorkPlaceHourRestrictionEdit viewModel)
        {
            return new WorkPlaceHourRestriction
            {
                Id = viewModel.Id,
                Month = viewModel.Month,
                Year = viewModel.Year,
                WorkPlaceId = viewModel.WorkPlaceId,
                HourRestrictions = HourRestrictionEdit.CreateFrom(viewModel.HourRestrictions),
                CreatedOn = DateTime.Now
            };
        }

        public static WorkPlaceHourRestrictionEdit CreateFrom(WorkPlaceHourRestriction viewModel)
        {
            return new WorkPlaceHourRestrictionEdit
            {
                Id = viewModel.Id,
                Month = viewModel.Month,
                Year = viewModel.Year,
                WorkPlaceId = viewModel.WorkPlaceId,
                HourRestrictions = HourRestrictionEdit.CreateFrom(viewModel.HourRestrictions)
            };
        }
    }
}
