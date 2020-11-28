using DataAccess.DataAnnotation.Unique;
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

        [Display(Name = "ΑΦΜ")]
        [EmployeeValidateUnique]
        public string VatNumber { get; set; }

        [Display(Name = "Αριθμός Ταυτότητας")]
        public string SocialSecurityNumber { get; set; }

        [Display(Name = "Κωδικός Erp")]
        public string ErpCode { get; set; }

        public string Email { get; set; }
        [Display(Name = "Διεύθυνση")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Ημερομηνία πρόσληψης")]
        public DateTime HireDate { get; set; }

        [Display(Name = "Έναρξη σύμβασης")]
        public DateTime? ContractStartOn { get; set; }

        [Display(Name = "Λήξη σύμβασης")]
        public DateTime? ContractEndOn { get; set; }


        [Display(Name = "Ειδικότητα")]
        public int? SpecializationId { get; set; }
        public Specialization Specialization { get; set; }

        [Display(Name = "Εταιρία")]
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        [Display(Name = "Σύμβαση")]
        public int? ContractId { get; set; }
        public Contract Contract { get; set; }

        [Display(Name = "Επαφές")]
        public ICollection<Contact> Contacts { get; set; }


        public static Employee CreateFrom(EmployeeCreate viewModel) =>
            new Employee()
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                ErpCode = viewModel.ErpCode,
                VatNumber = viewModel.VatNumber,
                SocialSecurityNumber = viewModel.SocialSecurityNumber,
                SpecializationId = viewModel.SpecializationId,
                CompanyId = viewModel.CompanyId,
                ContractId = viewModel.ContractId,
                Address = viewModel.Address,
                HireDate = viewModel.HireDate,
                ContractStartOn = viewModel.ContractStartOn,
                ContractEndOn = viewModel.ContractEndOn,
                IsActive = true,
                CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                CreatedOn = DateTime.Now
            };
    }
}
