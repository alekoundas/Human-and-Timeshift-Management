using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAccess.Models.Entity;

namespace DataAccess.ViewModels.HourRestrictions
{
    public class HourRestrictionCreate
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public int Day { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public int MaxHours { get; set; }

        public static ICollection<HourRestriction> CreateFrom(ICollection<HourRestrictionCreate> viewModels)
        {
            var returnList = new List<HourRestriction>();

            foreach (var viewModel in viewModels)
                returnList.Add(new HourRestriction
                {
                    Day = viewModel.Day,
                    MaxHours = viewModel.MaxHours,
                    CreatedOn = DateTime.Now
                });
            return returnList;
        }
    }
}
