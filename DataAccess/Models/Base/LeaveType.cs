using DataAccess.Models.Audit;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.Entity
{
    public class LeaveType : BaseEntityIsActive
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Είδος άδειας")]
        public string Name { get; set; }

        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }

        public ICollection<Leave> Leaves { get; set; }

    }
}
