using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Models.Entity
{
    public class TimeShift : BaseEntity
    {
        public string  Title { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public int WorkPlaceId { get; set; }
        public WorkPlace WorkPlace { get; set; }
        public ICollection<WorkHour> WorkHours { get; set; }
        public ICollection<RealWorkHour> RealWorkHours { get; set; }


        [NotMapped]
        public int DaysInMonth { get => DateTime.DaysInMonth(Year,Month);  }
    }
}
