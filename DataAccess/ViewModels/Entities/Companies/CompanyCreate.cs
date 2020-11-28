using DataAccess.DataAnnotation.Unique;
using DataAccess.Models.Entity;
using System;

namespace DataAccess.ViewModels
{
    public class CompanyCreate
    {
        [CompanyValidateUnique]
        public string VatNumber { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }



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
