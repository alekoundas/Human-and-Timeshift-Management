using DataAccess.Models.Security;
using DataAccess.Repository.Security.Interface;

namespace DataAccess.Repository.Security
{
    public class LogEntityRepository : SecurityRepository<LogEntity>, ILogEntityRepository
    {
        public LogEntityRepository(SecurityDbContext dbContext) : base(dbContext)
        {
        }

        public SecurityDbContext SecurityDbContext
        {
            get { return Context as SecurityDbContext; }
        }
    }
}
