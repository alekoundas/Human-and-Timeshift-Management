using DataAccess.Models.Security;
using DataAccess.Repository.Security.Interface;

namespace DataAccess.Repository.Security
{
    public class LogRepository : SecurityRepository<Log>, ILogRepository
    {
        public LogRepository(SecurityDbContext dbContext) : base(dbContext)
        {
        }

        public SecurityDbContext BaseDbContext
        {
            get { return Context as SecurityDbContext; }
        }
    }
}
