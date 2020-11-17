using DataAccess.Models.Entity;
using DataAccess.Repository.Base.Interface;

namespace DataAccess.Repository.Base
{
    public class EmployeeWorkPlaceRepository : BaseRepository<EmployeeWorkPlace>, IEmployeeWorkPlaceRepository
    {
        public EmployeeWorkPlaceRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }

        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }

    }
}
