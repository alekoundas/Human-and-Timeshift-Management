using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models.Entity.WorkTimeShift;

namespace DataAccess.Models.Entity
{
    public class WorkPlace : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int MyProperty { get; set; }

        public ICollection< TimeShift >TimeShift { get; set; }
    }
}
