using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models.Entity;

namespace DataAccess.ViewModels
{
    public class CompanyCreateViewModel
    {
        public string Afm { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }


        public static Company CreateFrom(CompanyCreateViewModel viewModel)
        {
            return new Company()
            {
                Title = viewModel.Title,
                Afm = viewModel.Afm,
                Description = viewModel.Description,
                CreatedOn = DateTime.Now
            };
        }
    }
}
