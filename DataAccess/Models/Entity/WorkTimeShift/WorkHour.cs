using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Entity.WorkTimeShift
{
    public class WorkHour : BaseEntity
    {
        public DateTime StartOn { get; set; }
        public DateTime EndOn { get; set; }

        public int TimeShiftId{ get; set; }
        public TimeShift TimeShift { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee{ get; set; }
    }
}
