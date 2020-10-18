using Bussiness.Repository.Base.Interface;
using DataAccess;
using DataAccess.Models.Entity;

namespace Bussiness.Repository.Base
{
    public class WorkPlaceHourRestrictionRepository : BaseRepository<WorkPlaceHourRestriction>, IWorkPlaceHourRestrictionRepository
    {
        public WorkPlaceHourRestrictionRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }

        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }
    }
}
