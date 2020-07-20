using System;
using System.Collections.Generic;
using System.Text;
using Bussiness.Repository.Base.Interface;
using DataAccess;
using DataAccess.Models.Entity;

namespace Bussiness.Repository.Base
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
