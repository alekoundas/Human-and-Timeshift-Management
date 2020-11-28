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

        [Display(Name = "Ημερομηνία γέννησης")]
        public DateTime BirthDay { get; set; }

        public bool HasToChangePassword { get; set; }

        public bool IsEmployee { get; set; }

        public int? EmployeeId { get; set; }

        public ICollection<ApplicationUserTag> ApplicationUserTags { get; set; }

        //Audit
        [Display(Name = "Δημηουργήθηκε απο")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string CreatedBy_Id { get; set; }
        public string CreatedBy_FullName { get; set; }

        [Display(Name = "Δημηουργήθηκε στις")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public DateTime CreatedOn { get; set; }

    }
}
