using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Business.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.ViewModels.User
{
    public class UserViewModel : ViewModel
    {

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Selected Role")]
        public int RoleId { get; set; }
        public IList<IdentityRole> Roles { get; set; }

    }
}
