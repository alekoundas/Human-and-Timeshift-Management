using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models.Entity;
using DataAccess.Models.Entity.WorkTimeShift;

namespace DataAccess.Models.Entity
{
    public class WorkPlace : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection< TimeShift >TimeShift { get; set; }
        public ICollection<EmployeeWorkPlace> EmployeeWorkPlaces { get; set; }
    }
}
