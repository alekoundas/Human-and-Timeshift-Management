using DataAccess.DataAnnotation.Unique;
using DataAccess.Models.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class ContractTypeCreate
    {
        [ContractTypeValidateUnique]
        [Display(Name = "Όνομα")]
        public string Name { get; set; }

        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }


        public static ContractType CreateFrom(ContractTypeCreate viewModel) =>
            new ContractType()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                IsActive = true,
                CreatedOn = DateTime.Now
            };
    }
}
