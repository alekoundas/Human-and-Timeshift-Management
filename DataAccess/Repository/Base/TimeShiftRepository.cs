using DataAccess.Models.Entity;
using DataAccess.Repository.Base.Interface;

namespace DataAccess.Repository.Base
{
    public class TimeShiftRepository : BaseRepository<TimeShift>, ITimeShiftRepository
    {
        public TimeShiftRepository(BaseDbContext dbContext) : base(dbContext)
        {

        }
        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }
    }
}
