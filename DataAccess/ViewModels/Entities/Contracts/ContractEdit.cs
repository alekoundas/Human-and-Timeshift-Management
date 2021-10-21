using DataAccess.Models.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class ContractEdit : ContractCreate
    {

        [Required]
        [Display(Name = "Τίτλος")]
        public string Title { get; set; }

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy_FullName { get; set; }
        public string CreatedBy_Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public ContractMembership ContractMembership { get; set; }
        public ContractType ContractType { get; set; }


        public static Contract CreateFrom(ContractEdit viewModel)
        {
            return new Contract()
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                HoursPerWeek = viewModel.HoursPerWeek,
                WorkingDaysPerWeek = viewModel.WorkingDaysPerWeek,
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

        public static ContractEdit CreateFrom(Contract viewModel)
        {
            return new ContractEdit()
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                HoursPerWeek = viewModel.HoursPerWeek,
                WorkingDaysPerWeek = viewModel.WorkingDaysPerWeek,
                HoursPerDay = viewModel.HoursPerDay,
                DayOfDaysPerWeek = viewModel.DayOfDaysPerWeek,
                Description = viewModel.Description,
                GrossSalaryPerHour = viewModel.GrossSalaryPerHour,
                NetSalaryPerHour = viewModel.NetSalaryPerHour,
                ContractMembershipId = viewModel.ContractMembershipId,
                ContractTypeId = viewModel.ContractTypeId,
                ContractMembership = viewModel.ContractMembership,
                ContractType = viewModel.ContractType,
                IsActive = true,
                CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                CreatedOn = DateTime.Now
            };
        }
    }
}
