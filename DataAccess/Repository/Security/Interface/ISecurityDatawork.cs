using System;
using System.Threading.Tasks;

namespace DataAccess.Repository.Security.Interface
{
    public interface ISecurityDatawork : IDisposable
    {
        IApplicationRoleRepository ApplicationRoles { get; }
        IApplicationUserRepository ApplicationUsers { get; }
        IApplicationUserRoleRepository ApplicationUserRoles { get; }
        Task<int> SaveChangesAsync();
    }
}
