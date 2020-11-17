using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository.Security.Interface
{
    public interface IApplicationUserRoleRepository : ISecurityRepository<ApplicationUserRole>
    {
        Task<List<ApplicationRole>> GetRolesFormLoggedInUserEmail(UserManager<ApplicationUser> userManager, string userEmail);
        List<ApplicationRole> GetRolesFormUserId(string userId);
        Task<List<ApplicationUserRole>> GetUserRolesToDelete(List<string> idsToDelete, string userId);
    }
}
