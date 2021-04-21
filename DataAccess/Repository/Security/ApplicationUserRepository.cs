using DataAccess.Models.Security;
using DataAccess.Repository.Security.Interface;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ApplicationUserRepository : SecurityRepository<ApplicationUser>, IApplicationUserRepository
    {
        protected readonly SecurityDbContext _dbContext;

        public ApplicationUserRepository(SecurityDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public SecurityDbContext SecurityDbContext
        {
            get { return Context as SecurityDbContext; }
        }

        public ApplicationUser Get(string id)
        {
            return SecurityDbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public async Task<List<string>> GetAllIds()
        {
            return await SecurityDbContext.Users.Select(x => x.Id).ToListAsync();
        }

        public async Task<List<ApplicationUser>> GetWithPagging(
            Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderingInfo,
            int pageSize = 10,
            int pageIndex = 1)
        {
            if (orderingInfo == null)
                return await SecurityDbContext
                    .Users
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize).ToListAsync();

            var query = (IQueryable<ApplicationUser>)orderingInfo(SecurityDbContext.Users);

            if (pageSize != -1)
                query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }



        public async Task<ApplicationRole> GetRoleByWorkPlaceAndUser(string workPlaceId, string userId)
        {
            var filter = PredicateBuilder.New<ApplicationRole>();

            var userRoles = await SecurityDbContext.UserRoles
                .Where(x => x.UserId == userId)
                .ToListAsync();

            foreach (var userRole in userRoles)
                filter = filter.Or(x => x.Id == userRole.RoleId && x.WorkPlaceId == workPlaceId);

            return await SecurityDbContext.Roles.FirstOrDefaultAsync(filter);
        }



        public void DeleteUser(int id)
        {
            var user = SecurityDbContext.Users.Find(id);
            if (user != null)
            {
                SecurityDbContext.Users.Remove(user);
                SecurityDbContext.SaveChanges();
            }
            else
                throw new Exception("Failed to delete user");
        }
    }
}
