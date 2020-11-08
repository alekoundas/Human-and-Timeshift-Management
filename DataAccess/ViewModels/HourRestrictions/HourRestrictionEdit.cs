using DataAccess.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class HourRestrictionEdit
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public int Day { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [RegularExpression("^([0-9]{1}[0-9]{1}[0-9]{1}|[0-9]{1}[0-9]{1}):[0-5]{1}[0-9]{1}$", ErrorMessage = "Ο τύπος ώρας πρέπει να ειναι της μορφής ΗΗ:ΜΜ")]
        public string MaxTime { get; set; }

        public static IList<HourRestriction> CreateFrom(ICollection<HourRestrictionEdit> viewModels)
        {
            var returnList = new List<HourRestriction>();
            foreach (var viewModel in viewModels)
                returnList.Add(new HourRestriction
                {
                    Id = viewModel.Id,
                    Day = viewModel.Day,
                    MaxTicks = new TimeSpan(int.Parse(viewModel.MaxTime.Split(':')[0]),
                                           int.Parse(viewModel.MaxTime.Split(':')[1]),
                                           0).TotalSeconds,
                    CreatedOn = DateTime.Now
                });
            return returnList;
        }

        public static IList<HourRestrictionEdit> CreateFrom(ICollection<HourRestriction> models)
        {
            var returnList = new List<HourRestrictionEdit>();
            foreach (var model in models)
                returnList.Add(new HourRestrictionEdit
                {
                    Id = model.Id,
                    Day = model.Day,
                    MaxTime = GetTime(model.MaxTicks)
                });
            return returnList;
        }

        private static string GetTime(double seconds)
        {
            var hours = (seconds / 3600).ToString();
            var minutes = (seconds % 3600).ToString();

            if (hours.Length == 1)
                hours = "0" + hours;
            if (minutes.Length == 1)
                minutes = "0" + minutes;

            return hours + ":" + minutes;

        }
    }
}
