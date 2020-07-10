﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Entity
{
    public class EmployeeWorkPlace : BaseEntity
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int WorkPlaceId { get; set; }
        public WorkPlace WorkPlace { get; set; }
    }
}