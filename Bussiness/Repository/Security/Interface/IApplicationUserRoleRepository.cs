using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Bussiness.Repository.Security.Interface
{
    public interface IApplicationUserRoleRepository : ISecurityRepository<ApplicationUserRole>
    {
        Task<List<ApplicationRole>> GetRolesFormLoggedInUserEmail(UserManager<ApplicationUser> userManager, string userEmail);
        Task<List<ApplicationRole>> GetRolesFormUserId(string userId);
        Task<List<ApplicationUserRole>> GetUserRolesToDelete(List<string> idsToDelete, string userId);
    }
}
