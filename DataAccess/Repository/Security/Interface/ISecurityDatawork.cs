using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository.Security.Interface
{
    public interface ISecurityDatawork : IDisposable
    {
        //public IApplicationRoleRepository ApplicationRoles { get; }
        //public IApplicationTagRepository ApplicationTags { get; }
        //public IApplicationUserRepository ApplicationUsers { get; }
        //public IApplicationUserRoleRepository ApplicationUserRoles { get; }
        //public IApplicationUserTagRepository ApplicationUserTags { get; }
        //public INotificationRepository Notifications { get; }
        Task<int> SaveChangesAsync();
        void Update<TEntity>(TEntity model);
        void UpdateRange<TEntity>(List<TEntity> models);
    }
}
