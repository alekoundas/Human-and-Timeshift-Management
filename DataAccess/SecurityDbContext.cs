using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DataAccess
{
    public class SecurityDbContext : IdentityDbContext<ApplicationUser, 
        ApplicationRole, string,IdentityUserClaim<string>, ApplicationUserRole, 
        IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options)
        : base(options)
        {
        }
    }
}


