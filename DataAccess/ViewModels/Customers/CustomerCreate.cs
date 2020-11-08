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
        public string ΙdentifyingΝame { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "ΑΦΜ")]
        public string AFM { get; set; }

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
        public int CompanyId { get; set; }
        public Company Company { get; set; }


        [Display(Name = "Επαφές")]
        public ICollection<Contact> Contacts { get; set; }


        public static Customer CreateFrom(CustomerCreate viewModel)
        {
            return new Customer()
            {
                ΙdentifyingΝame = viewModel.ΙdentifyingΝame,
                AFM = viewModel.AFM,
                Address = viewModel.Address,
                PostalCode = viewModel.PostalCode,
                DOY = viewModel.DOY,
                Description = viewModel.Description,
                CompanyId = viewModel.CompanyId,
                Profession = viewModel.Profession,
                IsActive = true,
                CreatedOn = DateTime.Now
            };
        }
    }
}
