using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using DataAccess.Models.Entity;

namespace Bussiness.Repository.Base
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
