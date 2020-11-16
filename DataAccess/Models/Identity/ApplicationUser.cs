using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Όνομα")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string FirstName { get; set; }

        [Display(Name = "Επίθετο")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string LastName { get; set; }

        [Display(Name = "Μέλος από")]
        public DateTime? MemberSince { get; set; }

        public bool HasToChangePassword { get; set; }

        public bool IsEmployee { get; set; }

        public int? EmployeeId { get; set; }

        public ICollection<ApplicationUserTag> ApplicationUserTags { get; set; }

    }
}
