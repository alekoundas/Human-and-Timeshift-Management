using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class ApplicationUserCreate : IdentityUser
    {
        [Display(Name = "Όνομα")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string FirstName { get; set; }

        [Display(Name = "Επίθετο")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string LastName { get; set; }

        [Display(Name = "Όνομα χρήστη")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string UserName { get; set; }

        [Display(Name = "Μέλος από")]
        public DateTime? MemberSince { get; set; }

        [Display(Name = "Ημερομηνία γέννησης")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public DateTime BirthDay { get; set; }

        public bool HasToChangePassword { get; set; }

        [Display(Name = "Είναι υπάλληλος;")]
        public bool IsEmployee { get; set; }

        [Display(Name = "Yπάλληλος")]
        public int? EmployeeId { get; set; }





        public static ApplicationUser CreateFrom(ApplicationUserCreate viewModel)
        {
            return new ApplicationUser()
            {
                UserName = viewModel.UserName,
                Email = viewModel.Email,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                IsEmployee = viewModel.IsEmployee,
                EmployeeId = viewModel.EmployeeId,
                BirthDay = viewModel.BirthDay,
                HasToChangePassword = true,
                CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                CreatedOn = DateTime.Now
            };
        }
    }
}
