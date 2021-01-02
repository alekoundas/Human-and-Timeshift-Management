using DataAccess.Models.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class CompanyEdit : CompanyCreate
    {
        public int Id { get; set; }

        //Audit
        [Display(Name = "Δημηουργήθηκε απο")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string CreatedBy_Id { get; set; }

        [Display(Name = "Δημηουργήθηκε απο")]
        public string CreatedBy_FullName { get; set; }

        [Display(Name = "Δημηουργήθηκε στις")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public DateTime CreatedOn { get; set; }

        public static CompanyEdit CreateFrom(Company model)
        {
            return new CompanyEdit()
            {
                Id = model.Id,
                Title = model.Title,
                VatNumber = model.VatNumber,
                Description = model.Description,
                IsActive = model.IsActive,
                CreatedBy_FullName = model.CreatedBy_FullName,
                CreatedBy_Id = model.CreatedBy_Id,
                CreatedOn = model.CreatedOn
            };
        }

        public static Company CreateFrom(CompanyEdit model)
        {
            return new Company()
            {
                Id = model.Id,
                Title = model.Title,
                VatNumber = model.VatNumber,
                Description = model.Description,
                IsActive = model.IsActive,
                CreatedBy_FullName = model.CreatedBy_FullName,
                CreatedBy_Id = model.CreatedBy_Id,
                CreatedOn = model.CreatedOn
            };
        }
    }
}
