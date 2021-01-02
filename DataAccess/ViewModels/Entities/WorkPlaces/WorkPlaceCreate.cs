using DataAccess.DataAnnotation.Unique;
using DataAccess.Models.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class WorkPlaceCreate
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Τίτλος")]
        [WorkPlaceValidateUnique("Title")]
        public string Title { get; set; }

        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }

        [Display(Name = "Πελάτης")]
        [WorkPlaceValidateUnique("CustomerId")]
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }


        public bool IsActive { get; set; } = true;

        public static WorkPlace CreateFrom(WorkPlaceCreate viewModel)
        {
            return new WorkPlace()
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                CustomerId = viewModel.CustomerId,
                IsActive = true,
                CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                CreatedOn = DateTime.Now
            };
        }
    }
}
