using DataAccess.Models.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository.Security.Interface
{
    public interface IApplicationRoleRepository : ISecurityRepository<ApplicationRole>
    {
        Task<List<string>> GetAvailableControllers();
        Task<List<ApplicationRole>> GetWorkPlaceRolesByUserId(string userId);
        bool IsWorkPlaceIdAnExistingRole(string workPlaceId);
    }
}
