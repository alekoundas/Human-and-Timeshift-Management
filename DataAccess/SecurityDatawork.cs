using DataAccess;
using DataAccess.Repository;
using DataAccess.Repository.Security;
using DataAccess.Repository.Security.Interface;
using System.Threading.Tasks;

namespace Bussiness
{
    public class SecurityDataWork : ISecurityDatawork
    {
        private readonly SecurityDbContext _dbcontext;
        public SecurityDataWork(SecurityDbContext context)
        {
            _dbcontext = context;

            ApplicationRoles = new ApplicationRoleRepository(_dbcontext);
            ApplicationTags = new ApplicationTagRepository(_dbcontext);
            ApplicationUsers = new ApplicationUserRepository(_dbcontext);
            ApplicationUserRoles = new ApplicationUserRoleRepository(_dbcontext);
            ApplicationUserTags = new ApplicationUserTagRepository(_dbcontext);
        }

        public IApplicationRoleRepository ApplicationRoles { get; }
        public IApplicationTagRepository ApplicationTags { get; }
        public IApplicationUserRepository ApplicationUsers { get; }
        public IApplicationUserRoleRepository ApplicationUserRoles { get; }
        public IApplicationUserTagRepository ApplicationUserTags { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbcontext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbcontext.Dispose();
        }

    }
}
