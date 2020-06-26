using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ViewModels;
using Bussiness.Repository.Security.Interface;
using DataAccess;
using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bussiness.Repository.Security
{
    public class ApplicationUserRoleRepository : SecurityRepository<IdentityUserRole<string>>, IApplicationUserRoleRepository
    {
        public ApplicationUserRoleRepository(SecurityDbContext dbContext) : base(dbContext)
        {
        }

        public SecurityDbContext SecurityDbContext
        {
            get { return Context as SecurityDbContext; }
        }

        public async Task<List<ApplicationRole>> GetRolesFormLoggedInUserEmail(UserManager<ApplicationUser> userManager, string userEmail)
        {
            var loggedInUserId = userManager.FindByEmailAsync(userEmail)
                .Result.Id.ToString();

            var userRoles = await SecurityDbContext.UserRoles
                .Where(x =>x.UserId == loggedInUserId)
                .ToListAsync();

            if (userRoles.Count() > 0)
                return SecurityDbContext.Roles.ToList()
                    .Where(y => userRoles.Where(z => z.RoleId == y.Id).Count() > 0)
                    .ToList();
            else
                return new List<ApplicationRole>();
        }

        public async Task<List<ApplicationRole>> GetRolesFormUserId( string userId)
        {

            //var userRoles = await SecurityDbContext.UserRoles
               //.Where(x => x.UserId == userId).ToListAsync();

            //var roles = await SecurityDbContext.Roles.ToListAsync();

            var userRoles =  SecurityDbContext.UserRoles
                .Where(x => x.UserId == userId).ToList();

            var roles =  SecurityDbContext.Roles.ToList();
            if (userRoles.Count() > 0)
                return  roles
                    .Where(y => userRoles.Any(z => z.RoleId == y.Id))
                    .ToList();
            else
                return new List<ApplicationRole>();
        }
    }
}
