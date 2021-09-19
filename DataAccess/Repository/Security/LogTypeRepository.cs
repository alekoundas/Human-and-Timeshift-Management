using DataAccess.Models.Security;
using DataAccess.Repository.Security.Interface;

namespace DataAccess.Repository.Security
{
    public class LogTypeRepository : SecurityRepository<LogType>, ILogTypeRepository
    {
        public LogTypeRepository(SecurityDbContext dbContext) : base(dbContext)
        {
        }

        public SecurityDbContext SecurityDbContext
        {
            get { return Context as SecurityDbContext; }
        }
    }
}
