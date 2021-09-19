using DataAccess.Models.Security;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class LogEntityCreate
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Τίτλος")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Ελληνικος τίτλος")]
        public string Title_GR { get; set; }


        public bool IsActive { get; set; } = true;

        public static LogEntity CreateFrom(LogEntityCreate viewModel)
        {
            return new LogEntity()
            {
                Title = viewModel.Title,
                Title_GR = viewModel.Title_GR,
                IsActive = true,
                CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                CreatedOn = DateTime.Now
            };
        }
    }
}
