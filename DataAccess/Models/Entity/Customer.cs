using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using DataAccess.Models.Entity;

namespace DataAccess.Models.Entity
{
    public class Customer : BaseEntity
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Όνομα")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Επίθετο")]
        public string LastName { get; set; }

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

        public ICollection<WorkPlace> WorkPlaces { get; set; }


        [NotMapped]
        public string FullName { get => FirstName + LastName; }
    }
}
