using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Models.Identity
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string ApplicationRoleId { get; set; }
        public ApplicationRole ApplicationRole { get; set; }
    }
}
