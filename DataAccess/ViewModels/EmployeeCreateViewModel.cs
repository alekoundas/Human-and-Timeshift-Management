using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DataAccess.Models.Entity;

namespace DataAccess.ViewModels
{
    public class EmployeeCreateViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string ErpCode { get; set; }
        public string Afm { get; set; }
        public string SocialSecurityNumber { get; set; }

        [Required]
        public int SpecializationId { get; set; }

        public int? CompanyId { get; set; }

        public ICollection<Contact> Contacts { get; set; }


        public static Employee CreateFrom(EmployeeCreateViewModel viewModel)
        {
            return new Employee()
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                DateOfBirth = viewModel.DateOfBirth,
                Email = viewModel.Email,
                ErpCode = viewModel.ErpCode,
                Afm = viewModel.Afm,
                SocialSecurityNumber = viewModel.SocialSecurityNumber,
                SpecializationId = viewModel.SpecializationId,
                CompanyId = viewModel.CompanyId,
                Contacts = viewModel.Contacts,
                CreatedOn= DateTime.Now
            };
        }
    }
}
