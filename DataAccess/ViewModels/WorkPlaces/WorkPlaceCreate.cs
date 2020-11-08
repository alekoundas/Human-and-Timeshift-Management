using DataAccess.Models.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class WorkPlaceCreate
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Τίτλος")]
        public string Title { get; set; }
        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }
        [Display(Name = "Πελάτης")]
        public int? CustomerId { get; set; }



        public static WorkPlace CreateFrom(WorkPlaceCreate viewModel)
        {
            return new WorkPlace()
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                CustomerId = viewModel.CustomerId,
                IsActive = true,
                CreatedOn = DateTime.Now
            };
        }
    }
}
