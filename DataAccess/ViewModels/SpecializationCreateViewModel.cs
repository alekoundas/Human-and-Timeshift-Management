using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using DataAccess.Models.Entity;

namespace DataAccess.ViewModels
{
    public class SpecializationCreateViewModel
    { 
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PayPerHour { get; set; }

        public static Specialization CreateFrom(SpecializationCreateViewModel viewModel)
        {
            return new Specialization()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                PayPerHour = viewModel.PayPerHour,
                CreatedOn = DateTime.Now

            };
        }
    }
}
