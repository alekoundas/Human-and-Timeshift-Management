using DataAccess.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class EmployeeCreate
    {
        [Display(Name = "Όνομα")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string FirstName { get; set; }

        [Display(Name = "Επίθετο")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string LastName { get; set; }

        [Display(Name = "Ημερομηνίαα Γέννησης")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "ΑΦΜ")]
        public string Afm { get; set; }

        [Display(Name = "Αριθμός Ταυτότητας")]
        public string SocialSecurityNumber { get; set; }

        [Display(Name = "Κωδικός Erp")]
        public string ErpCode { get; set; }

        public string Email { get; set; }
        [Display(Name = "Διεύθυνση")]
        public string Address { get; set; }


        [Display(Name = "Ειδικότητα")]
        public int? SpecializationId { get; set; }
        public Specialization Specialization { get; set; }


        [Display(Name = "Εταιρία")]
        public int? CompanyId { get; set; }
        public Company Company { get; set; }


        [Display(Name = "Επαφές")]
        public ICollection<Contact> Contacts { get; set; }


        public static Employee CreateFrom(EmployeeCreate viewModel)
        {
            return new Employee()
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                DateOfBirth = viewModel.DateOfBirth,
                Email = viewModel.Email,
                ErpCode = viewModel.ErpCode,
                Afm = viewModel.Afm,
                SocialSecurityNumber = viewModel.SocialSecurityNumber,
                SpecializationId = viewModel.SpecializationId,
                CompanyId = viewModel.CompanyId,
                Contacts = viewModel.Contacts,
                Address = viewModel.Address,
                IsActive = true,
                CreatedOn = DateTime.Now
            };
        }
    }
}
