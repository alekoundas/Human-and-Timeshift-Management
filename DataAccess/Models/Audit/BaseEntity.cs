using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.Audit
{
    public class BaseEntity
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

    }
}
