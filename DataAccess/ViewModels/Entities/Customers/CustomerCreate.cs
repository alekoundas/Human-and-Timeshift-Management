using DataAccess.DataAnnotation.Unique;
using DataAccess.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class CustomerCreate
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Επωνυμία")]
        public string IdentifyingName { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "ΑΦΜ")]
        [CustomerValidateUnique]
        public string VatNumber { get; set; }

        [Display(Name = "Επάγγελμα")]
        public string Profession { get; set; }

        [Display(Name = "Διεύθυνση")]
        public string Address { get; set; }

        [Display(Name = "ΤΚ")]
        public string PostalCode { get; set; }

        [Display(Name = "ΔΟΥ")]
        public string DOY { get; set; }

        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }

        [Display(Name = "Εταιρία")]
        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        [Display(Name = "Επαφές")]
        public ICollection<Contact> Contacts { get; set; }


        public static Customer CreateFrom(CustomerCreate viewModel)
        {
            return new Customer()
            {
                IdentifyingName = viewModel.IdentifyingName,
                VatNumber = viewModel.VatNumber,
                Address = viewModel.Address,
                PostalCode = viewModel.PostalCode,
                DOY = viewModel.DOY,
                Description = viewModel.Description,
                CompanyId = viewModel.CompanyId,
                Profession = viewModel.Profession,
                IsActive = true,
                CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                CreatedOn = DateTime.Now
            };
        }
    }
}
