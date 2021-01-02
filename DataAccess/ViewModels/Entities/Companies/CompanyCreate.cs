using DataAccess.DataAnnotation.Unique;
using DataAccess.Models.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class CompanyCreate
    {
        [CompanyValidateUnique]
        [Display(Name = "ΑΦΜ")]
        public string VatNumber { get; set; }

        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }

        [Display(Name = "Τίτλος")]
        public string Title { get; set; }


        public bool IsActive { get; set; } = true;



        public static Company CreateFrom(CompanyCreate viewModel)
        {
            return new Company()
            {
                Title = viewModel.Title,
                VatNumber = viewModel.VatNumber,
                Description = viewModel.Description,
                IsActive = true,
                CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                CreatedOn = DateTime.Now
            };
        }

    }
}
