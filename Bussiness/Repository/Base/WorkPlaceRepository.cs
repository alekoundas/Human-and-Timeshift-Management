using System;
using System.Collections.Generic;
using System.Text;
using Bussiness.Repository.Base.Interface;
using DataAccess;
using DataAccess.Models.Entity;

namespace Bussiness.Repository.Base
{
    public class WorkPlaceRepository:BaseRepository<WorkPlace>,IWorkplaceRepository
    {
        public WorkPlaceRepository(BaseDbContext baseDbContext):base(baseDbContext)
        {
        }

        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }
    }
}
