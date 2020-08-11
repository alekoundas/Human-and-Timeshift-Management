using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Repository.Base.Interface;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.RealWorkHours;
using Microsoft.EntityFrameworkCore;

namespace Bussiness.Repository.Base
{
    public class RealWorkHourRepository : BaseRepository<RealWorkHour>, IRealWorkHourRepository
    {
        public RealWorkHourRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }
        public bool AreDatesOverlaping(ApiRealWorkHourHasOverlap realWorkHour, int employeeId)
        {
            return Context.RealWorkHours.Where(x =>
                        ((x.StartOn <= realWorkHour.StartOn && realWorkHour.StartOn <= x.EndOn) ||
                        (x.StartOn <= realWorkHour.EndOn && realWorkHour.EndOn <= x.EndOn) ||
                        (realWorkHour.StartOn < x.StartOn && x.EndOn < realWorkHour.EndOn)))
                    .Any(y => y.Employee.Id == employeeId);
        }
        public bool AreDatesOverlapingLeaves(ApiRealWorkHourHasOverlap realWorkHour, int employeeId)
        {
            return Context.Leaves.Where(x =>
                       ((x.StartOn <= realWorkHour.StartOn && realWorkHour.StartOn <= x.EndOn) ||
                       (x.StartOn <= realWorkHour.EndOn && realWorkHour.EndOn <= x.EndOn) ||
                       (realWorkHour.StartOn < x.StartOn && x.EndOn < realWorkHour.EndOn)))
                    .Any(y => y.Employee.Id == employeeId);
        }
        public bool AreDatesOverlapingDayOff(ApiRealWorkHourHasOverlap realWorkHour, int employeeId)
        {
            return Context.WorkHours.Where(x =>
                       ((x.StartOn <= realWorkHour.StartOn && realWorkHour.StartOn <= x.EndOn) ||
                       (x.StartOn <= realWorkHour.EndOn && realWorkHour.EndOn <= x.EndOn) ||
                       (realWorkHour.StartOn < x.StartOn && x.EndOn < realWorkHour.EndOn)))
                    .Where(y => y.IsDayOff)
                    .Any(y => y.Employee.Id == employeeId);
        }

    }
}
