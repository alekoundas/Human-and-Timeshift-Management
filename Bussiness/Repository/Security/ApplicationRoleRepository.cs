using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Archium.Security.Core.Repositories;
using Bussiness.Repository.Security.Interface;
using DataAccess;
using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bussiness.Repository.Security
{
    public class ApplicationRoleRepository : SecurityRepository<ApplicationRole>, IApplicationRoleRepository
    {
        protected readonly SecurityDbContext _dbContext;
        public ApplicationRoleRepository(SecurityDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public SecurityDbContext SecurityDbContext 
        {
            get { return Context as SecurityDbContext; }
        }

        public async Task<List<string>> GetAvailableControllers()
        {
            return await SecurityDbContext.Roles.Select(x => x.Controller)
                .Distinct().ToListAsync();
        }

    }
}
