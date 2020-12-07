using DataAccess.Models.Identity;
using DataAccess.Repository.Security.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository.Security
{
    public class ApplicationTagRepository : SecurityRepository<ApplicationTag>, IApplicationTagRepository
    {
        protected readonly SecurityDbContext _dbContext;

        public ApplicationTagRepository(SecurityDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public SecurityDbContext SecurityDbContext
        {
            get { return Context as SecurityDbContext; }
        }

        public ApplicationTag Get(int id)
        {
            return SecurityDbContext.ApplicationTags.FirstOrDefault(x => x.Id == id);
        }

        public async Task<List<ApplicationTag>> GetForUserId(string id)
        {
            return await _dbContext.ApplicationUsers
                .SelectMany(x =>
                    x.ApplicationUserTags.Select(y => y.ApplicationTag)).ToListAsync();
        }
    }
}
