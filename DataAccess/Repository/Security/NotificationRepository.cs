using DataAccess.Models.Security;
using DataAccess.Repository.Security.Interface;

namespace DataAccess.Repository.Security
{
    public class NotificationRepository : SecurityRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(SecurityDbContext dbContext) : base(dbContext)
        {
        }

        public SecurityDbContext BaseDbContext
        {
            get { return Context as SecurityDbContext; }
        }
    }
}
