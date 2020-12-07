using DataAccess.Models.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository.Security.Interface
{
    public interface IApplicationUserTagRepository : ISecurityRepository<ApplicationUserTag>
    {
        Task<List<ApplicationUserTag>> GetFromUserId(string userId);
    }
}
