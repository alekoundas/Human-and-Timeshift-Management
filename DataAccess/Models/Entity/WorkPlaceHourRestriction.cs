using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.Entity
{
    public class WorkPlaceHourRestriction : BaseEntity
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Μήνας")]
        public int Month { get; set; }
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Έτος")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Πόστο")]
        public int WorkPlaceId { get; set; }
        public WorkPlace WorkPlace { get; set; }

        [Display(Name = "Περιορσμός μέγιστης εισαγωγής π.βαρδιών")]
        public ICollection<HourRestriction> HourRestrictions { get; set; }
    }
}
