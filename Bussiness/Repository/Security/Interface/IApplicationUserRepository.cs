using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bussiness.Repository.Security.Interface;
using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Archium.Security.Core.Repositories
{
    public interface IApplicationUserRepository : ISecurityRepository<ApplicationUser>
    {
        ApplicationUser Get(string id);
        Task<List<ApplicationUser>> GetWithPagging(
           Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderingInfo,
           int pageSize = 10,
           int pageIndex = 1);
        Task<ApplicationUser> UpdateUser(ApplicationUser applicationUser, UserManager<ApplicationUser> userManager);
        //IEnumerable<ApplicationUser> SearchWithProfile(
        //    Expression<Func<ApplicationUser, bool>> predicate,
        //    Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderingInfo = null,
        //    int pageSize = 10,
        //    int pageIndex = 1);

    }
}
