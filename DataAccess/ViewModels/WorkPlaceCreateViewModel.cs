using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DataAccess.Models.Entity;

namespace DataAccess.ViewModels
{
    public class WorkPlaceCreateViewModel
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Τίτλος")]
        public string Title { get; set; }
        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }
        [Display(Name = "Πελάτης")]
        public int? CustomerId { get; set; }



        public static WorkPlace CreateFrom(WorkPlaceCreateViewModel viewModel)
        {
            return new WorkPlace()
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                CustomerId = viewModel.CustomerId,
                CreatedOn = DateTime.Now
            };
        }
    }
}
