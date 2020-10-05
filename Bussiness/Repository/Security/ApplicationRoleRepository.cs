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
using LinqKit;
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

        public async Task<List<ApplicationRole>> GetWorkPlaceRolesByUserId(string userId)
        {
            var filter = PredicateBuilder.New<ApplicationRole>();

            var userRoles = await SecurityDbContext.UserRoles
                .Where(x => x.UserId == userId).ToListAsync();

            //So filter stays false and dont get anything
            if (userRoles.Count > 0)
            {
                foreach (var userRole in userRoles)
                    filter = filter.Or(x => x.Id == userRole.RoleId);

                filter = filter.And(x => x.Name.Contains("Specific_WorkPlace"));
            }
            return SecurityDbContext.Roles
                .Where(filter)
                .ToList();
        }
        public async Task<List<string>> GetAvailableControllers()
        {
            return await SecurityDbContext.Roles.Select(x => x.Controller)
                .Distinct().ToListAsync();
        }

        public bool IsWorkPlaceIdAnExistingRole(string workPlaceId)
        {
            return SecurityDbContext.Roles.Any(x => x.WorkPlaceId == workPlaceId);
        }

    }
}
