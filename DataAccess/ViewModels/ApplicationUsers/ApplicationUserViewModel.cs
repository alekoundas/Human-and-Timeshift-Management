using Business.ViewModels;
using DataAccess.Models.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class ApplicationUserViewModel : ViewModel
    {

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Selected Role")]
        public int RoleId { get; set; }
        public IList<ApplicationRole> Roles { get; set; }

        public string UserName { get; set; }


        public static ApplicationUserViewModel CreateFrom(ApplicationUser model)
        {
            return new ApplicationUserViewModel
            {
                Email = model.Email,
                UserName = model.UserName

            };
        }
    }
}
