using System;
using System.Collections.Generic;
using System.Text;
using Archium.Security.Core.Repositories;

namespace Bussiness.Repository.Security.Interface
{
    public interface ISecurityDatawork : IDisposable
    {
        IApplicationRoleRepository ApplicationRoles { get; }
        IApplicationUserRepository ApplicationUsers { get; }
        IApplicationUserRoleRepository ApplicationUserRoles { get; }
        int Complete();
    }
}
