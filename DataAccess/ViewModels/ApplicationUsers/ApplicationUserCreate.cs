using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models.Entity;
using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity;
using DataAccess.Models.Identity;

namespace DataAccess.ViewModels.ApplicationUsers
{
    public class ApplicationUserCreate: IdentityUser
    {
        [Display(Name = "Όνομα")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string FirstName { get; set; }

        [Display(Name = "Επίθετο")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string LastName { get; set; }

        [Display(Name = "Ημερομηνία Γέννησης")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Φύλο")]
        public bool Gender { get; set; }
        public bool IsEmployee { get; set; }

        public int? EmployeeId { get; set; }


        public static ApplicationUser CreateFrom(ApplicationUserCreate viewModel)
        {
            return new ApplicationUser()
            {
                UserName=viewModel.UserName,
                Email = viewModel.Email,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                IsEmployee = viewModel.IsEmployee,
                Gender = viewModel.Gender,
                DateOfBirth = viewModel.DateOfBirth,
                HasToChangePassword=true
            };
        }
    }
}
