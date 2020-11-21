using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.Entity
{
    public class Contract : BaseEntityIsActive
    {
        [Required]
        [Display(Name = "Τίτλος")]
        public string Title { get; set; }

        [Display(Name = "Ώρες ανα εβδομάδα")]
        public int HoursPerWeek { get; set; }

        [Display(Name = "Ώρες ανα ημέρα")]
        public int HoursPerDay { get; set; }

        [Display(Name = "Εργάσιμες μέρες εβδομάδας")]
        public int WorkingDaysPerWeek { get; set; }

        [Display(Name = "Μέρες ρεπού εβδομάδας")]
        public int DayOfDaysPerWeek { get; set; }

        [Display(Name = "Έναρξη σύμβασης")]
        public DateTime StartOn { get; set; }

        [Display(Name = "Λήψη σύμβασης")]
        public DateTime? EndOn { get; set; }

        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossSalaryPerHour { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetSalaryPerHour { get; set; }

        [Required]
        [Display(Name = "Ιδιότητα σύμβασης")]
        public int ContractMembershipId { get; set; }
        public ContractMembership ContractMembership { get; set; }

        [Required]
        [Display(Name = "Τύπος σύμβασης")]
        public int ContractTypeId { get; set; }
        public ContractType ContractType { get; set; }
    }
}
