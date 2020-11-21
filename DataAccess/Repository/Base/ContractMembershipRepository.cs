﻿using DataAccess.Models.Entity;
using DataAccess.Repository.Base.Interface;

namespace DataAccess.Repository.Base
{
    public class ContractMembershipRepository : BaseRepository<ContractMembership>, IContractMembershipRepository
    {
        public ContractMembershipRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }

        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }
    }
}
