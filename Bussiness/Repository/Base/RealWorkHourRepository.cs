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
using LinqKit;
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

        public async Task<double> GetEmployeeTotalSecondsFromRange(int employeeId, DateTime startOn, DateTime endOn)
        {
            var filter = PredicateBuilder.True<RealWorkHour>();
            filter = filter.And(x => x.EmployeeId == employeeId);
            filter = filter.And(x =>
                    (startOn.Date <= x.StartOn.Date && x.EndOn.Date <= endOn.Date));


            return Context.RealWorkHours
                .Where(filter)
                .Select(x => (x.EndOn - x.StartOn).TotalSeconds)
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


            //var asdfasdf = Context.RealWorkHours
            //    .Where(filter)
            //   .Select(x =>
            //       ( ( (x.EndOn <= startOnNight ? x.EndOn : startOnNight) - (x.StartOn <= endOnNight ? x.StartOn : endOnNight)).TotalSeconds);



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
            filter = filter.And(x =>  compareDate.Date == x.StartOn.Date );

            var   ddd =Context.RealWorkHours.First().StartOn.Date;
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
