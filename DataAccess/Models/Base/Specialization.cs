using DataAccess.Models.Audit;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.Entity
{
    public class Specialization : BaseEntityIsActive
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Όνομα")]
        public string Name { get; set; }

        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PayPerHour { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
