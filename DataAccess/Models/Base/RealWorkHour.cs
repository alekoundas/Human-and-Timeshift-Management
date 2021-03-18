using DataAccess.Models.Audit;
using System;

namespace DataAccess.Models.Entity
{
    public class RealWorkHour : BaseEntity
    {
        public DateTime StartOn { get; set; }
        public DateTime EndOn { get; set; }

        public int TimeShiftId { get; set; }
        public TimeShift TimeShift { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string Comments { get; set; }
    }
}
