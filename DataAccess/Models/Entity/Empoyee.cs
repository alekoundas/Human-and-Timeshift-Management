using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.Entity
{
    public class Employee : BaseEntityIsActive
    {
        [Display(Name = "Όνομα")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string FirstName { get; set; }

        [Display(Name = "Επίθετο")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string LastName { get; set; }

        [Display(Name = "Ημερομηνία Γέννησης")]
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

        [Required]
        [Display(Name = "Ημερομηνία πρόσληψης")]
        public DateTime HireDate { get; set; }


        [Display(Name = "Ειδικότητα")]
        public int? SpecializationId { get; set; }
        public Specialization Specialization { get; set; }


        [Display(Name = "Εταιρία")]
        public int? CompanyId { get; set; }
        public Company Company { get; set; }


        [Display(Name = "Επαφές")]
        public ICollection<Contact> Contacts { get; set; }

        [Display(Name = "Πόστα")]
        public ICollection<WorkHour> WorkHours { get; set; }

        [Display(Name = "Πραγμαατικές Βάρδιες")]
        public ICollection<RealWorkHour> RealWorkHours { get; set; }

        public ICollection<EmployeeWorkPlace> EmployeeWorkPlaces { get; set; }
        public ICollection<Leave> Leaves { get; set; }

        public int? ContractId { get; set; }
        public Contract Contract { get; set; }

        [NotMapped]
        public string FullName { get { return FirstName + " - " + LastName; } }
    }
}
