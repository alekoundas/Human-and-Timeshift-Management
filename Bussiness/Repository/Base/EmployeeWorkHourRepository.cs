using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Repository.Base.Interface;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.View;
using Microsoft.EntityFrameworkCore;

namespace Bussiness.Repository.Base
{
    public class EmployeeWorkHourRepository : BaseRepository<EmployeeWorkHour>, IEmployeeWorkHourRepository
    {
        public EmployeeWorkHourRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }

        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }

        public async Task<bool> IsDateOverlaps(WorkHoursApiViewModel workHour)
        {
            //var isDateOverlapping = await Context.WorkHours.Where(x =>
            //         x.TimeShiftId == workHour.TimeShiftId &&
            //         (x.StartOn <= workHour.StartOn && workHour.StartOn <= x.EndOn) ||
            //         (x.StartOn <= workHour.EndOn && workHour.EndOn <= x.EndOn))
            //          .Where(y => y.EmployeeWorkHours.Any(z => z.EmployeeId == workHour.EmployeeId)).ToListAsync();
            return Context.WorkHours.Where(x =>
                  x.TimeShiftId == workHour.TimeShiftId &&
                  (x.StartOn <= workHour.StartOn && workHour.StartOn <= x.EndOn) ||
                  (x.StartOn <= workHour.EndOn && workHour.EndOn <= x.EndOn))
                    .Any(y => y.EmployeeWorkHours.Any(z => z.EmployeeId == workHour.EmployeeId));

        }

    }
}
