using DataAccess.Models.Security;
using DataAccess.Repository.Security.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository.Security
{
    public class ApplicationUserTagRepository : SecurityRepository<ApplicationUserTag>, IApplicationUserTagRepository
    {
        public ApplicationUserTagRepository(SecurityDbContext dbContext) : base(dbContext)
        {
        }

        public SecurityDbContext SecurityDbContext
        {
            get { return Context as SecurityDbContext; }
        }

        public async Task<List<ApplicationUserTag>> GetFromUserId(string userId)
        {
            return await SecurityDbContext
                .ApplicationUserTags
                .Where(x => x.ApplicationUserId == userId)
                .ToListAsync();
        }
    }
}
