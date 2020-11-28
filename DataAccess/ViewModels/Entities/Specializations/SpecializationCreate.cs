using DataAccess.DataAnnotation.Unique;
using DataAccess.Models.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.ViewModels
{
    public class SpecializationCreate
    {
        [Required]
        [SpecializationValidateUnique]
        public string Name { get; set; }
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PayPerHour { get; set; }

        public static Specialization CreateFrom(SpecializationCreate viewModel)
        {
            return new Specialization()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                PayPerHour = viewModel.PayPerHour,
                IsActive = true,
                CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                CreatedOn = DateTime.Now

            };
        }
    }
}
