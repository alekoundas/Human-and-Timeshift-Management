using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models.Entity;

namespace DataAccess.ViewModels
{
    public class CustomerCreateViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string AFM { get; set; }
        public string Description { get; set; }
        public int? CompanyId { get; set; }


        public ICollection<Contact> Contacts { get; set; }

        public static Customer CreateFrom(CustomerCreateViewModel viewModel)
        {
            return new Customer()
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                DateOfBirth = viewModel.DateOfBirth,
                AFM = viewModel.AFM,
                Description = viewModel.Description,
                CompanyId = (int)viewModel.CompanyId,
                CreatedOn = DateTime.Now
            };
        }
    }
}
