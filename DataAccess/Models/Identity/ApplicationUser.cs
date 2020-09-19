using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models.Entity;
using Microsoft.AspNetCore.Identity;

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

        [Display(Name = "Ημερομηνία Γέννησης")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Φύλο")]
        public bool Gender { get; set; }

        public ICollection<ApplicationUserRole> EmployeeWorkPlaces { get; set; }

    }
}
