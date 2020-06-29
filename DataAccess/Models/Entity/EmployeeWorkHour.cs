using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Entity
{
    public class EmployeeWorkHour : BaseEntity
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int WorkHourId { get; set; }
        public WorkHour WorkHour { get; set; }
    }
}
