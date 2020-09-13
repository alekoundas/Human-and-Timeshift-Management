using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Repository.Base.Interface;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.RealWorkHours;
using DataAccess.ViewModels.WorkHours;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace Bussiness.Repository.Base
{
    public class RealWorkHourRepository : BaseRepository<RealWorkHour>, IRealWorkHourRepository
    {
        public RealWorkHourRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }
        public async Task< List<RealWorkHour>> GetCurrentAssignedOnCell(int timeShiftId, int year, int month, int day, int employeeId)
        {
            var filter = PredicateBuilder.New<RealWorkHour>();

            if (employeeId != 0)
                filter = filter.And(x => x.Employee.Id == employeeId);

            if (timeShiftId != 0)
                filter = filter.And(x => x.TimeShiftId == timeShiftId);

            filter = filter.And(x => x.StartOn.Year == year);
            filter = filter.And(x => x.StartOn.Month == month);
            filter = filter.And(x => x.StartOn.Day== day);


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
        public bool IsDateOverlaping(HasOverlapRangeWorkHoursApiViewModel workHour, int employeeId)
        {
            if (workHour.IsEdit)
                return Context.RealWorkHours.Where(x =>
                (x.StartOn != workHour.ExcludeStartOn && x.EndOn != workHour.ExcludeEndOn))
                        .Where(x =>
                      (x.StartOn <= workHour.StartOn && workHour.StartOn <= x.EndOn) ||
                      (x.StartOn <= workHour.EndOn && workHour.EndOn <= x.EndOn) ||
                      (workHour.StartOn < x.StartOn && x.EndOn < workHour.EndOn))
                        .Any(y => y.Employee.Id == employeeId);
            else
                return Context.RealWorkHours.Where(x =>
                (x.StartOn <= workHour.StartOn && workHour.StartOn <= x.EndOn) ||
                (x.StartOn <= workHour.EndOn && workHour.EndOn <= x.EndOn) ||
                (workHour.StartOn < x.StartOn && x.EndOn < workHour.EndOn))
                  .Any(y => y.Employee.Id == employeeId);
        }
        //public bool IsDateOverlaping(WorkHoursApiViewModel workHour, int employeeId)
        //{
        //    return Context.RealWorkHours.Where(x =>
        //          (x.StartOn <= workHour.StartOn && workHour.StartOn <= x.EndOn) ||
        //          (x.StartOn <= workHour.EndOn && workHour.EndOn <= x.EndOn))
        //            .Any(y => y.Employee.Id == employeeId);
        //}


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
                .ToList()
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
               .Select(x =>
                   (((x.EndOn <= x.EndOn.Date.Add(startOnNightTimeSpan) ? x.EndOn : x.EndOn.Date.Add(startOnNightTimeSpan)) -
                     (x.StartOn >= x.StartOn.Date.Add(endOnNightTimeSpan) ? x.StartOn : x.StartOn.Date.Add(endOnNightTimeSpan))).TotalSeconds))
                .ToList()
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
               .Select(x =>
                   ((x.EndOn >= x.EndOn.Date.Add(startOnNightTimeSpan) ? x.EndOn : x.EndOn.Date.Add(startOnNightTimeSpan)) - x.EndOn.Date.Add(startOnNightTimeSpan)).TotalSeconds +
                   (x.StartOn.Date.Add(endOnNightTimeSpan) - (x.StartOn <= x.StartOn.Date.Add(endOnNightTimeSpan) ? x.StartOn : x.StartOn.Date.Add(endOnNightTimeSpan))).TotalSeconds
                   )
                .ToList()
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
               .Select(x =>
                   (((x.EndOn <= startOnNight ? x.EndOn : startOnNight) -
                     (x.StartOn >= endOnNight ? x.StartOn : endOnNight)).TotalSeconds))
                .ToList()
                .Sum();
        }

        public async Task<double> GetEmployeeTotalSecondsForNight(int employeeId, DateTime compareDate)
        {
            var endOnNight = new DateTime(compareDate.Year, compareDate.Month, compareDate.Day, 6, 0, 0);
            var startOnNight = new DateTime(compareDate.Year, compareDate.Month, compareDate.Day, 22, 0, 0);

            var filter = PredicateBuilder.New<RealWorkHour>();
            filter = filter.And(x => x.EmployeeId == employeeId);
            filter = filter.And(x => compareDate.Date == x.StartOn.Date);

            var ddd = Context.RealWorkHours.First().StartOn.Date;
            var sdafsdf = Context.RealWorkHours
               .Where(filter)
               .Select(x =>
                    ((x.EndOn >= startOnNight ? x.EndOn : startOnNight) - startOnNight).TotalSeconds +
                    (endOnNight - (x.StartOn <= endOnNight ? x.StartOn : endOnNight)).TotalSeconds
                   )
               .ToList();

            return Context.RealWorkHours
                .Where(filter)
               .Select(x =>
                   ((x.EndOn >= startOnNight ? x.EndOn : startOnNight) - startOnNight).TotalSeconds +
                   (endOnNight - (x.StartOn <= endOnNight ? x.StartOn : endOnNight)).TotalSeconds
                   )
                .ToList()
                .Sum();
        }

    }
}
