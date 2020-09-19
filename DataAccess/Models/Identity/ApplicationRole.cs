﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Models.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public string Controller { get; set; }
        public string Permition { get; set; }
        public string WorkPlaceName { get; set; }
        public string WorkPlaceId { get; set; }
        public ICollection<ApplicationUserRole> EmployeeWorkPlaces { get; set; }

    }
}
