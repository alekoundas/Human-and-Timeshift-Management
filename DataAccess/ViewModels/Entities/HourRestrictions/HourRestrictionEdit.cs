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


        //Audit
        [Display(Name = "Δημηουργήθηκε απο")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string CreatedBy_Id { get; set; }
        public string CreatedBy_FullName { get; set; }

        [Display(Name = "Δημηουργήθηκε στις")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public DateTime CreatedOn { get; set; }

        public static IList<HourRestriction> CreateFrom(ICollection<HourRestrictionEdit> viewModels)
        {
            var returnList = new List<HourRestriction>();
            foreach (var viewModel in viewModels)
                returnList.Add(new HourRestriction
                {
                    Id = viewModel.Id,
                    Day = viewModel.Day,
                    MaxTicks = GetSeconds(viewModel.MaxTime.Split(':')[0],
                                           viewModel.MaxTime.Split(':')[1]),
                    CreatedBy_FullName = viewModel.CreatedBy_FullName,
                    CreatedBy_Id = viewModel.CreatedBy_Id,
                    CreatedOn = viewModel.CreatedOn
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
                    MaxTime = GetTime(model.MaxTicks),
                    CreatedBy_FullName = model.CreatedBy_FullName,
                    CreatedBy_Id = model.CreatedBy_Id,
                    CreatedOn = model.CreatedOn
                });
            return returnList;
        }

        private static string GetTime(double seconds)
        {
            var hours = (seconds / 3600).ToString();
            seconds %= 3600;
            var minutes = (seconds / 60).ToString();

            if (hours.Contains(","))
                hours = hours.Split(',')[0];

            if (hours.Length == 1)
                hours = "0" + hours;

            if (minutes.Length == 1)
                minutes = "0" + minutes;

            return hours + ":" + minutes;
        }

        private static double GetSeconds(string hours, string minutes)
        {
            var totalSeconds = int.Parse(hours) * 3600;
            totalSeconds += int.Parse(minutes) * 60;
            return totalSeconds;
        }
    }
}
