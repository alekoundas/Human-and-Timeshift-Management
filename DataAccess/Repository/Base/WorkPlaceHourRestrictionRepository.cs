using DataAccess.Models.Entity;
using DataAccess.Repository.Base.Interface;
using LinqKit;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;

namespace DataAccess.Repository.Base
{
    public class WorkPlaceHourRestrictionRepository : BaseRepository<WorkPlaceHourRestriction>, IWorkPlaceHourRestrictionRepository
    {
        public WorkPlaceHourRestrictionRepository(BaseDbContext dbContext) : base(dbContext)
        {
        }

        public BaseDbContext BaseDbContext
        {
            get { return Context as BaseDbContext; }
        }

        public string GetDayMaxTime(int timeShiftId, int year, int month, int day)
        {
            var hourFilter = PredicateBuilder.New<HourRestriction>();

            hourFilter = hourFilter.And(x => x.WorkPlaceHourRestriction.WorkPlace.TimeShifts.Any(y => y.Id == timeShiftId));
            hourFilter = hourFilter.And(x => x.WorkPlaceHourRestriction.Year == year);
            hourFilter = hourFilter.And(x => x.WorkPlaceHourRestriction.Month == month);
            hourFilter = hourFilter.And(x => x.Day == day);

            return Context.HourRestrictions
                .Where(hourFilter).Select(x => GetTime(x.MaxTicks)).FirstOrDefault();
        }

        public string GetDayRealWorkHoursTime(int timeShiftId, int year, int month, int day)
        {
            var realWorkHourFilter = PredicateBuilder.New<RealWorkHour>();

            realWorkHourFilter = realWorkHourFilter.And(x => x.StartOn.Day == day);
            realWorkHourFilter = realWorkHourFilter.And(x => x.StartOn.Month == month);//Dont need them.. but still
            realWorkHourFilter = realWorkHourFilter.And(x => x.StartOn.Year == year);//Dont need them.. but still
            realWorkHourFilter = realWorkHourFilter.And(x => x.TimeShiftId == timeShiftId);

            var currentSeconds = Context.RealWorkHours
               .Where(realWorkHourFilter)
               .Select(x => Math.Abs(x.StartOn.Subtract(x.EndOn).TotalSeconds))
               .ToList()
               .Sum();

            return GetTime(currentSeconds);
        }

        public bool ValidateMaxHours(int timeShiftId, int year, int month, int day, double secondsToSubmit)
        {
            var hourFilter = PredicateBuilder.New<HourRestriction>();
            var realWorkHourFilter = PredicateBuilder.New<RealWorkHour>();

            hourFilter = hourFilter.And(x => x.WorkPlaceHourRestriction.WorkPlace.TimeShifts.Any(y => y.Id == timeShiftId));
            hourFilter = hourFilter.And(x => x.WorkPlaceHourRestriction.Year == year);
            hourFilter = hourFilter.And(x => x.WorkPlaceHourRestriction.Month == month);
            hourFilter = hourFilter.And(x => x.Day == day);

            var currentDayMaxTime = Context.HourRestrictions
                .Where(hourFilter).Select(x => TimeSpan.FromSeconds(x.MaxTicks)).FirstOrDefault();

            realWorkHourFilter = realWorkHourFilter.And(x => x.StartOn.Day == day);
            realWorkHourFilter = realWorkHourFilter.And(x => x.StartOn.Month == month);//Dont need them.. but still
            realWorkHourFilter = realWorkHourFilter.And(x => x.StartOn.Year == year);//Dont need them.. but still
            realWorkHourFilter = realWorkHourFilter.And(x => x.TimeShiftId == timeShiftId);
            if (currentDayMaxTime.TotalSeconds == 0)
                return true;

            var currentSeconds = Context.RealWorkHours
                .Where(realWorkHourFilter)
                .Select(x => Math.Abs(x.StartOn.Subtract(x.EndOn).TotalSeconds))
                .ToList()
                .Sum();


            return TimeSpan.FromSeconds(currentSeconds + secondsToSubmit) <= currentDayMaxTime;
        }

        private static string GetTime(double seconds)
        {
            var hours = (seconds / 3600).ToString();
            var minutes = (seconds % 3600).ToString();

            if (hours.Length == 1)
                hours = "0" + hours;
            if (minutes.Length == 1)
                minutes = "0" + minutes;

            return hours + ":" + minutes;

        }

    }
}
