using System;
using System.Collections.Generic;
using System.Text;
using Archium.Security.Core.Repositories;
using Bussiness.Repository.Security.Interface;
using DataAccess;

namespace Business.Repository
{
    public class SecurityDatawork : ISecurityDatawork
    {
        public IApplicationUserRepository ApplicationUser{ get; private set; }

        public IApplicationUserRepository ApplicationUsers => throw new NotImplementedException();

        public SecurityDatawork(SecurityDbContext dbContext)
        {
            ApplicationUser = new ApplicationUserRepository(dbContext);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}