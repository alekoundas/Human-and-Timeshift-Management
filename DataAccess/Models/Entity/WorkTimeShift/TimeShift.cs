﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Entity.WorkTimeShift
{
    public class TimeShift : BaseEntity
    {
        public string  Title { get; set; }
        public DateTime StartOn { get; set; }
        public DateTime EndOn { get; set; }

        public int WorkPlaceId { get; set; }
        public WorkPlace WorkPlace { get; set; }
        public ICollection<WorkHour> WorkHours { get; set; }
    }
}