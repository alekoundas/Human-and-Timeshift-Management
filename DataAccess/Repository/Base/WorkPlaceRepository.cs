using DataAccess.Models.Entity;
using DataAccess.Repository.Base.Interface;

namespace DataAccess.Repository.Base
{
    public class WorkPlaceRepository : BaseRepository<WorkPlace>, IWorkplaceRepository
    {
        public WorkPlaceRepository(BaseDbContext baseDbContext) : base(baseDbContext)
        {
        }

        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }
    }
}
