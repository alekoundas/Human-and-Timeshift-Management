﻿using DataAccess.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class HourRestrictionCreate
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public int Day { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [RegularExpression("^([0-9]{1}[0-9]{1}[0-9]{1}|[0-9]{1}[0-9]{1}):[0-5]{1}[0-9]{1}$", ErrorMessage = "Ο τύπος ώρας πρέπει να ειναι της μορφής ΗΗ:ΜΜ")]
        public string MaxTime { get; set; }

        public static IList<HourRestriction> CreateFrom(IList<HourRestrictionCreate> viewModels)
        {
            var returnList = new List<HourRestriction>();

            foreach (var viewModel in viewModels)
                returnList.Add(new HourRestriction
                {
                    Day = viewModel.Day,
                    MaxTicks = new TimeSpan(int.Parse(viewModel.MaxTime.Split(':')[0]),
                                           int.Parse(viewModel.MaxTime.Split(':')[1]),
                                           0).TotalSeconds,
                    IsActive = true,
                    CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                    CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                    CreatedOn = DateTime.Now

                });
            return returnList;
        }
    }
}