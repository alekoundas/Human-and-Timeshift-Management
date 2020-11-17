using DataAccess.Models.Entity;
using DataAccess.Repository.Base.Interface;

namespace DataAccess.Repository.Base
{
    public class LeaveTypeRepository : BaseRepository<LeaveType>, ILeaveTypeRepository
    {
        public LeaveTypeRepository(BaseDbContext dbContext) : base(dbContext)
        {

        }

        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }
    }
}
