using Bussiness.Repository.Base.Interface;
using DataAccess;
using DataAccess.Models.Entity;

namespace Bussiness.Repository.Base
{
    public class HourRestrictionRepository : BaseRepository<HourRestriction>, IHourRestrictionRepository
    {
        public HourRestrictionRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }

        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }
    }
}
