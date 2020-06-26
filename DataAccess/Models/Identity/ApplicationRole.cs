using System;
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
    }
}
