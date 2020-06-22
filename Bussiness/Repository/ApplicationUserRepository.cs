using System;
using System.Collections.Generic;
using System.Text;
using Bussiness.Repository;
using Bussiness.Repository.Interface;
using DataAccess;
using DataAccess.Models.Identity;

namespace Business.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(BaseDbContext dbContext) : base(dbContext)
        {

        }
    }
}
