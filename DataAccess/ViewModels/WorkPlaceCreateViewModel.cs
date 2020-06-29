using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DataAccess.Models.Entity;
using DataAccess.Models.Entity.WorkTimeShift;

namespace DataAccess.ViewModels
{
    public class WorkPlaceCreateViewModel
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
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
