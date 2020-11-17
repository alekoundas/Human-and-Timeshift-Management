using DataAccess.Models.Entity;
using DataAccess.Repository.Base.Interface;

namespace DataAccess.Repository.Base
{
    public class SpecializationRepository : BaseRepository<Specialization>, ISpecializationRepository
    {
        public SpecializationRepository(BaseDbContext dbContext) : base(dbContext)
        {

        }
        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }

    }
}
