using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.Entity
{
    public class WorkPlace : BaseEntity
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Τίτλος")]
        public string Title { get; set; }

        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }

        [Display(Name = "Πελάτης")]
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<TimeShift> TimeShifts { get; set; }
        public ICollection<EmployeeWorkPlace> EmployeeWorkPlaces { get; set; }
        public ICollection<WorkPlaceHourRestriction> WorkPlaceHourRestrictions { get; set; }
    }
}
