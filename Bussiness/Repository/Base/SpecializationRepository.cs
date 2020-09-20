using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Repository.Base.Interface;
using DataAccess;
using DataAccess.Models.Entity;

namespace Bussiness.Repository.Base
{
    public class SpecializationRepository :BaseRepository<Specialization>,ISpecializationRepository
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
