using DataAccess.DataAnnotation.Unique;
using DataAccess.Models.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class ContractMembershipCreate
    {
        [Required]
        [ContractMembershipValidateUnique]
        [Display(Name = "Όνομα")]
        public string Name { get; set; }

        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }


        public static ContractMembership CreateFrom(ContractMembershipCreate viewModel)
        {
            return new ContractMembership()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                IsActive = true,
                CreatedOn = DateTime.Now
            };
        }
    }
}
