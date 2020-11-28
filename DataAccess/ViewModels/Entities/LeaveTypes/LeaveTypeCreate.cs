using DataAccess.DataAnnotation.Unique;
using DataAccess.Models.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class LeaveTypeCreate
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Είδος άδειας")]
        [LeaveTypeValidateUnique]
        public string Name { get; set; }

        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }


        public static LeaveType CreateFrom(LeaveTypeCreate viewModel)
        {
            return new LeaveType()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                IsActive = true,
                CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                CreatedOn = DateTime.Now

            };
        }
    }
}
