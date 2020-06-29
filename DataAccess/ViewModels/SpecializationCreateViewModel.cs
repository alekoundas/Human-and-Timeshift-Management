using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DataAccess.Models.Entity;

namespace DataAccess.ViewModels
{
    public class SpecializationCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public static Specialization CreateFrom(SpecializationCreateViewModel viewModel)
        {
            return new Specialization()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                CreatedOn = DateTime.Now

            };
        }
    }
}
