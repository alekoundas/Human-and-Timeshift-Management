using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Bussiness.Repository.Security.Interface
{
    public interface IApplicationRoleRepository : ISecurityRepository<ApplicationRole>
    {
        Task<List<string>> GetAvailableControllers();
        Task<List<ApplicationRole>> GetWorkPlaceRolesByUserId(string userId);
        bool IsWorkPlaceIdAnExistingRole(string workPlaceId);
    }
}
