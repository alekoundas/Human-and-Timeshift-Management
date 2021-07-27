using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.Audit
{
    public class BaseEntity
    {
        public int Id { get; set; }

        //Audit
        [Display(Name = "Δημιουργήθηκε απο")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public string CreatedBy_Id { get; set; }

        [Display(Name = "Δημιουργήθηκε απο")]
        public string CreatedBy_FullName { get; set; }

        [Display(Name = "Δημιουργήθηκε στις")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public DateTime CreatedOn { get; set; }

    }
}
