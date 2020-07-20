using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bussiness.Repository.Base.Interface;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.RealWorkHours;

namespace Bussiness.Repository.Base
{
     public class RealWorkHourRepository : BaseRepository<RealWorkHour>, IRealWorkHourRepository
    {
        public RealWorkHourRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }
        public bool IsDateOverlaping(RealWorkHourApiViewModel realWorkHour,int employeeId)
        {
            return Context.RealWorkHours.Where(x =>
                  (x.StartOn <= realWorkHour.StartOn && realWorkHour.StartOn <= x.EndOn) ||
                  (x.StartOn <= realWorkHour.EndOn && realWorkHour.EndOn <= x.EndOn))
                    .Any(y => y.Employee.Id == employeeId);
        }
    }
}
