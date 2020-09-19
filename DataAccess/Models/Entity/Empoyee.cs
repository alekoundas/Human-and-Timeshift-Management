using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Models.Entity;

namespace DataAccess.Models.Entity
{
    public class Employee : BaseEntity
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
        public string  SocialSecurityNumber { get; set; }

        [Display(Name = "Κωδικός Erp")]
        public string ErpCode { get; set; }

        public string Email { get; set; }
        [Display(Name = "Διεύθυνση")]
        public string Address { get; set; }


        [Display(Name = "Ειδικότητα")]
        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; }


        [Display(Name = "Εταιρία")]
        public int? CompanyId { get; set; }
        public Company Company { get; set; }


        [Display(Name = "Επαφές")]
        public ICollection<Contact> Contacts{ get; set; }

        [Display(Name = "Πόστα")]
        public ICollection<WorkHour> WorkHours { get; set; }

        [Display(Name = "Πραγμαατικές Βάρδιες")]
        public ICollection<RealWorkHour> RealWorkHours { get; set; }

        public ICollection<EmployeeWorkPlace> EmployeeWorkPlaces { get; set; }
        public ICollection<Leave> Leaves { get; set; }

        [NotMapped]
        public string FullName { get { return FirstName + " - " + LastName; } }
    }
}
