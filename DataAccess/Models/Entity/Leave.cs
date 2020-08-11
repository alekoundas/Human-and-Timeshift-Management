using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Entity
{
    public class Leave:BaseEntity
    {
        public DateTime StartOn{ get; set; }
        public DateTime EndOn{ get; set; }
        public string Description { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

    }
}
