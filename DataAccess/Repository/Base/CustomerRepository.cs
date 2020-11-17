using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Repository.Base.Interface;
using DataAccess;
using DataAccess.Models.Entity;

namespace DataAccess.Repository.Base
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }

        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }
    }
}
