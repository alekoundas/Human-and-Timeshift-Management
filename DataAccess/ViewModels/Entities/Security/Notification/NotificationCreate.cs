using DataAccess.Models.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class NotificationCreate
    {
        [Display(Name = "Τίτλος*")]
        [Required(ErrorMessage = "To παιδίο ειναι υποχρεωτικό")]
        public string Title { get; set; }

        [Display(Name = "Περιγραφή*")]
        [Required(ErrorMessage = "To παιδίο ειναι υποχρεωτικό")]
        public string Description { get; set; }

        public List<string> UserIds { get; set; }

        [Display(Name = "Αποστολή σε όλους τους χρήστες*")]
        public bool IsSendEveryone { get; set; }


        public static Notification CreateFrom(NotificationCreate viewModel) =>
            new Notification()
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                IsSeen = false,
                CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                CreatedOn = DateTime.Now
            };

    }
}
