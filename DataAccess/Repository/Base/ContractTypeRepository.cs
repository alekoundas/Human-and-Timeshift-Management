using DataAccess.Models.Entity;
using DataAccess.Repository.Base.Interface;

namespace DataAccess.Repository.Base
{
    public class ContractTypeRepository : BaseRepository<ContractType>, IContractTypeRepository
    {
        public ContractTypeRepository(BaseDbContext dbContext) : base(dbContext)
        {

        }
        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }
    }
}
