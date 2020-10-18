using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAccess.Models.Entity;

namespace DataAccess.ViewModels.HourRestrictions
{
    public class HourRestrictionEdit
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public int Day { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public int MaxHours { get; set; }

        public static ICollection<HourRestriction> CreateFrom(ICollection<HourRestrictionEdit> viewModels)
        {
            var returnList = new List<HourRestriction>();
            foreach (var viewModel in viewModels)
                returnList.Add(new HourRestriction
                {
                    Id = viewModel.Id,
                    Day = viewModel.Day,
                    MaxHours = viewModel.MaxHours,
                    CreatedOn = DateTime.Now
                });
            return returnList;
        }
        public static ICollection<HourRestrictionEdit> CreateFrom(ICollection<HourRestriction> viewModels)
        {
            var returnList = new List<HourRestrictionEdit>();
            foreach (var viewModel in viewModels)
                returnList.Add(new HourRestrictionEdit
                {
                    Id = viewModel.Id,
                    Day = viewModel.Day,
                    MaxHours = viewModel.MaxHours
                });
            return returnList;
        }
    }
}
