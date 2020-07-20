using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DataAccess.Models.Entity;

namespace DataAccess.ViewModels.RealWorkHours
{
    public class RealWorkHourCreateViewModel
    {
        [Required(ErrorMessage = "Birth Date is required.")]
        public DateTime StartOn { get; set; }

        [Required(ErrorMessage = "Birth Date is required.")]
        //[RegularExpression(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((20)\d\d))$", ErrorMessage = "Invalid date formatasdfadfadfadfasdfasdfasdfasdfasdfasdfasdfasfwdeassdfsadfsdaf.")]
        public DateTime EndOn { get; set; }

        [Required(ErrorMessage = "mana")]
        public int TimeShiftId { get; set; }
        public TimeShift TimeShift { get; set; }

        public List<Employee> Employees { get; set; }

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
