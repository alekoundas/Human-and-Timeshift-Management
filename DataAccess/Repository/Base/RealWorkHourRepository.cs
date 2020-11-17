using DataAccess.Models.Entity;
using DataAccess.Repository.Base.Interface;
using DataAccess.Service;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repository.Base
{
    public class RealWorkHourRepository : BaseRepository<RealWorkHour>, IRealWorkHourRepository
    {
        public RealWorkHourRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<RealWorkHour>> GetCurrentAssignedOnCell(int timeShiftId, int year, int month, int day, int employeeId)
        {
            var filter = PredicateBuilder.New<RealWorkHour>();

            if (employeeId != 0)
                filter = filter.And(x => x.Employee.Id == employeeId);

            if (timeShiftId != 0)
                filter = filter.And(x => x.TimeShiftId == timeShiftId);

            filter = filter.And(x => x.StartOn.Year == year);
            filter = filter.And(x => x.StartOn.Month == month);
            filter = filter.And(x => x.StartOn.Day <= day && day <= x.EndOn.Day);

            return await Context.RealWorkHours.Where(filter).ToListAsync();
        }

        public async Task<List<RealWorkHour>> GetCurrentAssignedOnCellFilterByEmployeeIds(GetForEditCellWorkHoursApiViewModel viewModel)
        {
            var filter = PredicateBuilder.New<RealWorkHour>();

            foreach (var employeeId in viewModel.EmployeeIds)
                filter = filter.Or(x => x.EmployeeId == employeeId);

            return await Context.RealWorkHours.Where(filter)
                .Where(x =>
                   x.TimeShiftId == viewModel.TimeShiftId && (
                   x.StartOn.Day == viewModel.CellDay ||
                   x.EndOn.Day == viewModel.CellDay))
                .ToListAsync();
        }

        public bool IsDateOverlaping(ApiRealWorkHoursHasOverlapRange workHour, int employeeId)
        {

            var filter = PredicateBuilder.New<RealWorkHour>();
            var filterOr = PredicateBuilder.New<RealWorkHour>();

            filter = filter.And(x => x.Employee.Id == employeeId);

            filterOr = filterOr.Or(x => x.StartOn <= workHour.StartOn && workHour.StartOn <= x.EndOn);
            filterOr = filterOr.Or(x => x.StartOn <= workHour.EndOn && workHour.EndOn <= x.EndOn);
            filterOr = filterOr.Or(x => workHour.StartOn < x.StartOn && x.EndOn < workHour.EndOn);//not sure

            filter = filter.And(filterOr);

            if (workHour.IsEdit)
                filter = filter.And(x => x.StartOn != workHour.ExcludeStartOn && x.EndOn != workHour.ExcludeEndOn);

            if (workHour.IsDayOff)
                filter = filter.And(x => x.StartOn.Day == workHour.StartOn.Day);

            return Context.RealWorkHours.Any(filter);
        }

        public bool IsDateOvertime(ApiRealWorkHoursHasOvertimeRange workHour, int employeeId)
        {
            var filter = PredicateBuilder.New<RealWorkHour>();
            var filterOr = PredicateBuilder.New<RealWorkHour>();
            workHour.StartOn = workHour.StartOn.AddHours(-11);
            workHour.EndOn = workHour.StartOn.AddHours(11);

            filterOr = filterOr.Or(x => x.StartOn <= workHour.StartOn && workHour.StartOn <= x.EndOn);
            filterOr = filterOr.Or(x => x.StartOn <= workHour.EndOn && workHour.EndOn <= x.EndOn);
            filterOr = filterOr.Or(x => workHour.StartOn < x.StartOn && x.EndOn < workHour.EndOn);

            if (workHour.IsEdit)
                filter = filter.And(x => x.StartOn != workHour.ExcludeStartOn && x.EndOn != workHour.ExcludeEndOn);


            filter = filter.And(x => x.TimeShiftId == workHour.TimeShiftId);
            filter = filter.And(x => x.EmployeeId == employeeId);
            filter = filter.And(filterOr);

            return Context.RealWorkHours.Any(filter);
        }

        public bool AreDatesOverlaping(DateTime startOn, DateTime endOn, int employeeId)
        {
            return Context.RealWorkHours.Where(x =>
                         ((x.StartOn <= startOn && startOn <= x.EndOn) ||
                       (x.StartOn <= endOn && endOn <= x.EndOn) ||
                       (startOn < x.StartOn && x.EndOn < endOn)))
                    .Any(y => y.Employee.Id == employeeId);
        }
        public bool AreDatesOverlapingLeaves(DateTime startOn, DateTime endOn, int employeeId)
        {
            return Context.Leaves.Where(x =>
                       ((x.StartOn <= startOn && startOn <= x.EndOn) ||
                       (x.StartOn <= endOn && endOn <= x.EndOn) ||
                       (startOn < x.StartOn && x.EndOn < endOn)))
                    .Any(y => y.Employee.Id == employeeId);
        }

        public bool AreDatesOverlapingDayOff(DateTime startOn, DateTime endOn, bool isDayOff, int employeeId)
        {
            var filter = PredicateBuilder.New<WorkHour>();

            filter = filter.And(x => x.Employee.Id == employeeId);
            filter = filter.And(x => x.IsDayOff);
            if (isDayOff)
                filter = filter.And(x => startOn.Day == x.StartOn.Day);
            else
                filter = filter.And(x => startOn.Day != x.StartOn.Day);

            return Context.WorkHours
                    .Any(filter);
        }

        public async Task<List<RealWorkHour>> GetCurrentAssignedOnCell(DateTime compareDate, int employeeId)
        {
            var filter = PredicateBuilder.New<RealWorkHour>();
            filter = filter.And(x => x.EmployeeId == employeeId);
            filter = filter.And(x => x.StartOn.Date == compareDate.Date);

            return await Context.RealWorkHours.Where(filter).ToListAsync();
        }

        public async Task<double> GetEmployeeTotalSecondsFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0)
        {
            var filter = PredicateBuilder.New<RealWorkHour>();
            filter = filter.And(x => x.EmployeeId == employeeId);

            if (workplaceId != 0)
                filter = filter.And(x => x.TimeShift.WorkPlaceId == workplaceId);

            filter = filter.And(x =>
                    (startOn.Date <= x.StartOn.Date && x.EndOn.Date <= endOn.Date));

            return Context.RealWorkHours
                .Where(filter)
                .Select(x => (x.EndOn - x.StartOn).TotalSeconds)
                .Select(x => Math.Abs(x))
                .ToList()
                .Sum();
        }

        public async Task<double> GetEmployeeTotalSecondsSaturdayDayFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0)
        {
            var filter = PredicateBuilder.New<RealWorkHour>();
            filter = filter.And(x => x.EmployeeId == employeeId);

            if (workplaceId != 0)
                filter = filter.And(x => x.TimeShift.WorkPlaceId == workplaceId);

            filter = filter.And(x =>
                    (startOn.Date <= x.StartOn.Date && x.EndOn.Date <= endOn.Date));

            return Context.RealWorkHours
                   .Where(filter)
                   .ToList()
                   .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToSaturdayDayWork().TotalSeconds)
                   .Sum();
        }

        public async Task<double> GetEmployeeTotalSecondsSaturdayNightFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0)
        {
            var filter = PredicateBuilder.New<RealWorkHour>();
            filter = filter.And(x => x.EmployeeId == employeeId);

            if (workplaceId != 0)
                filter = filter.And(x => x.TimeShift.WorkPlaceId == workplaceId);

            filter = filter.And(x =>
                    (startOn.Date <= x.StartOn.Date && x.EndOn.Date <= endOn.Date));

            return Context.RealWorkHours
                .Where(filter)
                .ToList()
                .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToSaturdayNightWork().TotalSeconds)
                .Sum();
        }

        public async Task<double> GetEmployeeTotalSecondsSundayDayFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0)
        {
            var filter = PredicateBuilder.New<RealWorkHour>();
            filter = filter.And(x => x.EmployeeId == employeeId);

            if (workplaceId != 0)
                filter = filter.And(x => x.TimeShift.WorkPlaceId == workplaceId);

            filter = filter.And(x =>
                    (startOn.Date <= x.StartOn.Date && x.EndOn.Date <= endOn.Date));

            return Context.RealWorkHours
                 .Where(filter)
                 .ToList()
                 .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToSundayDayWork().TotalSeconds)
                 .Sum();
        }

        public async Task<double> GetEmployeeTotalSecondsSundayNightFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0)
        {
            var filter = PredicateBuilder.New<RealWorkHour>();
            filter = filter.And(x => x.EmployeeId == employeeId);

            if (workplaceId != 0)
                filter = filter.And(x => x.TimeShift.WorkPlaceId == workplaceId);

            filter = filter.And(x =>
                    (startOn.Date <= x.StartOn.Date && x.EndOn.Date <= endOn.Date));

            return Context.RealWorkHours
                 .Where(filter)
                 .ToList()
                 .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToSundayNightWork().TotalSeconds)
                 .Sum();
        }

        public async Task<double> GetEmployeeTotalSecondsDayFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0)
        {
            var endOnNightTimeSpan = new TimeSpan(6, 0, 0);
            var startOnNightTimeSpan = new TimeSpan(22, 0, 0);

            var filter = PredicateBuilder.New<RealWorkHour>();
            filter = filter.And(x => x.EmployeeId == employeeId);

            if (workplaceId != 0)
                filter = filter.And(x => x.TimeShift.WorkPlaceId == workplaceId);

            filter = filter.And(x =>
                startOn.Date <= x.StartOn.Date && x.EndOn.Date <= endOn.Date);

            return Context.RealWorkHours
              .Where(filter)
              .ToList()
              .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToDayWork().TotalSeconds)
              .Sum();
        }

        public async Task<double> GetEmployeeTotalSecondsNightFromRange(int employeeId, DateTime startOn, DateTime endOn, int workplaceId = 0)
        {
            var endOnNightTimeSpan = new TimeSpan(6, 0, 0);
            var startOnNightTimeSpan = new TimeSpan(22, 0, 0);

            var filter = PredicateBuilder.New<RealWorkHour>();
            filter = filter.And(x => x.EmployeeId == employeeId);

            if (workplaceId != 0)
                filter = filter.And(x => x.TimeShift.WorkPlaceId == workplaceId);

            filter = filter.And(x =>
               startOn.Date <= x.StartOn.Date && x.EndOn.Date <= endOn.Date);

            return Context.RealWorkHours
               .Where(filter)
               .ToList()
               .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToNightWork().TotalSeconds)
               .Sum();
        }

        public async Task<double> GetEmployeeTotalSecondsForDay(int employeeId, DateTime compareDate)
        {
            var endOnNight = new DateTime(compareDate.Year, compareDate.Month, compareDate.Day, 6, 0, 0);
            var startOnNight = new DateTime(compareDate.Year, compareDate.Month, compareDate.Day, 22, 0, 0);

            var filter = PredicateBuilder.New<RealWorkHour>();
            filter = filter.And(x => x.EmployeeId == employeeId);
            filter = filter.And(x =>
                compareDate.Date == x.StartOn.Date && x.EndOn.Date == compareDate.Date);

            return Context.RealWorkHours
                .Where(filter)
                .ToList()
                .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToDayWork().TotalSeconds)
                .Sum();
        }

        public async Task<double> GetEmployeeTotalSecondsForNight(int employeeId, DateTime compareDate)
        {
            var endOnNight = new DateTime(compareDate.Year, compareDate.Month, compareDate.Day, 6, 0, 0);
            var startOnNight = new DateTime(compareDate.Year, compareDate.Month, compareDate.Day, 22, 0, 0);

            var filter = PredicateBuilder.New<RealWorkHour>();
            filter = filter.And(x => x.EmployeeId == employeeId);
            filter = filter.And(x => compareDate.Date == x.StartOn.Date);

            return Context.RealWorkHours
                .Where(filter)
                .ToList()
                .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToNightWork().TotalSeconds)
                .Sum();
        }

    }
}
