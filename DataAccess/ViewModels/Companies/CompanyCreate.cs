using DataAccess.Models.Entity;
using System;

namespace DataAccess.ViewModels
{
    public class CompanyCreate
    {
        public string Afm { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }


        public static Company CreateFrom(CompanyCreate viewModel)
        {
            return new Company()
            {
                Title = viewModel.Title,
                Afm = viewModel.Afm,
                Description = viewModel.Description,
                IsActive = true,
                CreatedOn = DateTime.Now
            };
        }
    }
}
