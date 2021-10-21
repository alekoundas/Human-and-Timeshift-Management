using DataAccess.Models.Audit;
using System;

namespace DataAccess.Models.Entity
{
    public class Amendment : BaseEntity
    {
        public DateTime NewStartOn { get; set; }
        public DateTime NewEndOn { get; set; }
        public string Comments { get; set; }


        public int? RealWorkHourId { get; set; }
        public RealWorkHour RealWorkHour { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int TimeShiftId { get; set; }
        public TimeShift TimeShift { get; set; }

    }
}
