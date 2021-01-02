using DataAccess.Models.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class WorkPlaceEdit : WorkPlaceCreate
    {
        public int Id { get; set; }

        //Audit
        [Display(Name = "Δημηουργήθηκε απο")]
        public string CreatedBy_Id { get; set; }

        [Display(Name = "Δημηουργήθηκε απο")]
        public string CreatedBy_FullName { get; set; }

        [Display(Name = "Δημηουργήθηκε στις")]
        public DateTime CreatedOn { get; set; }

        public static WorkPlaceEdit CreateFrom(WorkPlace model)
        {
            return new WorkPlaceEdit()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                IsActive = model.IsActive,
                CustomerId = model.CustomerId,
                CreatedBy_FullName = model.CreatedBy_FullName,
                CreatedBy_Id = model.CreatedBy_Id,
                CreatedOn = model.CreatedOn
            };
        }

        public static WorkPlace CreateFrom(WorkPlaceEdit model)
        {
            return new WorkPlace()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                IsActive = model.IsActive,
                CustomerId = model.CustomerId,
                CreatedBy_FullName = model.CreatedBy_FullName,
                CreatedBy_Id = model.CreatedBy_Id,
                CreatedOn = model.CreatedOn
            };
        }
    }
}
