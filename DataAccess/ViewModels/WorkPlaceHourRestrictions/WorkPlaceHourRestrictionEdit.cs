using DataAccess.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
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
        public IList<HourRestrictionEdit> HourRestrictions { get; set; }

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

        public static WorkPlaceHourRestrictionEdit CreateFrom(WorkPlaceHourRestriction model)
        {
            return new WorkPlaceHourRestrictionEdit
            {
                Id = model.Id,
                Month = model.Month,
                Year = model.Year,
                WorkPlaceId = model.WorkPlaceId,
                WorkPlace = model.WorkPlace,
                HourRestrictions = HourRestrictionEdit.CreateFrom(model.HourRestrictions)
            };
        }
    }
}
