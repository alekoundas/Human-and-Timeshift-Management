using DataAccess.Models.Security;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repository.Security.Interface
{
    public interface IApplicationTagRepository : ISecurityRepository<ApplicationTag>
    {
        ApplicationTag Get(int id);
        Task<List<ApplicationTag>> GetForUserId(string id);
    }
}
