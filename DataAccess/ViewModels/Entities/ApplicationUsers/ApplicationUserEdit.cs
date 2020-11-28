using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class ApplicationUserEdit : IdentityUser
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string Id { get; set; }

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


        [Display(Name = "Είναι υπάλληλος;")]
        public bool IsEmployee { get; set; }

        [Display(Name = "Υπάλληλος")]
        public int? EmployeeId { get; set; }

        [Display(Name = "Υπάλληλος")]
        public string EmployeeOption { get; set; }

        public List<WorkPlaceRoleValues> WorkPlaceRoles { get; set; }


        //Audit
        [Display(Name = "Δημηουργήθηκε απο")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string CreatedBy_Id { get; set; }
        public string CreatedBy_FullName { get; set; }

        [Display(Name = "Δημηουργήθηκε στις")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public DateTime CreatedOn { get; set; }


        public static ApplicationUserEdit CreateFrom(ApplicationUser user) =>
            new ApplicationUserEdit()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsEmployee = user.IsEmployee,
                EmployeeId = user.EmployeeId,
                BirthDay = user.BirthDay,
                CreatedBy_FullName = user.CreatedBy_FullName,
                CreatedBy_Id = user.CreatedBy_Id,
                CreatedOn = user.CreatedOn

            };
    }

    public class WorkPlaceRoleValues
    {
        public string WorkPlaceId { get; set; }
        public string Name { get; set; }
    }

}
