using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Archium.Security.Core.Repositories;
using Business.Repository;
using Bussiness.Repository.Security;
using Bussiness.Repository.Security.Interface;
using DataAccess;

namespace Bussiness
{
    public class SecurityDataWork : ISecurityDatawork
    {
        private readonly SecurityDbContext _dbcontext;
        public SecurityDataWork(SecurityDbContext context)
        {
            _dbcontext = context;

            ApplicationRoles = new ApplicationRoleRepository(_dbcontext);
            ApplicationUsers = new ApplicationUserRepository(_dbcontext);
            ApplicationUserRoles = new ApplicationUserRoleRepository(_dbcontext);


        }

        public IApplicationRoleRepository ApplicationRoles { get; }
        public IApplicationUserRepository ApplicationUsers { get; }
        public IApplicationUserRoleRepository ApplicationUserRoles { get; }

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
