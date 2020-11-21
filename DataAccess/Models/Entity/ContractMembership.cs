using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.Entity
{
    public class ContractMembership : BaseEntityIsActive
    {
        [Required]
        [Display(Name = "Όνομα")]
        public string Name { get; set; }

        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }

        public ICollection<Contract> Contracts { get; set; }



    }
}
