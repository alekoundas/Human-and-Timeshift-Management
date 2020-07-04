using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Repository.Base.Interface;
using DataAccess;
using DataAccess.Models.Datatable;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.View;
using Microsoft.EntityFrameworkCore;

namespace Bussiness.Repository.Base
{
    public class WorkHourRepository : BaseRepository<WorkHour>, IWorkHourRepository
    {
        public WorkHourRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }

        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }

        public List<WorkHour> GetCurrentAssignedOnCell(int timeShiftId, int year, int month, int day, int employeeId)
        {
            return  Context.WorkHours.Where(x =>
                    x.TimeShiftId == timeShiftId &&
                    x.StartOn.Year == year &&
                    x.StartOn.Month == month &&
                    (x.StartOn.Day <= day && day <= x.EndOn.Day) &&
                    x.Employee.Id == employeeId)
                .ToList();
        }

        public bool IsDateOverlaps(WorkHoursApiViewModel workHour)
        {

            return Context.WorkHours.Where(x =>
                  x.TimeShiftId == workHour.TimeShiftId &&
                  (x.StartOn <= workHour.StartOn && workHour.StartOn <= x.EndOn) ||
                  (x.StartOn <= workHour.EndOn && workHour.EndOn <= x.EndOn))
                    .Any(y => y.Employee.Id == workHour.EmployeeId);

        }

       
    }
}
