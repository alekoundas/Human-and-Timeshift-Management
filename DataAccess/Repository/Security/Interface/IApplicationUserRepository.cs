using DataAccess.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository.Security.Interface
{
    public interface IApplicationUserRepository : ISecurityRepository<ApplicationUser>
    {
        ApplicationUser Get(string id);
        Task<List<ApplicationUser>> GetWithPagging(
           Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderingInfo,
           int pageSize = 10,
           int pageIndex = 1);


        Task<ApplicationRole> GetRoleByWorkPlaceAndUser(string workPlaceId, string employeeId);


    }
}
