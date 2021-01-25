using DataAccess.Models.Entity;
using DataAccess.Repository.Base.Interface;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository.Base
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

        public async Task<List<WorkHour>> GetCurrentDayOffAssignedOnCell(DateTime compareDate, int employeeId)
        {
            var filter = PredicateBuilder.New<WorkHour>();
            filter = filter.And(x => x.IsDayOff == true);
            filter = filter.And(x => x.EmployeeId == employeeId);
            filter = filter.And(x => x.StartOn.Date == compareDate.Date);

            return await Context.WorkHours.Where(filter).ToListAsync();
        }
        public async Task<List<WorkHour>> GetCurrentAssignedOnCell(int timeShiftId, int? year, int? month, int day, int employeeId)
        {
            return await Context.WorkHours.Where(x =>
                   x.TimeShiftId == timeShiftId &&
                   x.StartOn.Year == year &&
                   x.StartOn.Month == month &&
                   (x.StartOn.Day <= day && day <= x.EndOn.Day) &&
                   x.Employee.Id == employeeId)
                .ToListAsync();
        }
        public async Task<List<WorkHour>> GetCurrentAssignedOnCellFilterByEmployeeIds(GetForEditCellWorkHoursApiViewModel viewModel)
        {
            var filter = PredicateBuilder.New<WorkHour>();

            foreach (var employeeId in viewModel.EmployeeIds)
                filter = filter.Or(x => x.EmployeeId == employeeId);

            return await Context.WorkHours.Where(filter)
                .Where(x =>
                   x.TimeShiftId == viewModel.TimeShiftId && (
                   x.StartOn.Day == viewModel.CellDay ||
                   x.EndOn.Day == viewModel.CellDay))
                .ToListAsync();
        }

        public bool IsDateOverlaping(WorkHourApiViewModel workHour, int employeeId)
        {
            var filterOr = PredicateBuilder.New<WorkHour>();
            var filter = PredicateBuilder.New<WorkHour>();

            filterOr = filterOr.Or(x => x.StartOn <= workHour.StartOn && workHour.StartOn <= x.EndOn);
            filterOr = filterOr.Or(x => x.StartOn <= workHour.EndOn && workHour.EndOn <= x.EndOn);
            filterOr = filterOr.Or(x => workHour.StartOn < x.StartOn && x.EndOn < workHour.EndOn);//Not sure

            filter = filter.And(x => x.Employee.Id == employeeId);
            filter = filter.And(filterOr);

            return Context.WorkHours.Any(filter);
        }
        public bool IsDateOverlaping(ApiRealWorkHoursHasOverlapRange workHour, int employeeId)
        {
            var filterOr = PredicateBuilder.New<WorkHour>();
            var filter = PredicateBuilder.New<WorkHour>();

            filterOr = filterOr.Or(x => x.StartOn <= workHour.StartOn && workHour.StartOn <= x.EndOn);
            filterOr = filterOr.Or(x => x.StartOn <= workHour.EndOn && workHour.EndOn <= x.EndOn);
            filterOr = filterOr.Or(x => workHour.StartOn < x.StartOn && x.EndOn < workHour.EndOn);

            if (workHour.IsDayOff)
                filter = filter
                    .And(x => x.StartOn.Day == workHour.StartOn.Day);

            if (workHour.IsEdit)
                filter = filter.And(x => x.StartOn != workHour.ExcludeStartOn && x.EndOn != workHour.ExcludeEndOn);

            filter = filter.And(x => x.Employee.Id == employeeId);
            filter = filter.And(x => !(workHour.StartOn.Day != x.StartOn.Day && x.IsDayOff == true));
            filter = filter.And(filterOr);

            //var asdfasdf = Context.WorkHours.Where(filter);
            return Context.WorkHours.Any(filter);
        }

        public bool IsDateOvertime(WorkHourHasOvertimeRange workHour, int employeeId)
        {

            var filter = PredicateBuilder.New<WorkHour>();
            var filterOr = PredicateBuilder.New<WorkHour>();
            workHour.StartOn = workHour.StartOn.AddHours(-11);
            workHour.EndOn = workHour.EndOn.AddHours(11);

            filterOr = filterOr.Or(x => x.StartOn <= workHour.StartOn && workHour.StartOn <= x.EndOn);
            filterOr = filterOr.Or(x => x.StartOn <= workHour.EndOn && workHour.EndOn <= x.EndOn);
            filterOr = filterOr.Or(x => workHour.StartOn < x.StartOn && x.EndOn < workHour.EndOn);

            if (workHour.IsEdit)
                filter = filter.And(x => x.StartOn != workHour.ExcludeStartOn && x.EndOn != workHour.ExcludeEndOn);


            filter = filter.And(x => x.TimeShiftId == workHour.TimeShiftId);
            filter = filter.And(x => x.EmployeeId == employeeId);
            filter = filter.And(filterOr);

            return Context.WorkHours.Any(filter);
        }

        public bool HasExactDate(WorkHourApiViewModel workHour)
        {
            return Context.WorkHours.Any(x =>
                  x.TimeShiftId == workHour.TimeShiftId &&
                  x.StartOn == workHour.StartOn && workHour.EndOn == x.EndOn);
        }

    }
}
