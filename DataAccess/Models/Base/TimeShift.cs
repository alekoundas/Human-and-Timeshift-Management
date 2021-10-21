using DataAccess.Models.Audit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.Entity
{
    public class TimeShift : BaseEntityIsActive
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Τίτλος")]
        public string Title { get; set; }

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

        public ICollection<Amendment> Amendments { get; set; }
        public ICollection<WorkHour> WorkHours { get; set; }
        public ICollection<RealWorkHour> RealWorkHours { get; set; }


        [NotMapped]
        public int DaysInMonth
        {
            get
            {
                return Year == 0 || Month == 0 ?
                     0
                :
                    DateTime.DaysInMonth(Year, Month);

            }
            set { }
        }
    }
}
