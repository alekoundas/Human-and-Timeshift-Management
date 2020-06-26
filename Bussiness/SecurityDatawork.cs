using System;
using System.Collections.Generic;
using System.Text;
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

        public int Complete()
        {
            return _dbcontext.SaveChanges();
        }

        public void Dispose()
        {
            _dbcontext.Dispose();
        }

    }
}
