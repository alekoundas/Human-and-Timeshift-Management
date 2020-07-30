using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models.Entity;

namespace DataAccess.Models.Entity
{
    public class WorkHour : BaseEntity
    {
        public DateTime StartOn { get; set; }
        public DateTime EndOn { get; set; }
        public bool IsDayOff { get; set; }
        public string Comments { get; set; }

        public int TimeShiftId { get; set; }
        public TimeShift TimeShift { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }


    }
}
