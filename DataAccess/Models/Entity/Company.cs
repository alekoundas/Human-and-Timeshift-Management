using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Models.Entity
{
    public class Company : BaseEntity
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "ΑΦΜ")]
        public string Afm { get; set; }
        [Display(Name = "Περιγραφή")]

        public string Description { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Τίτλος")]
        public string Title { get; set; }

        public ICollection<Employee> Employees { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
}
