using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class EmployeeContact:Entity
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
    }
}
