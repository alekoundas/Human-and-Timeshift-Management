using DataAccess.DataAnnotation.Unique;
using DataAccess.Models.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.ViewModels
{
    public class ContractCreate
    {
        [Required]
        [ContractValidateUnique]
        [Display(Name = "Τίτλος")]
        public string Title { get; set; }

        [Display(Name = "Ώρες ανα εβδομάδα")]
        public int HoursPerWeek { get; set; }

        [Display(Name = "Ώρες ανα ημέρα")]
        public decimal HoursPerDay { get; set; }

        [Display(Name = "Εργάσιμες μέρες εβδομάδας")]
        public int WorkingDaysPerWeek { get; set; }

        [Display(Name = "Μέρες ρεπού εβδομάδας")]
        public int DayOfDaysPerWeek { get; set; }

        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Ιδιότητα σύμβασης")]
        public int ContractMembershipId { get; set; }

        [Required]
        [Display(Name = "Τύπος σύμβασης")]
        public int ContractTypeId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Καθαρός μισθός ανα ώρα")]
        public decimal GrossSalaryPerHour { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Μικτός μισθός ανα ώρα")]
        public decimal NetSalaryPerHour { get; set; }


        public static Contract CreateFrom(ContractCreate viewModel)
        {
            return new Contract()
            {
                Title = viewModel.Title,
                HoursPerWeek = viewModel.HoursPerWeek,
                HoursPerDay = viewModel.HoursPerDay,
                DayOfDaysPerWeek = viewModel.DayOfDaysPerWeek,
                Description = viewModel.Description,
                GrossSalaryPerHour = viewModel.GrossSalaryPerHour,
                NetSalaryPerHour = viewModel.NetSalaryPerHour,
                ContractMembershipId = viewModel.ContractMembershipId,
                ContractTypeId = viewModel.ContractTypeId,
                IsActive = true,
                CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                CreatedOn = DateTime.Now
            };
        }
    }
}
